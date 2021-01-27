using System.IO;
using UnityEngine;
using UnityEditor;

namespace lgu3d.Editor
{
    public class ZipEditorTools : BaseTools
    {
        private string SelectPath = "";
        private string outpath = "";
        private string ZipPassword;
        public ZipEditorTools(EditorWindow _MyWindow)
         : base(_MyWindow)
        {

        }

        public override void OnGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            GUILayout.Label("压缩文件或目录:");
            GUILayout.Label(SelectPath);
            if (GUILayout.Button("选择文件或目录", GUILayout.Width(100)))
            {
                SelectPath = EditorUtility.OpenFolderPanel("SelectResFile", Application.dataPath, "");
                if (SelectPath.Contains(Application.dataPath))
                {
                    SelectPath = SelectPath.Substring(Application.dataPath.Length, SelectPath.Length - Application.dataPath.Length);
                }
                else
                {
                    SelectPath = "";
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            GUILayout.Label("压缩文件输出目录:");
            GUILayout.Label(outpath);
            if (GUILayout.Button("选择输出目录", GUILayout.Width(100)))
            {
                outpath = EditorUtility.OpenFolderPanel("OutZipFile", Application.dataPath, "");
                if (outpath.Contains(Application.dataPath))
                {
                    outpath = outpath.Substring(Application.dataPath.Length, outpath.Length - Application.dataPath.Length);
                }
                else
                {
                    outpath = "";
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            GUILayout.Space(100);
            ZipPassword = EditorGUILayout.TextField("压缩文件密码", ZipPassword, GUILayout.Width(300));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            if (GUILayout.Button("压缩", GUILayout.Width(100), GUILayout.Height(60)))
            {
                if (SelectPath == "" || outpath == "")
                {
                    EditorUtility.DisplayDialog("提示", "请选择正确的文件路径", "Ok");
                }
                else {
                    CreateZip();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        public void CreateZip()
        {
            DirectoryInfo dir = new DirectoryInfo(Application.dataPath+SelectPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            string[] Files = new string[fileinfo.Length];
            for (int i = 0; i < fileinfo.Length; i++)
            {
                Files[i] = fileinfo[i].FullName;
            }
            EditorCoroutineRunner.StartEditorCoroutine(ZipTools.Zip(Files, Application.dataPath + outpath +"/"+ dir.Name+".zip", ZipPassword, new string[] { ".meta" }, UpdataZipProgress));
        }

        public static void UpdataZipProgress(string _Describe, float _Progress)
        {
            EditorUtility.DisplayProgressBar("压缩文件", _Describe, _Progress);
            if (_Progress >= 1)
                EditorUtility.ClearProgressBar();
        }
    }
}
