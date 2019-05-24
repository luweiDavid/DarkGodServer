
/****************************************************
	文件：LoginSys.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:17   	
	功能：登陆系统层
*****************************************************/

using PENet;
using Protocol;

public class LoginSys
{
    private static LoginSys instance;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginSys();
            }
            return instance;
        }
    }

    public void Init() {
        PETool.LogMsg("LoginSys Init Done");
    }

    public void HandleMsgPack(MsgPack pack) {
        //处理客户端登陆请求
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.RspLogin,
        };

        ReqLogin loginData = pack.msg.ReqLogin;
        string acct = loginData.account;
        string password = loginData.password;

        bool isOnline = CacheSvc.Instance.IsOnline(acct);
        if (isOnline)
        {
            newMsg.err = (int)ErrorCode.AlreadyOnline;
        }
        else
        {
            PlayerData playerData = CacheSvc.Instance.GetPlayerData(acct, password);
            if (playerData == null)
            {
                newMsg.err = (int)ErrorCode.InvalidPassword;
            }
            else {
                newMsg.RspLogin = new RspLogin
                {
                    data = playerData,
                };

                CacheSvc.Instance.CachePlayerData(acct, playerData, pack.session);
            }
        }
        pack.session.SendMsg(newMsg); 
    }

}