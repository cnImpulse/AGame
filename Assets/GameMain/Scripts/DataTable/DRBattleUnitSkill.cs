//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-10-05 19:36:18.414
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 战斗单位技能表。
    /// </summary>
    public class DRBattleUnitSkill : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取技能编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取技能名。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取伤害倍率。
        /// </summary>
        public float DamageRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取基础释放范围。
        /// </summary>
        public int ReleaseRange
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取基础释放MP消耗。
        /// </summary>
        public int MPCost
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Name = columnStrings[index++];
            DamageRate = float.Parse(columnStrings[index++]);
            ReleaseRange = int.Parse(columnStrings[index++]);
            MPCost = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Name = binaryReader.ReadString();
                    DamageRate = binaryReader.ReadSingle();
                    ReleaseRange = binaryReader.Read7BitEncodedInt32();
                    MPCost = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
