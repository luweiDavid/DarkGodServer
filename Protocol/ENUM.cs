
/****************************************************
	文件：ENUM.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/24 17:00   	
	功能：网络数据传输的所有枚举类型都定义在这个文件中
*****************************************************/

using System;

namespace Protocol
{

    //cmd 类型
    [Serializable]
    public enum MsgType
    {
        None = 100,

        ReqLogin = 101,
        RspLogin = 102,
    }

    [Serializable]
    public enum ErrorCode {
        None = -1,
        AlreadyOnline = 0,
        InvalidPassword,

    }

}
