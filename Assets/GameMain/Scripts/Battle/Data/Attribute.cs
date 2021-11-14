using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SSRPG
{
    public struct Attribute
    {
        public int HP  { get; private set; }  // 生命值
        public int MP  { get; private set; }  // 魔法值
        public int MOV { get; private set; }  // 移动力
        public int STR { get; private set; }  // 力量
        public int SPR { get; private set; }  // 魔力
        public int DEF { get; private set; }  // 防御
        public int RES { get; private set; }  // 魔抗
        public int SKL { get; private set; }  // 技巧
        public int SPD { get; private set; }  // 速度
        public int LUK { get; private set; }  // 幸运

        public Attribute(Cfg.Battle.Attribute attribute)
        {
            HP = attribute.HP;
            MP = attribute.MP;
            MOV = attribute.MOV;
            STR = attribute.STR;
            SPR = attribute.SPR;
            DEF = attribute.DEF;
            RES = attribute.RES;
            SKL = attribute.SKL;
            SPD = attribute.SPD;
            LUK = attribute.LUK;
        }

        public Attribute(Cfg.Battle.Talent talent)
        {
            MOV = 0;
            HP = GetRandom(talent.HP);
            MP = GetRandom(talent.MP);
            STR = GetRandom(talent.STR);
            SPR = GetRandom(talent.SPR);
            DEF = GetRandom(talent.DEF);
            RES = GetRandom(talent.RES);
            SKL = GetRandom(talent.SKL);
            SPD = GetRandom(talent.SPD);
            LUK = GetRandom(talent.LUK);
        }

        private static int GetRandom(float probability)
        {
            if (Random.Range(0f, 1f) < probability)
            {
                return 1;
            }

            return 0;
        }

        public static Attribute operator +(Attribute a, Attribute b)
        {
            return new Attribute
            {
                HP = a.HP + b.HP,
                MP = a.MP + b.MP,
                MOV = a.MOV + b.MOV,
                STR = a.STR + b.STR,
                SPR = a.SPR + b.SPR,
                DEF = a.DEF + b.DEF,
                RES = a.RES + b.RES,
                SKL = a.SKL + b.SKL,
                SPD = a.SPD + b.SPD,
                LUK = a.LUK + b.LUK,
            };
        }

        public static Attribute operator *(Attribute a, int k)
        {
            return new Attribute
            {
                HP = a.HP * k,
                MP = a.MP * k,
                MOV = a.MOV * k,
                STR = a.STR * k,
                SPR = a.SPR * k,
                DEF = a.DEF * k,
                RES = a.RES * k,
                SKL = a.SKL * k,
                SPD = a.SPD * k,
                LUK = a.LUK * k,
            };
        }
    }
}