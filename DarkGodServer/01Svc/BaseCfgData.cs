﻿/****************************************************
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
 
public class CfgGuideData : BaseCfgData<CfgGuideData>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;

}

