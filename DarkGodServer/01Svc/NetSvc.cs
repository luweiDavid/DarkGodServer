/****************************************************
	文件：NetSvc.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:16   	
	功能：网络服务层

需要改进的地方：
（1）处理消息的功能应该抽象出一个公有基类，根据cmd类型反射出对应的处理脚本
*****************************************************/

using PENet;
using Protocol;
using System.Collections.Generic;

public class NetSvc:ServiceRoot<NetSvc>
{ 
    PESocket<ServerSession, NetMsg> socket = null; 
    private Queue<MsgPack> packQueue = new Queue<MsgPack>();


    public override void Init() {
        socket = new PESocket<ServerSession, NetMsg>();
        socket.StartAsServer(ServerCfg.IP,ServerCfg.Port);

        PECommonTool.Log("NetSvc Init Done");
    }


    public void AddMsgPack(MsgPack pack) {
        packQueue.Enqueue(pack);
    }

    public void Update() {
        if (packQueue != null && packQueue.Count > 0) {
            //这里访问修改数据时，应该要加锁，这里涉及多线程问题 TODO
            MsgPack pack = packQueue.Dequeue();
            lock (pack) {
                HandleMsgPack(pack);
            }
        }
    } 
   
    public void HandleMsgPack(MsgPack pack) {
        MsgType msgType = (MsgType)pack.msg.cmd;
        switch (msgType)
        {
            case MsgType.None:
                break;
            case MsgType.ReqLogin:
                LoginSys.Instance.HandleReqLogin(pack);
                break;
            case MsgType.ReqRename:
                LoginSys.Instance.HandleReqRename(pack);
                break;
            case MsgType.ReqGuide:
                GuideSys.Instance.HandleReqGuide(pack);
                break; 
            case MsgType.ReqStrong:
                StrongSys.Instance.HandleReqStrong(pack);
                break;
            case MsgType.SendChatMsg:
                ChatSys.Instance.HandleSendChatMsg(pack);
                break;
            case MsgType.ReqBuy:
                CommonBuySys.Instance.HandleReqBuy(pack);
                break; 
            case MsgType.ReqFuBen:
                FuBenSys.Instance.HandleReqFuBen(pack);
                break;
        }
    } 
}