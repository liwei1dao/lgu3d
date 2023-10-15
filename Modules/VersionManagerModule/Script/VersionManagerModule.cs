using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace lgu3d
{
    /// <summary>
    /// 版本检测模块
    /// </summary>
    public class VersionManagerModule : ManagerContorBase<VersionManagerModule>
    {

        private string ResServiceUrl;
        private LocalVersionCheckeComp LocalCheckeComp;              //本地环境检测组件
        private AppVersionContrastComp VersionContrastComp;     //App版本对比组件
        private VersionDownlooadComp DownlooadComp;             //下载组件
        public IVersionManageInfoOutComp InfoOutComp;

        public override void Load(params object[] agr)
        {
            ResourceComp = AddComp<Module_ResourceComp>();
            TimerComp = AddComp<Module_TimerComp>();
            CoroutineComp = AddComp<Module_CoroutineComp>();
            LocalCheckeComp = AddComp<LocalVersionCheckeComp>();
            VersionContrastComp = AddComp<AppVersionContrastComp>();
            DownlooadComp = AddComp<VersionDownlooadComp>();
            if (agr != null && agr.Length >= 1)
            {
                ResServiceUrl = (string)agr[0];
                if (agr.Length >= 2 && agr[1] is IVersionManageInfoOutComp)
                {
                    agr[1] = InfoOutComp as IVersionManageInfoOutComp;
                }
            }
            else
            {
                Debug.LogError("VersionManagerModule 启动参数异常");
            }
            base.Load(agr);
        }

        /// <summary>
        /// 开始App版本校验
        /// </summary>
        public void StartAppVersionCheck(Action<bool> Complete)
        {
            //先校验本地资源文件是否ok
            LocalCheckeComp.CheckeLocalVersion((LocalVersion, LocalAssetInfo) =>
            {
                string Url = ResServiceUrl + AppConfig.TargetPlatform.ToString() + "/VersionInfo.json";
                Action<UnityWebRequest> func = (UnityWebRequest result) =>
                {
                    if (string.IsNullOrEmpty(result.error))
                    {
                        Dictionary<string, int> ServiceVersion = JsonTools.JsonStrToDictionary<string, int>(result.downloadHandler.text);
                        if (ServiceVersion["ProVersion"] <= LocalVersion["ProVersion"])
                        {
                            if (ServiceVersion["ResVersion"] > LocalVersion["ResVersion"])
                            {
                                VersionContrastComp.ContrastVersion(ResServiceUrl + AppConfig.TargetPlatform.ToString() + "/AssetInfo.json", LocalAssetInfo, (sassetinfos, upres) =>
                                {
                                    if (upres != null && upres.Count >= 0)
                                    {
                                        DownloadTask[] tsdks = new DownloadTask[upres.Count];
                                        for (int i = 0; i < upres.Count; i++)
                                        {
                                            tsdks[i] = new DownloadTask()
                                            {
                                                Id = upres[i].Id,
                                                Url = ResServiceUrl + AppConfig.TargetPlatform.ToString() + "/" + upres[i].Id,
                                                FileName = AppConfig.AppAssetBundleTemp + "/" + upres[i].Id,
                                                Szie = upres[i].Size,
                                                IsComp = false,
                                            };
                                        }

                                        DownlooadComp.StartDownload(tsdks, (TasksGroup, Progress) =>
                                        {
                                            if (!TasksGroup.IsComp)
                                            {
                                                InfoOutComp.UpdataView("下载资源文件", TasksGroup.CurrTask.Url, Progress, TasksGroup.CurrTask.Progress);
                                            }
                                            else
                                            {
                                                InfoOutComp.UpdataView("下载资源文件", "下载完毕", Progress);
                                            }
                                        },
                                        (TasksGroup, Task) =>
                                        {
                                            string ResId = Task.Id;
                                            string ResPath = AppConfig.AppAssetBundleTemp + "/" + ResId;
                                            string TargetPath = AppConfig.AppAssetBundleAddress + "/" + ResId;
                                            FilesTools.CopyFile(ResPath, TargetPath);
                                            LocalAssetInfo.AppResInfo[ResId] = sassetinfos.AppResInfo[ResId];
                                            string AssetInfoStr = JsonTools.ObjectToJsonStr(LocalAssetInfo);
                                            FilesTools.WriteStrToFile(AppConfig.AppAssetBundleAddress + "/AssetInfo.json", AssetInfoStr);
                                            if (TasksGroup.IsComp)
                                            {
                                                string VersionStr = JsonTools.DictionaryToJsonStr<string, int>(ServiceVersion);
                                                FilesTools.WriteStrToFile(AppConfig.AppAssetBundleAddress + "/VersionInfo.json", VersionStr);
                                                FilesTools.ClearDirectory(AppConfig.AppAssetBundleTemp);
                                                Complete?.Invoke(true);
                                            }
                                        },
                                        (TasksGroup, Task) =>
                                        {
                                            InfoOutComp.CheckVersionError(VersionCheckErrorType.AppVersionTooLow, () =>
                                            {
                                                DownlooadComp.StartNextTask();      //继续任务
                                            });
                                        });
                                    }
                                    else
                                    {
                                        Complete?.Invoke(false);
                                    }
                                },
                                () =>
                                { //请求对比服务端资源info文件失败
                                    InfoOutComp.CheckVersionError(VersionCheckErrorType.CannotAccessResService, () =>
                                    {
                                        StartAppVersionCheck(Complete);
                                    });
                                });
                            }
                            else
                            {
                                Complete?.Invoke(false);
                            }
                        }
                        else
                        {
                            InfoOutComp.CheckVersionError(VersionCheckErrorType.AppVersionTooLow, () =>
                            {
                                Application.OpenURL("你的App下载地址");
                            });
                        }
                    }
                    else
                    {
                        InfoOutComp.CheckVersionError(VersionCheckErrorType.CannotAccessResService, () =>
                        {
                            StartAppVersionCheck(Complete);
                        });
                    }
                };
                WebServiceModule.Instance.Get(Url, func);
            });
        }

        //启动模块版本校验
        public void StartModuleVersionCheck(string ModuleName, Action<bool> Complete)
        {
            ModuleName = ModuleName.ToLower();
            LocalCheckeComp.CheckLoadModule(ModuleName, (LocalVersion, LocalAssetInfo) =>
            {
                string AssetUrl = ResServiceUrl + AppConfig.TargetPlatform.ToString() + "/" + ModuleName + "/AssetInfo.json";
                VersionContrastComp.ContrastVersion(AssetUrl, LocalAssetInfo, (sassetinfos, upres) =>
                {
                    if (upres != null && upres.Count >= 0)
                    {
                        DownloadTask[] tsdks = new DownloadTask[upres.Count];
                        for (int i = 0; i < upres.Count; i++)
                        {
                            tsdks[i] = new DownloadTask()
                            {
                                Id = upres[i].Id,
                                Url = ResServiceUrl + AppConfig.TargetPlatform.ToString() + "/" + upres[i].Id,
                                FileName = AppConfig.AppAssetBundleTemp + "/" + upres[i].Id,
                                Szie = upres[i].Size,
                                IsComp = false,
                            };
                        }

                        DownlooadComp.StartDownload(tsdks, (TasksGroup, Progress) =>
                        {
                            if (!TasksGroup.IsComp)
                            {
                                InfoOutComp.UpdataView("下载资源文件", TasksGroup.CurrTask.Url, Progress, TasksGroup.CurrTask.Progress);
                            }
                            else
                            {
                                InfoOutComp.UpdataView("下载资源文件", "下载完毕", Progress);
                            }
                        },
                        (TasksGroup, Task) =>
                        {
                            string ResId = Task.Id;
                            string ResPath = AppConfig.AppAssetBundleTemp + "/" + ResId;
                            string TargetPath = AppConfig.AppAssetBundleAddress + "/" + ResId;
                            FilesTools.CopyFile(ResPath, TargetPath);
                            LocalAssetInfo.AppResInfo[ResId] = sassetinfos.AppResInfo[ResId];
                            string AssetInfoStr = JsonTools.ObjectToJsonStr(LocalAssetInfo);
                            FilesTools.WriteStrToFile(AppConfig.AppAssetBundleAddress + ModuleName + "/AssetInfo.json", AssetInfoStr);
                            if (TasksGroup.IsComp)
                            {
                                FilesTools.ClearDirectory(AppConfig.AppAssetBundleTemp);
                                Complete?.Invoke(true);
                            }
                        },
                        (TasksGroup, Task) =>
                        {
                            //需要下载新的App
                            Debug.LogError("资源下载出现错误!");
                            // CommonModule.Instance.ShowBox("资源下载出现错误!", () => {
                            //     DownlooadComp.StartNextTask();      //继续任务
                            // }, () => {
                            //     Application.Quit();
                            // });
                        });
                    }
                    else
                    {
                        Complete?.Invoke(false);
                    }
                }, () =>
                {
                    InfoOutComp.CheckVersionError(VersionCheckErrorType.CannotAccessResService, () =>
                    {
                        StartAppVersionCheck(Complete);
                    });
                });
            });
        }
    }
}
