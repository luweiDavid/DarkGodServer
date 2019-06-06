

using System;
using System.Collections.Generic;
using System.Timers;

public class TimerTool : ServiceRoot<TimerTool>
{
    private Action<string> logCb;
    private DateTime startDataTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    private Timer timer;

    private readonly string lock_idList = "lockidlist";
    private readonly string lock_timeTaskList = "locktimetasklist";
    private readonly string lock_frameTaskList = "lockframetasklist"; 
    public int uniqueId;
    public List<int> idList;
    public List<int> delIdList;

    public List<TimeTask> tmpTimeTaskList;
    public List<TimeTask> timeTaskList;
    public List<int> delTimeTaskIdList;

    public List<FrameTask> tmpFrameTaskList;
    public List<FrameTask> frameTaskList;
    public List<int> delFrameTaskIdList;
    private int frameCounter;

    //如果需要把任务的回调运行在特定的线程中，比如主线程，就需要注册这个回调
    private Action<Action<int>, int> taskHandleCb;

    public TimerTool() { }

    public TimerTool(float interval = 0) {
        idList = new List<int>();
        delIdList = new List<int>();
        tmpTimeTaskList = new List<TimeTask>();
        timeTaskList = new List<TimeTask>();
        delTimeTaskIdList = new List<int>();

        tmpFrameTaskList = new List<FrameTask>();
        frameTaskList = new List<FrameTask>();
        delFrameTaskIdList = new List<int>();
         
        if (interval != 0)
        {
            //用c#底层的线程池去运行
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Elapsed += ((object sender, ElapsedEventArgs args) =>
            { 
                Tick();
            });

            timer.Start();
        }
    }

    public void Tick() {
        frameCounter++;
        TickTimeTask();
        TickFrameTask();
         
        DeleteUniqueId(); 
    }

    #region  timetask
    public void TickTimeTask() {
        if (tmpTimeTaskList.Count > 0) {
            lock (lock_timeTaskList) {
                for (int i = 0; i < tmpTimeTaskList.Count; i++)
                {
                    timeTaskList.Add(tmpTimeTaskList[i]);
                }
                tmpTimeTaskList.Clear();
            }
        }
        if (timeTaskList.Count > 0) {
            for (int i = 0; i < timeTaskList.Count; i++)
            {
                try
                {
                    TimeTask task = timeTaskList[i];
                    if (task.count == 0 || task.count < -1)
                    {
                        lock (lock_timeTaskList)
                        {
                            timeTaskList.RemoveAt(i);
                            i -= 1;
                        }
                        delIdList.Add(task.Id);
                        continue;
                    }
                    if (GetUTCMillisecondTime() >= task.destTime)
                    {
                       
                        if (task.count == -1)
                        {
                            task.destTime = GetUTCMillisecondTime() + task.intervalTime;
                        }
                        else
                        {
                            task.count -= 1;
                            if (task.count == 0)
                            {
                                lock (lock_timeTaskList)
                                {
                                    timeTaskList.RemoveAt(i);
                                    i -= 1;
                                }
                                delIdList.Add(task.Id);
                            }
                            else {
                                task.destTime = GetUTCMillisecondTime() + task.intervalTime;
                            }  
                        }
                        if (taskHandleCb != null)
                        {
                            taskHandleCb(task.Cb, task.Id);
                        }
                        else
                        {
                            if (task.Cb != null)
                            {
                                task.Cb(task.Id);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    logCb(e.Message);
                } 
            }
        }

        DeleteTimeTask();
    }
      
    public void AddTimeTask(Action<int> cb, double interval, int count = 1, TimeUnit unit = TimeUnit.Millisecond) {
        if (unit != TimeUnit.Millisecond) {
            interval = GetMillisecond(interval, unit);
        }
        int id = GetUniqueId();
        TimeTask task = new TimeTask(id, cb, GetUTCMillisecondTime() + interval, interval, count);
        tmpTimeTaskList.Add(task);
    }

    private void DeleteTimeTask() {
        if (delTimeTaskIdList.Count > 0) {
            lock (lock_timeTaskList) {
                bool canFind = false; 
                for (int i = 0; i < delTimeTaskIdList.Count; i++)
                {
                    for (int j = 0; j < timeTaskList.Count; j++)
                    {
                        if (delTimeTaskIdList[i] == timeTaskList[i].Id) {
                            timeTaskList.RemoveAt(j);
                            canFind = true;
                            break;
                        }
                    }
                    if (canFind) {
                        continue;
                    }
                    for (int k = 0; k < tmpTimeTaskList.Count; k++)
                    {
                        if (delTimeTaskIdList[i] == tmpTimeTaskList[k].Id) {
                            tmpTimeTaskList.RemoveAt(k);
                            break;
                        }
                    }
                }
                delTimeTaskIdList.Clear();
            } 
        }
    }

    public bool DeleteTimeTask(int id) {
        bool canfindTask = false;
        for (int i = 0; i < timeTaskList.Count; i++)
        {
            if (id == timeTaskList[i].Id) {
                canfindTask = true;
                break;
            }
        }

        if (!canfindTask) {
            for (int i = 0; i < tmpTimeTaskList.Count; i++)
            {
                if (id == tmpTimeTaskList[i].Id) {
                    canfindTask = true;
                    break;
                }
            }
        }
        if (canfindTask) {
            lock (lock_timeTaskList) { 
                delTimeTaskIdList.Add(id);
            }
        }

        return canfindTask;
    }

    public bool ReplaceTimeTask(int id,Action<int> cb, float interval,int count=1,TimeUnit unit=TimeUnit.Millisecond) {
        bool ret = false;
        TimeTask task = null;
        for (int i = 0; i < timeTaskList.Count; i++)
        {
            if (id == timeTaskList[i].Id) {
                task = timeTaskList[i];
                ret = true;
                break;
            }
        }
        if (!ret) {
            for (int i = 0; i < tmpTimeTaskList.Count; i++)
            {
                if (id == tmpTimeTaskList[i].Id) {
                    task = tmpTimeTaskList[i];
                    ret = true;
                    break;
                }
            }
        }
        if (ret && task != null) {
            lock (lock_timeTaskList) {
                task.Cb = cb;
                task.destTime = GetUTCMillisecondTime() + interval;
                task.intervalTime = interval;
                task.count = count;
            }
        }

        return ret;
    }
    #endregion

    #region frametask
    private void TickFrameTask() {
        if (tmpFrameTaskList.Count > 0) {
            lock (lock_frameTaskList) {
                for (int i = 0; i < tmpFrameTaskList.Count; i++)
                {
                    frameTaskList.Add(tmpFrameTaskList[i]);
                }
                tmpFrameTaskList.Clear();
            }
        }
        if (frameTaskList.Count > 0) {
            for (int i = 0; i < frameTaskList.Count; i++)
            {
                FrameTask task = frameTaskList[i];
                if (task.count == 0 || task.count < -1) {
                    frameTaskList.RemoveAt(i);
                    i -= 1;
                    delIdList.Add(task.Id);
                    continue;
                }
                if (frameCounter >= task.destFrame)
                {
                    if (task.count == -1)
                    {
                        task.destFrame = frameCounter + task.intervalFrame;
                    }
                    else {
                        task.count -= 1;
                        if (task.count == 0)
                        {
                            frameTaskList.RemoveAt(i);
                            i -= 1;
                            delIdList.Add(task.Id);
                        }
                        else {
                            task.destFrame = frameCounter + task.intervalFrame;
                        }
                        if (taskHandleCb != null)
                        {
                            taskHandleCb(task.Cb, task.Id);
                        }
                        else {
                            if (task.Cb != null) {
                                task.Cb(task.Id);
                            }
                        }
                    }
                }
                else {
                    continue;
                }
            }
        } 
        DeleteFrameTask();
    }

    private void AddFrameTask(Action<int> cb,int interval,int count)
    {
        int id = GetUniqueId();
        FrameTask task = new FrameTask(id, cb, frameCounter + interval, interval, count); 
        tmpFrameTaskList.Add(task);
    }

    private void DeleteFrameTask() {
        if (delFrameTaskIdList.Count > 0) {
            lock (lock_frameTaskList) {
                for (int i = 0; i < delFrameTaskIdList.Count; i++)
                {
                    bool canFind = false;
                    for (int j = 0; j < frameTaskList.Count; j++)
                    {
                        if (delFrameTaskIdList[i] == frameTaskList[j].Id)
                        {
                            canFind = true;
                            frameTaskList.RemoveAt(j);
                            break;
                        }
                    }
                   
                    if (canFind) {
                        continue;
                    }
                    for (int k = 0; k < tmpFrameTaskList.Count; k++)
                    {
                        if (delFrameTaskIdList[i] == tmpFrameTaskList[k].Id)
                        {
                            tmpFrameTaskList.RemoveAt(k);
                            break;
                        }
                    }
                }
                delFrameTaskIdList.Clear();
            }
        }
    }

    public bool DeleteFrameTask(int id) {
        bool canFind = false;
        for (int i = 0; i < frameTaskList.Count; i++)
        {
            if (id == frameTaskList[i].Id) {
                canFind = true;
                break;
            }
        }
        if (!canFind) {
            for (int i = 0; i < tmpFrameTaskList.Count; i++)
            {
                if (id == tmpFrameTaskList[i].Id) {
                    canFind = true;
                    break;
                }
            }
        }
         
        if (canFind) {
            delFrameTaskIdList.Add(id);
        }
        return canFind;
    }

    public bool ReplaceFrameTask(int id, Action<int> cb,int interval,int count = 1) {
        bool ret = false;
        FrameTask task = null;
        for (int i = 0; i < frameTaskList.Count; i++)
        {
            if (id == frameTaskList[i].Id)
            {
                ret = true;
                task = frameTaskList[i];
                break;
            }
        }
        if (!ret)
        {
            for (int i = 0; i < tmpFrameTaskList.Count; i++)
            {
                if (id == tmpFrameTaskList[i].Id)
                {
                    task = tmpFrameTaskList[i];
                    ret = true;
                    break;
                }
            }
        }
        if (ret && task != null) {
            lock (lock_frameTaskList) {
                task.Cb = cb;
                task.destFrame = frameCounter + interval;
                task.intervalFrame = interval;
                task.count = count;
            }
        }

        return ret;
    }

    #endregion


    public void Reset() {
        idList.Clear();
        delIdList.Clear();
        tmpTimeTaskList.Clear();
        timeTaskList.Clear();
        delTimeTaskIdList.Clear();

        tmpFrameTaskList.Clear();
        frameTaskList.Clear();
        delFrameTaskIdList.Clear();

        if (timer != null) {
            timer.Stop();
        }
    }
     
    public double GetUTCMillisecondTime()
    { 
        TimeSpan span = DateTime.UtcNow - startDataTime; 
        return span.TotalMilliseconds;
    }

    public DateTime GetLocalDateTime() {
        DateTime time = TimeZone.CurrentTimeZone.ToLocalTime(startDataTime.AddMilliseconds(GetUTCMillisecondTime()));

        return time;
    }
    public string GetLocalTimeStr() {
        DateTime time = GetLocalDateTime();
        return GetStr(time.Hour) + ":" + GetStr(time.Minute) + ":" + GetStr(time.Second);
    }
    private string GetStr(int time) {
        if (time < 10)
        {
            return "0" + time;
        }
        else {
            return time.ToString();
        }
    }
     

    private void DeleteUniqueId() {
        if (delIdList.Count > 0) {
            lock (lock_idList) {
                for (int i = 0; i < delIdList.Count; i++)
                {
                    for (int j = 0; j < idList.Count; j++)
                    {
                        if (delIdList[i] == idList[j]) {
                            idList.RemoveAt(j);
                            break;
                        }
                    }
                }
                delIdList.Clear();
            }
        }
    }

    private int GetUniqueId() {
        uniqueId += 1;
        if (uniqueId >= int.MaxValue) {
            while (true)
            {
                bool used = false;
                for (int i = 0; i < idList.Count; i++)
                {
                    if (uniqueId == idList[i]) {
                        used = true;
                        break;
                    }
                }
                if (used)
                {
                    uniqueId += 1;
                }
                else {
                    break;
                }
            }
        }
        idList.Add(uniqueId);
        return uniqueId;
    }

    public void SetLog(Action<string> cb)
    {
        logCb = cb;
    }
    public void LogInfo(string str)
    {
        if (logCb != null)
        {
            logCb(str);
        }
    }

    public void SetHandleCb(Action<Action<int>, int> cb)
    {
        this.taskHandleCb = cb;
    }

    private double GetMillisecond(double value, TimeUnit unit) {
        switch (unit)
        {
            case TimeUnit.Millisecond:
                return value; 
            case TimeUnit.Second:
                return value * 1000;
            case TimeUnit.Minute:
                return value * 1000 * 60;
            case TimeUnit.Hour:
                return value * 1000 * 60 * 60;
            case TimeUnit.Day:
                return value * 1000 * 60 * 60 * 24;
            default:
                return value;
        }
    }


}




public class TimeTask {
    public int Id;
    public Action<int> Cb;
    public double destTime;
    public double intervalTime;
    public int count;

    public TimeTask(int id, Action<int> cb, double dest, double interval, int count) {
        this.Id = id;
        this.Cb = cb;
        this.destTime = dest;
        this.intervalTime = interval;
        this.count = count;
    }
}

public class FrameTask {
    public int Id;
    public Action<int> Cb;
    public int destFrame;
    public int intervalFrame;
    public int count;

    public FrameTask(int id, Action<int> cb, int dest, int interval, int count) {
        this.Id = id;
        this.Cb = cb;
        this.destFrame = dest;
        this.intervalFrame = interval;
        this.count = count;
    }
}


public enum TimeUnit {
    Millisecond,
    Second,
    Minute,
    Hour,
    Day,
}








