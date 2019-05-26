
/****************************************************
	文件：DBMgr.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/25 13:10   	
	功能：数据库连接管理器，对数据库进行增删改查
*****************************************************/
using System;
using MySql.Data.MySqlClient;
using Protocol;

public class DBMgr
{
    public static MySqlConnection conn;

    private static DBMgr instance;
    public static DBMgr Instance {
        get {
            if (instance == null) {
                instance = new DBMgr();
            }
            return instance;
        }
    }
    
    
    public void Init() {
        string conStr = "server=localhost;User Id=root;password=;Database=darkgod; Charset=utf8;";
        conn = new MySqlConnection(conStr);
        try
        {
            conn.Open();
            PECommonTool.Log("数据库连接成功");
        }
        catch (Exception e)
        {
            PECommonTool.Log("数据库连接失败：" + e.Message, LogType.Error);
            return;
        } 
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public PlayerData QueryPlayerData(string acct,string password) {
        bool isNewAcct = true;
        PlayerData playerData = null;
        MySqlDataReader reader = null;
        string quest = "select * from account where acct=@acct";
        try
        {
            MySqlCommand cmd = new MySqlCommand(quest, conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                isNewAcct = false;
                string tmpPassword = reader.GetString("password");
                if (string.Equals(password, tmpPassword))
                {
                    int id = reader.GetInt32("id");
                    string name = reader.GetString("name");
                    int level = reader.GetInt32("level");
                    int exp = reader.GetInt32("exp");
                    int power = reader.GetInt32("power");
                    int coin = reader.GetInt32("coin");
                    int diamond = reader.GetInt32("diamond");

                    playerData = new PlayerData(id, name, level, exp, power, coin, diamond);
                }
                else
                {
                    //密码错误
                }
            }
        }
        catch (Exception e)
        {
            PECommonTool.Log(e.Message, LogType.Error);
        }
        finally {
            if (reader != null) {
                reader.Close();
            }
            if (isNewAcct) {
                //创建新账号
                playerData = new PlayerData();

                //插入新账号时，一定要把返回的主键id赋值给playerdata，不然会导致所有的新账号的id都是默认值-1
                playerData.ID = InsertNewAccount(acct, password, playerData);
            }
        }
         
        return playerData;

    }
    /// <summary>
    /// 插入一个新账号
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="password"></param>
    /// <param name="data"></param>
    /// <returns>返回主键id</returns>
    public int InsertNewAccount(string acct, string password, PlayerData data) {
        string quest = "insert into account set acct = @acct,password = @password,name = " +
            "@name,level = @level,exp = @exp,power = @power,coin = @coin,diamond = @diamond";
        MySqlCommand cmd = new MySqlCommand(quest, conn);
        cmd.Parameters.AddWithValue("acct", acct);
        cmd.Parameters.AddWithValue("password", password);
        cmd.Parameters.AddWithValue("name", data.Name);
        cmd.Parameters.AddWithValue("level", data.Level);
        cmd.Parameters.AddWithValue("exp", data.Experience);
        cmd.Parameters.AddWithValue("power", data.Power);
        cmd.Parameters.AddWithValue("coin", data.Coin);
        cmd.Parameters.AddWithValue("diamond", data.Diamond);
        cmd.ExecuteNonQuery();
        int id = (int)cmd.LastInsertedId;
        PECommonTool.Log(id.ToString(), LogType.Info);
        return id;
    }


   

    /// <summary>
    /// 修改数据
    /// </summary>
    public void UpdatePlayerData(int id, PlayerData data) {
        string quest = "select * from account where id=@id";
    }


    public bool CheckName(string name) {
        bool isExist = false;

        return isExist;
    }
}
