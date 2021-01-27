using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lgu3d
{
    /// <summary>
    /// 版本下载组件
    /// </summary>
    public class VersionDownlooadComp : Module_DownloadComp<VersionManagerModule>
    {
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        public void StartDownload(DownloadTask[] tsdks, DownloadProgress downloadBack = null, TaskCompleted compTaskBack = null, TaskError errorTaskBack = null)
        {
            DownloadTaskGroup Task = new DownloadTaskGroup(tsdks, downloadBack, compTaskBack, errorTaskBack);
            AddTask(Task);
        }
    }
}
