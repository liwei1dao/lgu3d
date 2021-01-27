using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 调试帮助类
/// </summary>
namespace lgu3d.Editor
{
    public static class EditorHelper
    {
        private static Func<Rect> VisibleRect;
        public static void InitType()
        {
            var tyGUIClip = Type.GetType("UnityEngine.GUIClip,UnityEngine");
            if (tyGUIClip != null)
            {
                var piVisibleRect = tyGUIClip.GetProperty("visibleRect", BindingFlags.Static | BindingFlags.Public);
                if (piVisibleRect != null)
                    VisibleRect = (Func<Rect>)Delegate.CreateDelegate(typeof(Func<Rect>), piVisibleRect.GetGetMethod());
            }
        }

        public static Rect visibleRect
        {
            get
            {
                InitType();
                return VisibleRect();
            }
        }

        /// <summary>
        /// 获取选择目录
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path;
        }

    }
}