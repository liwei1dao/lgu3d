using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class WeChat_XcodeProjectMod
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

// 		proj.AddBuildProperty (targetGuid, "OTHER_LDFLAGS", "-ObjC -force_load");
// 		proj.AddFrameworkToProject(targetGuid, "WebKit.framework",false);

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

// 		// add url scheme for wechat
// 		PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");
// 		PlistElementDict wxUrl = urlTypes.AddDict();
// 		wxUrl.SetString("CFBundleTypeRole","Editor");
// 		wxUrl.SetString("CFBundleURLName", "weixin");
// 		PlistElementArray wxUrlScheme = wxUrl.CreateArray("CFBundleURLSchemes");
// 		wxUrlScheme.AddString("wx1c943dc722b2da25");
// 		wxUrlScheme.AddString("xianliao41QPTeLYs2h77tOi");

//         // for wechat
//         PlistElementArray queriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
// 		queriesSchemes.AddString("weixin");
// 		queriesSchemes.AddString("weixinULAPI");
// 		queriesSchemes.AddString("xianliao");


// 		// 保存修改
// 		File.WriteAllText(plistPath, plist.WriteToString());

// 		#endregion
// 		#endif
// 	}
}

