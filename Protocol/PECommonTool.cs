/****************************************************
	文件：PECommonTool.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/24 15:11   	
	功能：客户端服务端通用工具
*****************************************************/

using PENet;

public enum LogType {
    Log = 0,
    Warn,
    Error,
    Info,
}
public class PECommonTool
{
    public static void Log(string msg, LogType _type = LogType.Log) {
        LogLevel lv = (LogLevel)_type;
        PETool.LogMsg(msg, lv);
    } 
}