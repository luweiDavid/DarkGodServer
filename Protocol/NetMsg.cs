

/****************************************************
	文件：NetMsg.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:34   	
	功能：网络信息

需要改进的地方：
（1）NetMsg
*****************************************************/
using PENet;
using System;

namespace Protocol
{
   
    [Serializable] 
    public class NetMsg:PEMsg
    {
        public ReqLogin ReqLogin;
        public RspLogin RspLogin;
        public ReqRename ReqRename;
        public RspRename RspRename;

    }

    [Serializable]
    public class ReqLogin {
        public string account;
        public string password;
    }

    [Serializable]
    public class RspLogin {
        public PlayerData data;
    }

    [Serializable]
    public class ReqRename {
        public string name;
    }

    [Serializable]
    public class RspRename {
        public string name;
    }


    public class ServerCfg {
        public const string IP = "127.0.0.1";
        public const int Port = 17666;
    }
}
