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

    /// <summary>
    /// 增加经验（同时判断是否可以升级）
    /// </summary>
    /// <param name="data"></param>
    /// <param name="exp"></param>
    public static void AddExp(ref PlayerData data, int exp) {
        int curExp = data.Experience;
        int curLv = data.Level;
        int totalAddExp = exp;

        while (true)
        {
            int upLvNeedExp = GetExpUpvalue(curLv) - curExp;
            if (totalAddExp >= upLvNeedExp)
            {
                curLv += 1;
                curExp = 0;
                totalAddExp -= upLvNeedExp;
            }
            else {
                data.Level = curLv;
                data.Experience += totalAddExp;
                break;
            }
        }
    }
    
    public static string GetJointString(int[] strArr, char _char)
    {
        string str = "";
        for (int i = 0; i < strArr.Length; i++)
        { 
            str += strArr[i];
            if (i < strArr.Length - 1)
            {
                str += _char;
            }
        }
        return str;
    }


    public const int AddPowerTimeSpan = 5;  //恢复体力时间间隔（分钟）
    public const int AddPowerPerTimes = 3;  //每次恢复的体力值



}