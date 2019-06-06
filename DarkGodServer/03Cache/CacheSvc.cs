/****************************************************
	文件：CacheSvc.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/24 17:03   	
	功能：网络数据缓存层, 比如登陆状态，玩家数据，。。。
*****************************************************/


using Protocol;
using System.Collections.Generic;

public class CacheSvc:ServiceRoot<CacheSvc>
{  
    //key：玩家账号， 缓存所有已经上线的玩家
    private Dictionary<string, ServerSession> onlinePlayerSessionDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onlinePlayerDataDic = new Dictionary<ServerSession, PlayerData>();

    private List<ServerSession> onlineSessionList = new List<ServerSession>();
    
    public bool Offline(ServerSession ses) {
        bool offlienSuc = true;
        string key1 = "";
        foreach (var item in onlinePlayerSessionDic)
        {
            if (item.Value == ses) {
                key1 = item.Key;
            }
        }
        if (onlinePlayerSessionDic.ContainsKey(key1))
        {
            onlinePlayerSessionDic.Remove(key1);
        }
        else {
            offlienSuc = false;
        }

        if (onlinePlayerDataDic.ContainsKey(ses))
        {
            onlinePlayerDataDic.Remove(ses);
        }
        else {
            offlienSuc = false;
        }

        if (onlineSessionList.Contains(ses)) {
            onlineSessionList.Remove(ses);
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

    public void CachePlayerData(string acct, PlayerData data, ServerSession ses) {
        if (!onlinePlayerSessionDic.ContainsKey(acct)) {
            onlinePlayerSessionDic.Add(acct, ses);
        }
        if (!onlinePlayerDataDic.ContainsKey(ses)) {
            onlinePlayerDataDic.Add(ses, data);
        }
        if (!onlineSessionList.Contains(ses)) {
            onlineSessionList.Add(ses);
        } 
    } 
     
    public bool CheckName(string name) {
        return DBMgr.Instance.CheckName(name);
    }
     
    public bool UpdatePlayerDataToDB(int id, PlayerData data) {

        return DBMgr.Instance.UpdatePlayerData(id, data);
    }

    public bool IsOnline(string acct)
    {
        return onlinePlayerSessionDic.ContainsKey(acct);
    } 

    public PlayerData GetPlayerDataBySession(ServerSession ses)
    {
        PlayerData data = null;
        if (onlinePlayerDataDic.TryGetValue(ses, out data))
        {
            return data;
        }
        return data;
    }

    public List<ServerSession> GetOnlineServerSes()
    {
        return onlineSessionList;
    } 
}