

/****************************************************
	文件：NetMsg.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:34   	
	功能：网络信息
*****************************************************/
using PENet;

namespace Protocol
{
    public class NetMsg:PEMsg
    {
        public string content;
    }

    public class ServerCfg {
        public const string IP = "127.0.0.1";
        public const int Port = 17666;
    }
}
