//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using GameFramework;
using UnityEditor;
using UnityEngine;

namespace StarForce.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Data Table/Generate DataTables")]
        private static void GenerateDataTables()
        {
            List<string> dataTableNames = GetDataTableNames();
            foreach (var name in dataTableNames)
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(name);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, name))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", name));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, name);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, name);
            }

            AssetDatabase.Refresh();
        }

        private static List<string> GetDataTableNames()
        {
            List<string> dataTableNames = new List<string>();
            dataTableNames.Add("Entity");
            dataTableNames.Add("GridMap");
            return dataTableNames;
        }
    }
}
