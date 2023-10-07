using System;
using UnityEngine;
using UnityEngine.UI;

namespace lgu3d
{
    public enum VersionCheckErrorType
    {
        CannotAccessResService = 0,             //无法访问资源服务器
        AppVersionTooLow = 1,              //当前运行App版本过低
        ResDownlooadError = 2,              //资源下载错误
    }

    /// <summary>
    /// 邦本信息输出组件
    /// </summary>
    public interface IVersionManageInfoOutComp
    {
        void UpdataView(string TitleStr, string DescribeStr, float Progress);
        void UpdataView(string TitleStr, string DescribeStr, float Progress01, float Progress02);
        void CheckVersionError(VersionCheckErrorType errtype, Action retryFunc);

    }

    public class VersionManageDefaultInfoOutComp : Model_BaseViewComp<VersionManagerModule>, IVersionManageInfoOutComp
    {
        private Slider Progress1;
        private Text Progress1Describe;
        private Slider Progress2;
        private Text Progress2Describe;
        public override void Load(ModuleBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, "DefaultInfoOutView");
            Progress1 = UIGameobject.OnSubmit<Slider>("Progress1");
            Progress1Describe = Progress1.gameObject.OnSubmit<Text>("Describe");
            Progress2 = UIGameobject.OnSubmit<Slider>("Progress2");
            Progress2Describe = Progress2.gameObject.OnSubmit<Text>("Describe");
            LoadEnd();
        }

        public virtual void UpdataView(string TitleStr, string DescribeStr, float Progress)
        {
            Progress1.value = Progress;
            Progress1Describe.text = TitleStr + DescribeStr;
        }

        public virtual void UpdataView(string TitleStr, string DescribeStr, float Progress01, float Progress02)
        {
            Progress1.value = Progress01;
            Progress2.value = Progress02;
            Progress1Describe.text = TitleStr + DescribeStr;
        }

        public virtual void CheckVersionError(VersionCheckErrorType errtype, Action retryFunc)
        {
            switch (errtype)
            {
                case VersionCheckErrorType.CannotAccessResService:
                    //需要下载新的App
                    CommonModule.Instance.ShowBox("无法访问远程资源服务器", () =>
                    {
                        retryFunc?.Invoke();
                    }, () =>
                    {
                        Application.Quit();
                    });
                    break;
                case VersionCheckErrorType.AppVersionTooLow:
                    CommonModule.Instance.ShowBox("当前App版本过低!", () =>
                    {
                        retryFunc?.Invoke();
                    }, () =>
                    {
                        Application.Quit();
                    });
                    break;
                case VersionCheckErrorType.ResDownlooadError:
                    CommonModule.Instance.ShowBox("资源下载异常!", () =>
                    {
                        retryFunc?.Invoke();
                    }, () =>
                    {
                        Application.Quit();
                    });
                    break;
            }


        }
    }
}
