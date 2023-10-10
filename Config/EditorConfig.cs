using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace lgu3d.Editor
{
    public static class ToolsConfig
    {
        /// <summary>
        /// 可编译文件
        /// </summary>
        public static string[] CanBuildFileTypes = { ".prefab", ".unity", ".asset", ".fbx", ".mat", ".tga", ".wav", ".ogg", ".mp3", ".ttc", ".png", ".jpg", ".bytes", ".lua", ".proto" };

        /// <summary>
        /// 工具开发资源目录 完整路径
        /// </summary>
        public static string CompleteEditorResources
        {
            get
            {
                return Path.Combine(Application.dataPath, "lgu3d/Editor/ToolsResources");
            }
        }

        /// <summary>
        /// 工具开发资源目录 相对路径
        /// </summary>
        public static string RelativeEditorResources
        {
            get
            {
                return "Assets/lgu3d/Editor/ToolsResources";
            }
        }
    }
}
