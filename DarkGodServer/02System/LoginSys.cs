
/****************************************************
	文件：LoginSys.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:17   	
	功能：登陆业务系统层
*****************************************************/

using PENet;
using Protocol;

public class LoginSys:SystemRoot<LoginSys>
{ 
    public override void Init() {
        PETool.LogMsg("LoginSys Init Done");
    }

    public void HandleReqLogin(MsgPack pack) {
        //处理客户端登陆请求
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.RspLogin,
        };

        ReqLogin loginData = pack.msg.ReqLogin;
        string acct = loginData.account;
        string password = loginData.password;

        bool isOnline = CacheSvc.Instance.IsOnline(acct);
        if (isOnline)
        {
            newMsg.err = (int)ErrorCode.AlreadyOnline;
        }
        else
        {
            PlayerData pd = CacheSvc.Instance.GetPlayerData(acct, password);  
            if (pd == null)
            {
                newMsg.err = (int)ErrorCode.InvalidPassword;
            }
            else {
                
                //计算玩家体力恢复
                long now = TimerSvc.Instance.GetNowTime();
                int timeInterval = (int)(now - pd.Time);

                int addPower = (timeInterval / (PECommonTool.AddPowerTimeSpan * 1000 * 60)) *
                    PECommonTool.AddPowerPerTimes;
                PECommonTool.Log(addPower+ " -- " +"before:  " + pd.Power.ToString(), LogType.Info);
                
                if (addPower > 0)
                {
                    pd.Power += (int)addPower;
                    if (pd.Power > PECommonTool.GetPowerLimit(pd.Level)) {
                        pd.Power = PECommonTool.GetPowerLimit(pd.Level);
                    }
                    if (!CacheSvc.Instance.UpdatePlayerDataToDB(pd.ID, pd)) {
                        PECommonTool.Log("update player time error when login", LogType.Error);
                    }
                }
                PECommonTool.Log("after: "+pd.Power.ToString(), LogType.Info);
                

                newMsg.RspLogin = new RspLogin
                {
                    data = pd,
                };

                CacheSvc.Instance.CachePlayerData(acct, pd, pack.session);
            }
        }
        pack.session.SendMsg(newMsg); 
    }



    /// <summary>
    /// 处理修改名字请求
    /// </summary> 
    public void HandleReqRename(MsgPack pack) {
        ReqRename reqData = pack.msg.ReqRename;
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.RspRename,
        };

        //检查名字是否已经存在
        bool check = CacheSvc.Instance.CheckName(reqData.name);
        if (check)
        {
            newMsg.err = (int)ErrorCode.NameExisted;
        }
        else {
            PlayerData data = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
            data.Name = reqData.name;
             
            if (CacheSvc.Instance.UpdatePlayerDataToDB(data.ID, data))
            {
                newMsg.RspRename = new RspRename
                {
                    name=reqData.name,
                };
            }
            else {
                newMsg.err = (int)ErrorCode.UpdateDBFailed;
            }
        }
        pack.session.SendMsg(newMsg);
    }

    public void Offline(ServerSession ses, int sesID) { 
        //下线时，先更新下线时间
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(ses);
        if (pd != null) {
            pd.Time = TimerSvc.Instance.GetNowTime();
            if (!CacheSvc.Instance.UpdatePlayerDataToDB(pd.ID, pd)) {
                PECommonTool.Log("update player time error when offline", LogType.Error);
            }
        }
        bool offlienSuc = CacheSvc.Instance.Offline(ses);
        if (offlienSuc)
        {
            PECommonTool.Log("客户端下线成功: "+sesID);
        }
        else {
            PECommonTool.Log("客户端下线失败: " + sesID);
        }
    }

}