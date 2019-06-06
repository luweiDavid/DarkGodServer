
/****************************************************
	文件：TimerSvc.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/06/06 10:51   	
	功能：计时器服务
*****************************************************/
using System;
using System.Collections.Generic;

public class TimerSvc : ServiceRoot<TimerSvc>
{
    private TimerTool timerTool = null;
    private readonly string lock_que = "lockcbpackque";
    private Queue<TaskCbPack> cbPackQue;

    public override void Init()
    {
        base.Init();

        cbPackQue = new Queue<TaskCbPack>(); 
        timerTool = new TimerTool(20);

        timerTool.SetLog((string str) =>
        {
            Console.WriteLine(str);
        });

        timerTool.SetHandleCb((Action<int> cb, int id) =>
        {
            if (cb != null)
            {
                lock (lock_que)
                { 
                    cbPackQue.Enqueue(new TaskCbPack(id, cb));
                }
            }
        }); 
    }

    public void Update() {
        while (cbPackQue.Count > 0)
        {
            lock (lock_que)
            {
                TaskCbPack pack = cbPackQue.Dequeue();
                pack.Cb(pack.taskId);
            }
        }
    } 

    public void AddTimeTask(Action<int> cb, float interval, int count, TimeUnit unit = TimeUnit.Millisecond) {
        timerTool.AddTimeTask(cb, interval, count, unit); 
    }
     
    public long GetNowTime() {
       return (long)timerTool.GetUTCMillisecondTime();
    }

    public void LogInfo(string str) {
        timerTool.LogInfo(str);
    }
}

public class TaskCbPack
{
    public int taskId;
    public Action<int> Cb;
    public TaskCbPack(int id, Action<int> cb)
    {
        this.taskId = id;
        this.Cb = cb;
    }
}