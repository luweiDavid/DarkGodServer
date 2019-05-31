
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
        ReqRename = 103,
        RspRename = 104,


        ReqGuide = 200,
        RspGuide = 201,
    }

    [Serializable]
    public enum ErrorCode {
        None = 0,
        AlreadyOnline = 1,
        InvalidPassword,
        NameExisted,
        UpdateDBFailed,

        ServerDataError,
    }

}
