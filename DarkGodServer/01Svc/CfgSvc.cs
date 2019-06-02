/****************************************************
	文件：CfgSvc.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/23 16:34   	
	功能：配置服务层
*****************************************************/
using System;
using System.Collections.Generic;
using System.Xml;

public class CfgSvc : ServiceRoot<CfgSvc>
{

    public override void Init()
    {
        base.Init();

        InitGuideCfg();
        InitStrongCfg();
    }

    #region
    public CfgStrongData GetStrongData(int pos, int starLv)
    {
        CfgStrongData data = null;
        Dictionary<int, CfgStrongData> dataDic = null;
        if (strongDataDic.TryGetValue(pos, out dataDic))
        {
            if (dataDic != null && dataDic.TryGetValue(starLv, out data))
            {
                return data;
            }
        }
        return null;
    }

    private Dictionary<int, Dictionary<int, CfgStrongData>> strongDataDic = new Dictionary<int, Dictionary<int, CfgStrongData>>();
    private void InitStrongCfg()
    { 
        XmlDocument xmlDoc = new XmlDocument();
        string inHaoXinStr = @"G:\Homework\DarkGod\Assets\Resources\Configs\strong.xml";
        string inHomeStr = @"E:\UnityPorjects\DarkGod\Assets\Resources\Configs\strong.xml";
        xmlDoc.Load(inHomeStr);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
        Dictionary<int, CfgStrongData> dataDic = null;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));
            CfgStrongData cfg = new CfgStrongData
            {
                ID = ID,
            };

            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            {
                string str = subEle.Name;
                switch (str)
                {
                    case "pos":
                        cfg.pos = int.Parse(subEle.InnerText);
                        break;
                    case "starlv":
                        cfg.starLv = int.Parse(subEle.InnerText);
                        break;
                    case "addhp":
                        cfg.addHp = int.Parse(subEle.InnerText);
                        break;
                    case "addhurt":
                        cfg.addHurt = int.Parse(subEle.InnerText);
                        break;
                    case "adddef":
                        cfg.addDef = int.Parse(subEle.InnerText);
                        break;
                    case "minlv":
                        cfg.minLv = int.Parse(subEle.InnerText);
                        break;
                    case "coin":
                        cfg.coin = int.Parse(subEle.InnerText);
                        break;
                    case "crystal":
                        cfg.crystal = int.Parse(subEle.InnerText);
                        break;
                }
            }

            if (strongDataDic.ContainsKey(cfg.pos))
            {
                dataDic.Add(cfg.starLv, cfg);
            }
            else
            {
                dataDic = new Dictionary<int, CfgStrongData>();
                dataDic.Add(cfg.starLv, cfg);
                strongDataDic.Add(cfg.pos, dataDic);
            }
        }
    } 
    #endregion


    #region   引导配置表读取
    public CfgGuideData GetGuideData(int id)
    {
        CfgGuideData cfg = null;
        guideDataDic.TryGetValue(id, out cfg);
        return cfg;
    }

    private Dictionary<int, CfgGuideData> guideDataDic = new Dictionary<int, CfgGuideData>();

    private void InitGuideCfg() {
        XmlDocument xmlDoc = new XmlDocument();
        string inHaoXinStr = @"G:\Homework\DarkGod\Assets\Resources\Configs\guide.xml";
        string inHomeStr = @"E:\UnityPorjects\DarkGod\Assets\Resources\Configs\guide.xml";
         
        xmlDoc.Load(inHomeStr);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));
            CfgGuideData cfg = new CfgGuideData();
            cfg.ID = ID;

            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            {
                string str = subEle.Name;
                switch (str)
                {
                    case "npcID":
                        cfg.npcID = int.Parse(subEle.InnerText);
                        break; 
                    case "actID":
                        cfg.actID = int.Parse(subEle.InnerText);
                        break;
                    case "coin":
                        cfg.coin = int.Parse(subEle.InnerText);
                        break;
                    case "exp":
                        cfg.exp = int.Parse(subEle.InnerText);
                        break;
                }
            }

            guideDataDic.Add(ID, cfg);
        }
    }

    #endregion


}