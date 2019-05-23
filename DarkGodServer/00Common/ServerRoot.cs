/****************************************************
	文件：ServerRoot.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:10   	
	功能：初始化各个系统
*****************************************************/

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
        NetSvc.Instance.Init();
        LoginSys.Instance.Init();

    }

}
