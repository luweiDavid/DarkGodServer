
/****************************************************
	文件：GuideSys.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/31 16:14   	
	功能：引导业务系统
*****************************************************/

using Protocol;

public class StrongSys : SystemRoot<StrongSys>
{

    public override void Init()
    {
        base.Init();


    }
    public void HandleReqStrong(MsgPack pack) {
        ReqStrong reqData = pack.msg.ReqStrong; 
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.RspStrong,
        }; 
         
        int pos = reqData.pos;
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session); 
        int curStarLv = pd.Strong[pos];

        CfgStrongData nextCfg = CfgSvc.Instance.GetStrongData(pos, curStarLv + 1);
        if (nextCfg != null)
        { 
            if (pd.Coin < nextCfg.coin)
            {
                newMsg.err = (int)ErrorCode.LackCoin;
            }
            else if (pd.Level < nextCfg.minLv)
            {
                newMsg.err = (int)ErrorCode.LackLevel;
            }
            else if (pd.Crystal < nextCfg.crystal)
            {
                newMsg.err = (int)ErrorCode.LackCrystal;
            }
            else { 
                //修改缓存，更新数据库
                pd.Strong[pos] += 1;
                pd.Hp += nextCfg.addHp;
                pd.Ad += nextCfg.addHurt;
                pd.Ap += nextCfg.addHurt;
                pd.Addef += nextCfg.addDef;
                pd.Apdef += nextCfg.addDef;
                pd.Coin -= nextCfg.coin;
                pd.Crystal -= nextCfg.crystal;

                if (CacheSvc.Instance.UpdatePlayerDataToDB(pd.ID, pd))
                {
                    newMsg.RspStrong = new RspStrong
                    {
                        data = pd,
                    };
                }
                else {
                    newMsg.err = (int)ErrorCode.UpdateDBFailed;
                }
            }

        }
        else {  
            //这里防止客户端在已经升满级的情况下发起强化请求
            PECommonTool.Log("获取强化配置错误", LogType.Error);
            return;
        } 
         
        pack.session.SendMsg(newMsg);
    }

    


}