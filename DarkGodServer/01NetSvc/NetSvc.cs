/****************************************************
	文件：NetSvc.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:16   	
	功能：网络服务层
*****************************************************/

using PENet;
using Protocol;

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

    public void Init() {
        PESocket<ServerSession, NetMsg> socket = new PESocket<ServerSession, NetMsg>();
        socket.StartAsServer(ServerCfg.IP,ServerCfg.Port);

        PETool.LogMsg("NetSvc Init Done");
    }


}