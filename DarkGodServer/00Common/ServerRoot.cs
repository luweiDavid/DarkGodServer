﻿/****************************************************
	文件：ServerRoot.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:10   	
	功能：初始化各个系统
*****************************************************/

using System;

public class ServerRoot
{
    private static ServerRoot instance;
    public static ServerRoot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ServerRoot();
            }
            return instance;
        }
    }

    public void Init()
    {
        //数据层
        DBMgr.Instance.Init();

        //缓存层
        CacheSvc.Instance.Init();

        //服务层
        NetSvc.Instance.Init();
        CfgSvc.Instance.Init();
        TimerSvc.Instance.Init();
       
        //业务层
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        PowerSys.Instance.Init();
        AddTimeAndFrameTask();
    }

    public void Update() {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    //计时任务统一添加到这里
    public void AddTimeAndFrameTask() {
        PowerSys.Instance.AddPowerAddTimeTask();


    }

    private int SessionID = 0;
    public int GetSessionID() {
        SessionID += 1;
        return SessionID;
    }
}
