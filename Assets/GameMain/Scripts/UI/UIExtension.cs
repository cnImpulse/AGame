
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.UI;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;



namespace SSRPG
{
    public static class UIExtension
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static int? OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId, userData);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            var cfg = GameEntry.Cfg.Tables.TblUIForm.Get(uiFormId);
            string assetName = AssetUtl.GetUIFormAsset(cfg.AssetName);
            return uiComponent.OpenUIForm(assetName, cfg.UIGroup, userData);
        }
    }
}
