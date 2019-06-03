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

    /// <summary>
    /// 判断玩家是否在线
    /// </summary> 
    public bool IsOnline(string acct) {
        return onlineStateDic.ContainsKey(acct);
    }

    public bool Offline(ServerSession ses) {
        bool offlienSuc = true;
        string key1 = "";
        foreach (var item in onlineStateDic)
        {
            if (item.Value == ses) {
                key1 = item.Key;
            }
        }
        if (onlineStateDic.ContainsKey(key1))
        {
            onlineStateDic.Remove(key1);
        }
        else {
            offlienSuc = false;
        }
        if (playerDataDic.ContainsKey(ses))
        {
            playerDataDic.Remove(ses);
        }
        else {
            offlienSuc = false;
        }
        return offlienSuc;
    }

    /// <summary>
    /// 根据账号密码获取玩家数据， 密码错误返回null， 
    /// 账号不存在则默认创建新账号
    /// </summary> 
    public PlayerData GetPlayerData(string acct, string password) {
        PlayerData data = DBMgr.Instance.QueryPlayerData(acct, password);
        return data;
    }

    /// <summary>
    /// 玩家上线后缓存数据
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="data"></param>
    /// <param name="ses"></param>
    public void CachePlayerData(string acct, PlayerData data, ServerSession ses) {
        if (!onlineStateDic.ContainsKey(acct)) {
            onlineStateDic.Add(acct, ses);
        }
        if (!playerDataDic.ContainsKey(ses)) {
            playerDataDic.Add(ses, data);
        }
    }

    public PlayerData GetPlayerDataBySession(ServerSession ses) {
        PlayerData data = null;
        if (playerDataDic.TryGetValue(ses, out data)) {
            return data;
        }
        return data;
    }

    public List<ServerSession> GetOnlineServerSes() {
        List<ServerSession> _list = new List<ServerSession>();
        foreach (var key in playerDataDic.Keys)
        {
            _list.Add(key);
        }
        return _list;
    }

    /// <summary>
    /// 检查数据库中是否存在名字
    /// </summary> 
    public bool CheckName(string name) {
        return DBMgr.Instance.CheckName(name);
    }
     
    public bool UpdatePlayerDataToDB(int id, PlayerData data) {

        return DBMgr.Instance.UpdatePlayerData(id, data);
    }



}