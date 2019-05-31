
using System;

namespace Protocol
{
    [Serializable]
    public class PlayerData
    {
        public int ID;
        public string Name;
        public int Level;
        public int Experience;   //经验
        public int Power;        //体力
        public int Coin;         //金币
        public int Diamond;     //钻石
        public int Hp;          //血量
        public int Ad;          //物理攻击
        public int Ap;          //法术攻击
        public int Addef;       //物理防御
        public int Apdef;       //法术防御
        public int Dodge;       //闪避
        public int Pierce;      //穿透
        public int Critical;    //暴击率
        public int GuideID;    //引导id
         

        public PlayerData(int id, string name, int lv, int exp, int power, int coin, int diamond,
            int hp, int ad, int ap, int addef, int apdef, int dodge, int pierce, int critical,
            int guideid) {
            this.ID = id;
            this.Name = name;
            this.Level = lv;
            this.Experience = exp;
            this.Power = power;
            this.Coin = coin;
            this.Diamond = diamond;
            this.Hp = hp;
            this.Ad = ad;
            this.Ap = ap;
            this.Addef = addef;
            this.Apdef = apdef;
            this.Dodge = dodge;
            this.Pierce = pierce;
            this.Critical = critical;
            this.GuideID = guideid;
        }

        /// <summary>
        /// 默认构造函数对新账号数据进行赋值
        /// </summary>
        public PlayerData() {
            this.ID = -1;
            this.Name = "";
            this.Level = 1;
            this.Experience = 0;
            this.Power = 100;
            this.Coin = 1000;
            this.Diamond = 5;
            this.Hp = 100;
            this.Ad = 10;
            this.Ap = 5;
            this.Addef = 10;
            this.Apdef = 10;
            this.Dodge = 5;
            this.Pierce = 5;
            this.Critical = 1;
            this.GuideID = 1001;
        }
    }
}
