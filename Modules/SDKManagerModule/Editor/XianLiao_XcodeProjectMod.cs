using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class XianLiao_XcodeProjectMod
{
//    [PostProcessBuildAttribute(1)]
//     public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
// 	{
// 		//Debug.Log ("path: " + path);
// 		if (buildTarget != BuildTarget.iOS) {
// 			return;
// 		}
// 		#if UNITY_IOS
// 		#region 修改工程
// 		string projPath = PBXProject.GetPBXProjectPath (path);
// 		//Debug.Log ("projPath: " + projPath);
// 		PBXProject proj = new PBXProject ();
// 		string fileText = File.ReadAllText (projPath);
// 		proj.ReadFromString (fileText);
// 		string targetName = PBXProject.GetUnityTargetName ();//Unity-iPhone
// 		string targetGuid = proj.TargetGuidByName (targetName);
// 		//修改内容
 	

//         // save changed
//         File.WriteAllText (projPath, proj.WriteToString ());
// 		#endregion

// 		#region 修改plist
// 		string plistPath = Path.Combine(path, "Info.plist");
// 		Debug.Log ("plistPath: " + plistPath);
// 		PlistDocument plist = new PlistDocument();
// 		string plistFileText = File.ReadAllText(plistPath);
// 		plist.ReadFromString(plistFileText);
// 		PlistElementDict rootDict = plist.root;
// 		//修改内容
// 		PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");
// 		PlistElementDict Urltypes = urlTypes.AddDict();
// 		PlistElementArray UrlScheme = Urltypes.CreateArray("CFBundleURLSchemes");
//         UrlScheme.AddString("xianliaolgOiVgKwceYP1G9X");
// 		// 保存修改
// 		File.WriteAllText(plistPath, plist.WriteToString());

// 		#endregion
// 		#endif
// 	}
}

