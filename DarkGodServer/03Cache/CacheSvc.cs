/****************************************************
	文件：CacheSvc.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/24 17:03   	
	功能：网络数据缓存层, 比如登陆状态，玩家数据，。。。
*****************************************************/


using Protocol;
using System.Collections.Generic;

public class CacheSvc
{
    private static CacheSvc instance;
    public static CacheSvc Instance {
        get {
            if (instance == null) {
                instance = new CacheSvc();
            }
            return instance;
        }
    }

    //key：玩家账号， 缓存所有已经上线的玩家
    private Dictionary<string, ServerSession> onlineStateDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> playerDataDic = new Dictionary<ServerSession, PlayerData>();
    public void Init() {
        PECommonTool.Log("CacheSvc Init Done");
    }


    public bool IsOnline(string acct) {
        return onlineStateDic.ContainsKey(acct);
    }


    /// <summary>
    /// 根据账号密码获取玩家数据， 密码错误返回null， 
    /// 账号不存在则默认创建新账号
    /// </summary> 
    public PlayerData GetPlayerData(string acct, string password) {
        return new PlayerData();
    }

    public void CachePlayerData(string acct, PlayerData data, ServerSession ses) {

    }




}