

using Protocol;

public class FuBenSys:SystemRoot<FuBenSys>
{ 

    public void HandleReqFuBen(MsgPack pack) {
        ReqFuBen reqData = pack.msg.ReqFuBen;

        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.RspFuBen
        };
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
        CfgMap cfg = CfgSvc.Instance.GetCfgMap(reqData.fubenID);

        if (pd.FuBenId < reqData.fubenID) {
            newMsg.err = (int)ErrorCode.ClientDataError;
        }
        if (cfg != null)
        {
            if (pd.Power < cfg.power)
            {
                newMsg.err = (int)ErrorCode.LackPower;
            }
            else {
                pd.Power -= cfg.power;
                if (CacheSvc.Instance.UpdatePlayerDataToDB(pd.ID, pd))
                {
                    newMsg.RspFuBen = new RspFuBen
                    {
                        fubenID = reqData.fubenID,
                        pd = pd,
                    };
                }
                else {
                    newMsg.err = (int)ErrorCode.UpdateDBFailed;
                }
            }
        }
        else {
            newMsg.err = (int)ErrorCode.GetCfgError;
        }

        pack.session.SendMsg(newMsg);
    }
}