using System;
using UnityEditor;
using UnityEngine;

namespace lgu3d.Editor
{
    public class ExcelImportWindow: BaseTools<ExeclImportTools>
    {

        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 FilesViewscrollPosition = Vector2.zero;
        public ExcelImportWindow(EditorWindow _MyWindow, ExeclImportTools _Parent)
            : base(_MyWindow, _Parent)
        {

        }

        public override void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            GUILayout.Label("表格所在目录:", GUILayout.Width(60));
            GUILayout.Label(Parent.ScannPath, GUILayout.Width(150));
            EditorGUILayout.Space();
            if (GUILayout.Button("选择数据导入接口", GUILayout.Width(80)))
            {
                Parent.ScannPath = EditorUtility.OpenFolderPanel("SelectResFile", Application.dataPath, "");
                if (Parent.ScannPath.Contains(Application.dataPath))
                {
                    Parent.ScannPath = Parent.ScannPath.Substring(Application.dataPath.Length, Parent.ScannPath.Length - Application.dataPath.Length);
                    if (Parent.SavePath == ""){
                        Parent.SavePath = Parent.ScannPath;
                    }
                }
                else
                {
                    Parent.ScannPath = "";
                }
                Parent.ScannPathDirectory();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            GUILayout.Label("表格输出目录:", GUILayout.Width(60));
            GUILayout.Label(Parent.SavePath, GUILayout.Width(150));
            EditorGUILayout.Space();
            if (GUILayout.Button("选择导出路径", GUILayout.Width(80)))
            {
                Parent.SavePath = EditorUtility.OpenFolderPanel("SelectSaveFile", AppConfig.PlatformRoot, "");
                if (Parent.SavePath.Contains(AppConfig.PlatformRoot))
                {
                    Parent.SavePath = Parent.SavePath.Substring(AppConfig.PlatformRoot.Length, Parent.SavePath.Length - AppConfig.PlatformRoot.Length);
                }
                else
                {
                    Parent.SavePath = "";
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            FilesViewscrollPosition = GUILayout.BeginScrollView(FilesViewscrollPosition, EditorStyles.helpBox, GUILayout.MaxWidth(400));
            foreach (var item in Parent.Files)
            {
                item.OnGUI();
            }
            GUILayout.EndScrollView();
            ShowRightView();
            EditorGUILayout.EndHorizontal();
        }

        #region 右边控制界面
        private void ShowRightView()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            GUILayout.BeginVertical(EditorStyles.textField);
            GUILayout.Label("选择导入数据类型");
            Parent.importDataType = (ImputFileType)EditorGUILayout.EnumPopup(Parent.importDataType, GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
            GUILayout.Label("刷新数据文件列表", EditorStyles.centeredGreyMiniLabel);
            bool RefreshClick = GUILayout.Button("刷新", EditorStyles.miniButtonRight, GUILayout.Height(50));
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.Space(50);
            GUILayout.BeginVertical(EditorStyles.textField);
            GUILayout.Label("选择导出数据类型");
            Parent.exportDataType = (ExportFileType)EditorGUILayout.EnumPopup(Parent.exportDataType, GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
            GUILayout.Label("导出对应的数据文件", EditorStyles.centeredGreyMiniLabel);
            bool ExportClick = GUILayout.Button("导出", EditorStyles.miniButtonRight, GUILayout.Height(50));
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            if (RefreshClick)
            {
                Parent.ScannPathDirectory();
            }
            if (ExportClick)
            {
                Check();
            }
        }

        private void Check()
        {
            if (string.IsNullOrEmpty(Parent.SavePath))
            {
                bool option = EditorUtility.DisplayDialog(
                    "错误",
                    "请指定保存路径",
                    "ok");
                if (option)
                {
                    Debug.LogError("知道错了!");
                }
            }
            else
            {
                if (Parent.importDataType.ToString() == Parent.exportDataType.ToString())
                {
                    bool option = EditorUtility.DisplayDialog(
                       "错误",
                       "输出文件类型等于输入文件类型",
                       "ok");
                    if (option)
                    {
                        Debug.LogError("知道错了!");
                    }
                }
                else
                {
                    Parent.ImportFile();
                }
            }
        }
        #endregion
    }
}
