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
    }



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
        xmlDoc.Load(@"G:\Homework\DarkGod\Assets\Resources\Configs\guide.xml");

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