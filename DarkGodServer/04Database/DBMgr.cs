﻿
/****************************************************
	文件：DBMgr.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/25 13:10   	
	功能：数据库连接管理器，对数据库进行增删改查
*****************************************************/
using System;
using System.Collections.Generic;
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
        string conStr = "server=localhost;User Id=root;password=;Database=darkgod";
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

    #region  测试代码
    private void Test() {
        PlayerData pd = QueryPlayerData("1", "21");
        if (pd!= null)
        {
            //if (pd.RewardStateArr == null)
            //{
            //    pd.RewardStateArr = new TaskRewardState[6];
            //}
            //else
            //{
            //    TaskRewardState rs = new TaskRewardState
            //    {
            //        progress = 1,
            //        state = 33,
            //    };
            //    PECommonTool.Log("++update++");
            //    pd.RewardStateArr[0] = rs; 
            //}
            //UpdatePlayerData(pd.ID, pd);


            //for (int i = 0; i < pd.RewardStateArr.Length; i++)
            //{
            //    PECommonTool.Log(pd.RewardStateArr[i].progress + "+++++++++" + pd.RewardStateArr[i].state);
            //}
        }

        #endregion
    }

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
                    int hp = reader.GetInt32("hp");
                    int ad = reader.GetInt32("ad");
                    int ap = reader.GetInt32("ap");
                    int addef = reader.GetInt32("addef");
                    int apdef = reader.GetInt32("apdef");
                    int dodge = reader.GetInt32("dodge");
                    int pierce = reader.GetInt32("pierce");
                    int critical = reader.GetInt32("critical");
                    int guideid = reader.GetInt32("guideid");  
                    string strongStr = reader.GetString("strong");
                    int crystal = reader.GetInt32("crystal"); 
                    long time = reader.GetInt64("time");
                    int fbid = reader.GetInt32("fubenid");


                    #region  读取拼接的强化数据
                    string[] tmp = strongStr.Split('#');
                    int[] strong = new int[6];
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tmp[i])) {
                            continue;
                        }
                        strong[i] = int.Parse(tmp[i]);
                    }
                    #endregion
                    
                    
                    playerData = new PlayerData(id, name, level, exp, power, coin, diamond,
                        hp, ad, ap, addef, apdef, dodge, pierce, critical, guideid, strong, crystal,
                        time, fbid);



                    #region  测试代码
                    //byte[] btArr = new byte[124];
                    //reader.GetBytes(reader.GetOrdinal("taskrewardstate"), 0, btArr, 0, 124);
                    //TaskRewardState[] arr = SerializeMgr.DeserializeBinary<TaskRewardState>(btArr);
                    //playerData.RewardStateArr = arr;

                    #endregion
                }
                else
                {
                    //密码错误
                }
            }
        }
        catch (Exception e)
        {
            PECommonTool.Log("QueryPlayerData: "+e.Message, LogType.Error);
        }
        finally {
            if (reader != null) {
                reader.Close();
            }
            if (isNewAcct) {
                //创建新账号
                long time = TimerSvc.Instance.GetNowTime();
                playerData = new PlayerData(time);

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
            "@name,level = @level,exp = @exp,power = @power,coin = @coin,diamond = @diamond," +
            "hp = @hp,ad = @ad,ap = @ap,addef = @addef,apdef = @apdef,dodge = @dodge," +
            "pierce = @pierce,critical = @critical, guideid = @guideid, strong = @strong," +
            "crystal = @crystal, time = @time, fubenid = @fubenid";
        int id = -1;
        try
        {
            MySqlCommand cmd = new MySqlCommand(quest, conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("name", data.Name);
            cmd.Parameters.AddWithValue("level", data.Level);
            cmd.Parameters.AddWithValue("exp", data.Experience);
            cmd.Parameters.AddWithValue("power", data.Power);
            cmd.Parameters.AddWithValue("coin", data.Coin);
            cmd.Parameters.AddWithValue("diamond", data.Diamond);
            cmd.Parameters.AddWithValue("hp", data.Hp);
            cmd.Parameters.AddWithValue("ad", data.Ad);
            cmd.Parameters.AddWithValue("ap", data.Ap);
            cmd.Parameters.AddWithValue("addef", data.Addef);
            cmd.Parameters.AddWithValue("apdef", data.Apdef);
            cmd.Parameters.AddWithValue("dodge", data.Dodge);
            cmd.Parameters.AddWithValue("pierce", data.Pierce);
            cmd.Parameters.AddWithValue("critical", data.Critical);
            cmd.Parameters.AddWithValue("guideid", data.GuideID);  
            string str = PECommonTool.GetJointString(data.Strong, '#');
            cmd.Parameters.AddWithValue("strong", str); 
            cmd.Parameters.AddWithValue("crystal", data.Crystal);
            cmd.Parameters.AddWithValue("time", data.Time);
            cmd.Parameters.AddWithValue("fubenid", data.FuBenId);

            cmd.ExecuteNonQuery();
             id = (int)cmd.LastInsertedId;
            PECommonTool.Log(id.ToString(), LogType.Info);
        }
        catch (Exception e)
        {
            PECommonTool.Log("InsertNewAccount: " + e.Message, LogType.Error);
        } 
        return id;
    } 

    /// <summary>
    /// 修改数据
    /// </summary>
    public bool UpdatePlayerData(int id, PlayerData data) {
        bool updateSuc = true; 

        string quest = "update account set name = @name,level = @level,exp = @exp,power = @power,coin = @coin,diamond = @diamond," +
            "hp = @hp,ad = @ad,ap = @ap,addef = @addef,apdef = @apdef,dodge = @dodge," +
            "pierce = @pierce,critical = @critical, guideid = @guideid,strong = @strong," +
            "crystal = @crystal, time = @time, fubenid = @fubenid where id=@id ";

        try
        {
            MySqlCommand cmd = new MySqlCommand(quest, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", data.Name);
            cmd.Parameters.AddWithValue("level", data.Level);
            cmd.Parameters.AddWithValue("exp", data.Experience);
            cmd.Parameters.AddWithValue("power", data.Power);
            cmd.Parameters.AddWithValue("coin", data.Coin);
            cmd.Parameters.AddWithValue("diamond", data.Diamond);
            cmd.Parameters.AddWithValue("hp", data.Hp);
            cmd.Parameters.AddWithValue("ad", data.Ad);
            cmd.Parameters.AddWithValue("ap", data.Ap);
            cmd.Parameters.AddWithValue("addef", data.Addef);
            cmd.Parameters.AddWithValue("apdef", data.Apdef);
            cmd.Parameters.AddWithValue("dodge", data.Dodge);
            cmd.Parameters.AddWithValue("pierce", data.Pierce);
            cmd.Parameters.AddWithValue("critical", data.Critical);
            cmd.Parameters.AddWithValue("guideid", data.GuideID);
            string str = PECommonTool.GetJointString(data.Strong, '#');
            cmd.Parameters.AddWithValue("strong", str);
            cmd.Parameters.AddWithValue("crystal", data.Crystal);
            cmd.Parameters.AddWithValue("time", data.Time); 
            cmd.Parameters.AddWithValue("fubenid", data.FuBenId); 

            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            updateSuc = false;
            PECommonTool.Log("UpdatePlayerData: " + e.Message, LogType.Error);
        }
        return updateSuc;
    }



    /// <summary>
    /// 由于编码问题，不能判断中文名称+
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckName(string name) {
        bool isExist = false;
        MySqlDataReader reader = null;
        string quest = "select * from account";
        try
        {
            MySqlCommand cmd = new MySqlCommand(quest, conn); 
            reader = cmd.ExecuteReader();   
            while (reader.Read())
            {
                string tmp = reader.GetString("name");  
                if (string.Equals(tmp, name)) {
                    isExist = true;
                } 
            }
        }
        catch (Exception e)
        {
            PECommonTool.Log("CheckName: " + e.Message, LogType.Error);
        }
        finally {
            if (reader != null) {
                reader.Close();
            }
        }  

        return isExist;
    }
}
