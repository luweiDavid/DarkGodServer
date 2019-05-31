
/****************************************************
	文件：GuideSys.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/31 16:14   	
	功能：引导业务系统
*****************************************************/

using Protocol;

public class GuideSys:SystemRoot<GuideSys>
{

    public override void Init()
    {
        base.Init();


    }
    public void HandleReqGuide(MsgPack pack) {
        ReqGuide reqData = pack.msg.ReqGuide;

        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.RspGuide,
        };

        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
        if (pd == null || pd.GuideID != reqData.guideId) {
            //数据异常
            newMsg.err = (int)ErrorCode.ServerDataError;
        }
        else { 
            CfgGuideData cfg = CfgSvc.Instance.GetGuideData(reqData.guideId);
            if (cfg != null) {
                //更新数据（缓存和数据库）
                pd.GuideID += 1;
                PECommonTool.AddExp(ref pd, cfg.exp);

                //更新到数据库 todo
                if (!CacheSvc.Instance.UpdatePlayerDataToDB(pd.ID, pd))
                {
                    newMsg.err = (int)ErrorCode.UpdateDBFailed;
                }
                else {
                    newMsg.RspGuide = new RspGuide
                    {
                        guideId = pd.GuideID,
                        data = pd,
                    };
                }
            } 
        }
        pack.session.SendMsg(newMsg);
    }

    


}