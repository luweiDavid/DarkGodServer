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

public class NetSvc
{
    private static NetSvc instance;
    public static NetSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetSvc();
            }
            return instance;
        }
    }
    PESocket<ServerSession, NetMsg> socket = null;


    private Queue<MsgPack> packQueue = new Queue<MsgPack>();


    public void Init() {
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
                LoginSys.Instance.HandleMsgPack(pack);
                break;
            case MsgType.RspLogin:
                break;
            default:
                break;
        }
    } 
}