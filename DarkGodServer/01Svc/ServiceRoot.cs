


/****************************************************
	文件：ServiceRoot.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/31 15:54   	
	功能：服务系统基类
*****************************************************/
public class ServiceRoot<T> where T : new()
{
    //懒汉式单利
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    public virtual void Init()
    {


    }

}