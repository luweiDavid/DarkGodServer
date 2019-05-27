/****************************************************
	文件：PECommonTool.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/24 15:11   	
	功能：客户端服务端通用工具
*****************************************************/

using PENet;
using Protocol;

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

    /// <summary>
    /// 返回角色的战斗力
    /// </summary> 
    public static int GetFight(PlayerData data) {
        return data.Level * 100 + data.Ad + data.Ap + data.Addef + data.Apdef;
    }

    /// <summary>
    /// 返回角色的体力
    /// </summary> 
    public static int GetPowerLimit(int lv) {
        return ((lv - 1) / 10) * 150 + 150;
    }

    public static int GetExpUpvalue(int lv) {
        return 100 * lv * lv;
    }
}