
using Protocol;

public class CommonBuySys:SystemRoot<CommonBuySys>
{


    public void HandleReqBuy(MsgPack pack) {
        ReqBuy reqData = pack.msg.ReqBuy;

        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.RspBuy,
        };

        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
        if (pd.Diamond < 10)
        {
            newMsg.err = (int)ErrorCode.LackDiamond;
        }
        else {
            pd.Diamond -= 10;
            switch (reqData.buyType)
            {
                case (int)CommonBuyType.Coin:
                    pd.Coin += 100;
                    break;
                case (int)CommonBuyType.Power:
                    pd.Power += 50;
                    break;
            }
            if (CacheSvc.Instance.UpdatePlayerDataToDB(pd.ID, pd))
            {
                newMsg.RspBuy = new RspBuy
                {
                    buyType = reqData.buyType,
                    data = pd,
                };
            }
            else {
                newMsg.err = (int)ErrorCode.UpdateDBFailed;
            }
        }

        pack.session.SendMsg(newMsg);
    }

}
