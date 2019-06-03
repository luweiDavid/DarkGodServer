

using Protocol;
using System.Collections.Generic;

public class ChatSys:SystemRoot<ChatSys>
{    
    public void HandleSendChatMsg(MsgPack pack) {
        SendChatMsg reqData = pack.msg.SendChatMsg; 

        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.PushChatMsg,
        };
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
        string _name = pd.Name;

        newMsg.PushChatMsg = new PushChatMsg
        {
            name = _name,
            str = reqData.str,
        };

        //这里广播消息时要做一个优化，先将数据转化为二进制数据
        byte[] btys = PENet.PETool.PackNetMsg(newMsg);
        List<ServerSession> _list = CacheSvc.Instance.GetOnlineServerSes();
        for (int i = 0; i < _list.Count; i++)
        {
            _list[i].SendMsg(btys);
        }
    }

}