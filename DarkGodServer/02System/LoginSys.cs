
/****************************************************
	文件：LoginSys.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:17   	
	功能：登陆系统层
*****************************************************/

using PENet;

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

}