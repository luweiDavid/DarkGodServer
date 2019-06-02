/****************************************************
	文件：BaseCfgData.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/31 16:02   	
	功能：配置表数据结构
*****************************************************/


public class BaseCfgData<T>
{
    public int ID;
}

public class CfgStrongData : BaseCfgData<CfgStrongData>
{
    public int pos;
    public int starLv;
    public int addHp;
    public int addHurt;
    public int addDef;
    public int minLv;
    public int coin;
    public int crystal;

}

public class CfgGuideData : BaseCfgData<CfgGuideData>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;

}

