
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.UI;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using Cfg.UI;

namespace SSRPG
{
    public static class UIExtension
    {
        public static int OpenUIForm(this UIComponent uiComponent, FormType type, object userData = null)
        {
            var cfg = GameEntry.Cfg.Tables.TblUIForm.Get(type);
            string assetName = AssetUtl.GetUIFormAsset(cfg.AssetName);
            return uiComponent.OpenUIForm(assetName, cfg.UIGroup, userData);
        }

        /// <summary>
        /// 界面不存在时不会报错
        /// </summary>
        public static void CloseUIForm(this UIComponent uiComponent, bool isShutdown, int serialId)
        {
            if (uiComponent.HasUIForm(serialId))
            {
                var form = uiComponent.GetUIForm(serialId).Logic as UIForm;
                form.Close(isShutdown);
            }
            else if (uiComponent.IsLoadingUIForm(serialId))
            {
                uiComponent.CloseUIForm(serialId);
            }

            return;
        }

        public static void CloseUIForm(this UIComponent uiComponent, UIForm form, bool isShutdown = false)
        {
            if (form == null)
            {
                return;
            }

            form.Close(isShutdown);
        }
    }
}
