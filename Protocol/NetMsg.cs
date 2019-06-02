

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
    public class NetMsg : PEMsg
    {
        public ReqLogin ReqLogin;
        public RspLogin RspLogin;
        public ReqRename ReqRename;
        public RspRename RspRename; 
        public ReqGuide ReqGuide;
        public RspGuide RspGuide;

        public ReqStrong ReqStrong;
        public RspStrong RspStrong;

    }
    #region strong
    [Serializable]
    public class ReqStrong {
        public int pos;
    }

    [Serializable]
    public class RspStrong {
        public PlayerData data;
    }

    #endregion

    #region  guidesys
    [Serializable]
    public class ReqGuide {
        public int guideId;
    }

    //既然客户端有guide配置表，然后可以读取到相应的引导奖励，那为什么还要服务器下发呢？
    //基于数据安全的考虑，不能让客户端随意的修改数据（防修改，防外挂），
    //在客户端跟服务器的数据传输中，只传输有必要的数据，比如说id等，这样可以节省服务器带宽
    //可以通过配置表去读取的就不用客户端传输过来了。所以一般是这样的，开发过程中，客户端跟服务器
    //读取的配置表是一样的，在游戏发布后，给服务器单独复制一份进行读取。
    [Serializable]
    public class RspGuide {
        public int guideId;
        public PlayerData data;
    }
    #endregion
     
    #region  loginsys
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

    #endregion


    public class ServerCfg {
        public const string IP = "127.0.0.1";
        public const int Port = 17666;
    }
}
