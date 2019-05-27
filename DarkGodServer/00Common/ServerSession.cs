/****************************************************
	文件：ServerSession.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:30   	
	功能：网络会话连接
*****************************************************/
using PENet;
using Protocol;

public class ServerSession : PESession<NetMsg>
{

    private int SessionID = 0;

    protected override void OnConnected()
    {
        PECommonTool.Log("New Client Connected");
        SessionID = ServerRoot.Instance.GetSessionID();
    }
   
    protected override void OnReciveMsg(NetMsg msg)
    {
        //受到消息时，打包给网络服务层处理
        string str = ((MsgType)msg.cmd).ToString();
        PECommonTool.Log(str);
        NetSvc.Instance.AddMsgPack(new MsgPack(this, msg));
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.Offline(this, SessionID);
    }  
}

public class MsgPack {
    public ServerSession session;
    public NetMsg msg;
    public MsgPack(ServerSession s, NetMsg m) {
        session = s;
        msg = m;
    }
}