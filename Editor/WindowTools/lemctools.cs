using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace lgu3d.Editor
{
    public class lgu3dtools : EditorWindow
    {
        [MenuItem("Window/lgu3d")]
        public static void CreateView()
        {
            lgu3dtools newWindow = GetWindowWithRect<lgu3dtools>(new Rect(100, 100, 600, 400), false, "框架工具集合");
            newWindow.Init();
        }

        Dictionary<string, BaseTools> Tools;
        string CurrTools = "";
        public void Init()
        {
            Tools = new Dictionary<string, BaseTools>
            {
                //{ "样式演示工具", new StylesShowTools(this) },
                { "打包工具", new PackingTools(this) },
                { "压缩工具", new ZipEditorTools(this) },
                { "Excel导入工具", new ExeclImportTools(this) },
            };
        }


        #region 视图
        void OnGUI()
        {
            if (Tools == null)
            {
                Init();
            }
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            LeftView();
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (Tools.ContainsKey(CurrTools))
            {
                Tools[CurrTools].OnGUI();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        void LeftView()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(80), GUILayout.ExpandHeight(true));
            foreach (var item in Tools)
            {
                if (CurrTools != item.Key)
                {
                    if (GUILayout.Button(item.Key,Styles.ButtonNoSelectStyle, GUILayout.Height(40), GUILayout.ExpandWidth(true)))
                    {
                        CurrTools = item.Key;
                    }
                }
                else
                {
                    if (GUILayout.Button(item.Key,Styles.ButtonSelectStyle, GUILayout.Height(40), GUILayout.ExpandWidth(true)))
                    {
                        CurrTools = CurrTools = item.Key;
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
        #endregion


        void OnDestroy()
        {
            foreach (var item in Tools)
            {
                item.Value.OnDestroy();
            }
        }
    }

}
