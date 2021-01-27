using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace lgu3d
{
    /// <summary>
    /// 版本校验组件
    /// </summary>
    public class LocalVersionCheckeComp : ModelCompBase<VersionManagerModule>
    {

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        /// <summary>
        /// 校验本地环境
        /// </summary>
        public void CheckeLocalVersion(Action<Dictionary<string, int>, AppModuleAssetInfo> CallBack)
        {
            string localVersionPath = AppConfig.AppAssetBundleAddress + "/VersionInfo.json";
            string localAssetPath = AppConfig.AppAssetBundleAddress + "/AssetInfo.json";
            bool IsSucc = FilesTools.IsKeepFileOrDirectory(localVersionPath);
            if (!IsSucc) //外部资源不存在 进行版本迁移处理
            {
                AssemblyLocalVersion(() =>
                {
                    string versionstr = FilesTools.ReadFileToStr(localVersionPath);
                    string assetinfostr = FilesTools.ReadFileToStr(localAssetPath);
                    Dictionary<string, int> LocalVersion = JsonTools.JsonStrToDictionary<string, int>(versionstr);
                    AppModuleAssetInfo LocalAssetInfo = JsonTools.JsonStrToObject<AppModuleAssetInfo>(assetinfostr);
                    CallBack?.Invoke(LocalVersion, LocalAssetInfo);

                });
            }
            else
            {
                string versionstr = FilesTools.ReadFileToStr(localVersionPath);
                string assetinfostr = FilesTools.ReadFileToStr(localAssetPath);
                Dictionary<string, int> LocalVersion = JsonTools.JsonStrToDictionary<string, int>(versionstr);
                AppModuleAssetInfo LocalAssetInfo = JsonTools.JsonStrToObject<AppModuleAssetInfo>(assetinfostr);
                CheckeAppInside(LocalVersion, LocalAssetInfo, CallBack);
            }

        }

        /// <summary>
        /// 对比App版本
        /// </summary>
        public void CheckeAppInside(Dictionary<string, int> LocalVersion, AppModuleAssetInfo LocalAssetInfo, Action<Dictionary<string, int>, AppModuleAssetInfo> CallBack)
        {
            float AppVersion = float.Parse(Application.version);
            if (LocalVersion["ProVersion"] < AppVersion)        //外部资源为上一个版本的资源文件，需要重新覆盖
            {
                AssemblyLocalVersion(() =>
                {
                    string localVersionPath = AppConfig.AppAssetBundleAddress + "/VersionInfo.json";
                    string localAssetPath = AppConfig.AppAssetBundleAddress + "/AssetInfo.json";
                    string versionstr = FilesTools.ReadFileToStr(localVersionPath);
                    string assetinfostr = FilesTools.ReadFileToStr(localAssetPath);
                    LocalVersion = JsonTools.JsonStrToDictionary<string, int>(versionstr);
                    LocalAssetInfo = JsonTools.JsonStrToObject<AppModuleAssetInfo>(assetinfostr);
                    CallBack?.Invoke(LocalVersion, LocalAssetInfo);
                });
            }
            else
            {
                CallBack?.Invoke(LocalVersion, LocalAssetInfo);
            }
        }

        private void AssemblyLocalVersion(Action CallBack)
        {
            MyModule.InfoOutComp.UpdataView("初次运行解压资源文件", "开始解压资源文件", 0.0f);
            MyModule.StartCoroutine(AssemblyAsset(CallBack));
        }

        IEnumerator AssemblyAsset(Action CallBack)
        {
            UnityWebRequest www = new UnityWebRequest(AppConfig.GetstreamingAssetsPath + "/Res.zip");
            DownloadHandlerBuffer dowle = new DownloadHandlerBuffer();
            www.downloadHandler = dowle;
            yield return www.SendWebRequest();
            if (www.error != null)
                Debug.Log(www.error);
            else
            {
                yield return ZipTools.UnzipFile(dowle.data, AppConfig.AppAssetBundleAddress, AppConfig.ResZipPassword, (string DescribeStr, float Progress) => {
                    MyModule.InfoOutComp.UpdataView("初次运行解压资源文件", DescribeStr, Progress);
                });
                if (CallBack != null)
                {
                    CallBack();
                }
                www.Dispose();
            }
        }


        //校验本地模块资源信息文件
        public void CheckLoadModule(string ModuleName, Action<Dictionary<string, int>, AppModuleAssetInfo> CallBack) {
            string localmodulepath = AppConfig.AppAssetBundleAddress + "/" + ModuleName + "/AssetInfo.json";
            bool IsSucc = FilesTools.IsKeepFileOrDirectory(localmodulepath);
            if (IsSucc)
            {
                string assetinfostr = FilesTools.ReadFileToStr(localmodulepath);
                AppModuleAssetInfo LocalAssetInfo = JsonTools.JsonStrToObject<AppModuleAssetInfo>(assetinfostr);
                CallBack?.Invoke(null, LocalAssetInfo);
            }
            else {
                CallBack?.Invoke(null, new AppModuleAssetInfo());
            }
        }
    }
}
