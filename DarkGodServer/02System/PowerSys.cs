
/****************************************************
	文件：PowerSys.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/06/06 11:40   	
	功能：体力恢复系统
*****************************************************/


using Protocol;
using System.Collections.Generic;

public class PowerSys:SystemRoot<PowerSys>
{ 
    public void AddPowerAddTimeTask() {
        TimerSvc.Instance.AddTimeTask(NtfAddPowerTask, PECommonTool.AddPowerTimeSpan, -1, TimeUnit.Minute);
    }
     

    /// <summary>
    /// 通知玩家体力恢复
    /// </summary>
    /// <param name="id"></param>
    private void NtfAddPowerTask(int id) {
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.NtfPowerChg,
        };
        newMsg.NtfPowerChg = new NtfPowerChg();
        #region  通知在线玩家
        List<ServerSession> _list = CacheSvc.Instance.GetOnlineServerSes();
        for (int i = 0; i < _list.Count; i++)
        {
            PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(_list[i]);
            if (pd.Power < PECommonTool.GetPowerLimit(pd.Level))
            {
                int leftValue = PECommonTool.GetPowerLimit(pd.Level) - pd.Power;
                if (leftValue > PECommonTool.AddPowerPerTimes)
                {
                    pd.Power += PECommonTool.AddPowerPerTimes;
                }
                else {
                    pd.Power += leftValue;
                }

                if (!CacheSvc.Instance.UpdatePlayerDataToDB(pd.ID, pd))
                {
                    newMsg.err = (int)ErrorCode.UpdateDBFailed;
                }
                else {
                    newMsg.NtfPowerChg.data = pd;
                }
            }
            else {
                continue;
            }  
           
            byte[] btys = PENet.PETool.PackNetMsg(newMsg);
            _list[i].SendMsg(btys);
        }
        #endregion
        
        //离线玩家的体力恢复通过增加time字段去计算
    }
}