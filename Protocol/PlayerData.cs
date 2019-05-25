
using System;

namespace Protocol
{
    [Serializable]
    public class PlayerData
    {
        public int ID;
        public string Name;
        public int Level;
        public int Experience;
        public int Power;
        public int Coin;
        public int Diamond;

        public PlayerData(int id, string name, int lv, int exp, int power, int coin, int diamond) {
            this.ID = id;
            this.Name = name;
            this.Level = lv;
            this.Experience = exp;
            this.Power = power;
            this.Coin = coin;
            this.Diamond = diamond;
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
        }
    }
}
