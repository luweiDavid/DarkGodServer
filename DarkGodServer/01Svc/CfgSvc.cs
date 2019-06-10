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

        InitTaskRewardCfg();
        InitGuideCfg();
        InitStrongCfg(); 
    }


    #region taskreward
    public CfgTaskReward GetTaskRewardCfg(int id) {
        CfgTaskReward cfg = null;
        taskrewardDic.TryGetValue(id, out cfg);

        return cfg;
    }

    private Dictionary<int, CfgTaskReward> taskrewardDic = new Dictionary<int, CfgTaskReward>();
    private void InitTaskRewardCfg()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string inHaoXinStr = @"G:\Homework\DarkGod\Assets\Resources\Configs\taskreward.xml";
        string inHomeStr = @"E:\UnityPorjects\DarkGod\Assets\Resources\Configs\taskreward.xml";

        xmlDoc.Load(inHaoXinStr);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));

            CfgTaskReward cfg = new CfgTaskReward
            {
                ID = ID,
            };

            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            {
                string str = subEle.Name;
                switch (str)
                {
                    case "count":
                        cfg.count = int.Parse(subEle.InnerText);
                        break;
                    case "exp":
                        cfg.exp = int.Parse(subEle.InnerText);
                        break;
                    case "coin":
                        cfg.coin = int.Parse(subEle.InnerText);
                        break; 
                }
            }
            taskrewardDic.Add(cfg.ID, cfg);
        }
    }
    #endregion


    #region strong 
    public CfgStrong GetStrongCfg(int pos, int starLv)
    {
        CfgStrong cfg = null;
        Dictionary<int, CfgStrong> cfgDic = null;
        if (strongDic.TryGetValue(pos, out cfgDic))
        {
            if (cfgDic != null && cfgDic.TryGetValue(starLv, out cfg))
            {
                return cfg;
            }
        }
        return null;
    }

    private Dictionary<int, Dictionary<int, CfgStrong>> strongDic = new Dictionary<int, Dictionary<int, CfgStrong>>();
    private void InitStrongCfg()
    { 
        XmlDocument xmlDoc = new XmlDocument();
        string inHaoXinStr = @"G:\Homework\DarkGod\Assets\Resources\Configs\strong.xml";
        string inHomeStr = @"E:\UnityPorjects\DarkGod\Assets\Resources\Configs\strong.xml";
        xmlDoc.Load(inHaoXinStr);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
        Dictionary<int, CfgStrong> cfgDic = null;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));
            CfgStrong cfg = new CfgStrong
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

            if (strongDic.ContainsKey(cfg.pos))
            {
                cfgDic.Add(cfg.starLv, cfg);
            }
            else
            {
                cfgDic = new Dictionary<int, CfgStrong>();
                cfgDic.Add(cfg.starLv, cfg);
                strongDic.Add(cfg.pos, cfgDic);
            }
        }
    } 
    #endregion


    #region   引导配置表读取
    public CfgGuide GetGuideCfg(int id)
    {
        CfgGuide cfg = null;
        guideDic.TryGetValue(id, out cfg);
        return cfg;
    }

    private Dictionary<int, CfgGuide> guideDic = new Dictionary<int, CfgGuide>();

    private void InitGuideCfg() {
        XmlDocument xmlDoc = new XmlDocument();
        string inHaoXinStr = @"G:\Homework\DarkGod\Assets\Resources\Configs\guide.xml";
        string inHomeStr = @"E:\UnityPorjects\DarkGod\Assets\Resources\Configs\guide.xml";
         
        xmlDoc.Load(inHaoXinStr);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));
            CfgGuide cfg = new CfgGuide();
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

            guideDic.Add(ID, cfg);
        }
    }

    #endregion


}