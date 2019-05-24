/****************************************************
	文件：ServerStart.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:09   	
	功能：服务器启动入口
*****************************************************/
class ServerStart
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();

        while (true) {
            ServerRoot.Instance.Update();

        }
    }
}