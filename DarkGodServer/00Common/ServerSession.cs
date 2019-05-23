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
    protected override void OnConnected()
    {
        base.OnConnected();
    }

    protected override void OnDisConnected()
    {
        base.OnDisConnected();
    }

    protected override void OnReciveMsg(NetMsg msg)
    {
        base.OnReciveMsg(msg);
    }
}