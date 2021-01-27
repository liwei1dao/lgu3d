using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace lgu3d
{
    /// <summary>
    /// App版本对比组件 只对比 资源校验模式为AppStartCheck的资源
    /// </summary>
    public class AppVersionContrastComp : ModelCompBase<VersionManagerModule>
    {

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        public void ContrastVersion(string AssetInfoUrl,AppModuleAssetInfo Localassetinfo,Action<AppModuleAssetInfo, List<ResBuileInfo>> CallBack, Action ErrorBack)
        {
            RequestAssetVersionInfo(AssetInfoUrl,(SerassetInfo) =>
            {
                List<ResBuileInfo> UpdataAssetinfo = ContrastAssetInfo(Localassetinfo, SerassetInfo);
                CallBack?.Invoke(SerassetInfo,UpdataAssetinfo);
            },ErrorBack);
        }

        private void RequestAssetVersionInfo(string AssetInfoUrl, Action<AppModuleAssetInfo> CallBack, Action ErrorBack)
        {
            Action<UnityWebRequest> func =  (UnityWebRequest result) => {
                if (!result.isNetworkError)
                {
                    AppModuleAssetInfo SerassetInfo = JsonTools.JsonStrToObject<AppModuleAssetInfo>(result.downloadHandler.text);
                    CallBack?.Invoke(SerassetInfo);
                }
                else {
                    ErrorBack?.Invoke();
                }
            };
            WebServiceModule.Instance.Get(AssetInfoUrl, func);
        }

        /// <summary>
        /// 对比资源信息文件
        /// </summary>
        /// <param name="LocalAssetInfo"></param>
        /// <param name="ServiceAssetInfo"></param>
        /// <returns></returns>
        private List<ResBuileInfo> ContrastAssetInfo(AppModuleAssetInfo LocalAssetInfo, AppModuleAssetInfo ServiceAssetInfo)
        {
            List<ResBuileInfo> UpdataAssetinfo = new List<ResBuileInfo>();
            List<string> ServiceKeys = new List<string>(ServiceAssetInfo.AppResInfo.Keys);
            List<string> LocalKeys = new List<string>(LocalAssetInfo.AppResInfo.Keys);
            for (int i = 0; i < ServiceKeys.Count; i++)
            {
                bool IsNewAsset = true;
                for (int j = 0; j < LocalKeys.Count; j++)
                {
                    if (ServiceKeys[i] == LocalKeys[j])
                    {
                        IsNewAsset = false;
                        if (ServiceAssetInfo.AppResInfo[ServiceKeys[i]].Md5 != LocalAssetInfo.AppResInfo[LocalKeys[j]].Md5)
                        {
                            UpdataAssetinfo.Add(ServiceAssetInfo.AppResInfo[ServiceKeys[i]]);
                        }
                    }
                }
                if (IsNewAsset)
                {
                    UpdataAssetinfo.Add(ServiceAssetInfo.AppResInfo[ServiceKeys[i]]);
                }
            }
            return UpdataAssetinfo;
        }
    }
}
