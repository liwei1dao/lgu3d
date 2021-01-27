using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class PhoneSys_XcodeProjectMod
{
    // [PostProcessBuildAttribute(1)]
    // public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	// {
	// 	//Debug.Log ("path: " + path);
	// 	if (buildTarget != BuildTarget.iOS) {
	// 		return;
	// 	}
	// 	#if UNITY_IOS
	// 	#region 修改工程
	// 	string projPath = PBXProject.GetPBXProjectPath (path);
	// 	PBXProject proj = new PBXProject ();
	// 	string fileText = File.ReadAllText (projPath);
	// 	proj.ReadFromString (fileText);
	// 	string targetName = PBXProject.GetUnityTargetName ();//Unity-iPhone
	// 	string targetGuid = proj.TargetGuidByName (targetName);


    //     File.WriteAllText (projPath, proj.WriteToString ());
	// 	#endregion

	// 	#region 修改plist
	// 	string plistPath = Path.Combine(path, "Info.plist");
	// 	Debug.Log ("plistPath: " + plistPath);
	// 	PlistDocument plist = new PlistDocument();
	// 	string plistFileText = File.ReadAllText(plistPath);
	// 	plist.ReadFromString(plistFileText);
	// 	PlistElementDict rootDict = plist.root;
    //     //设置语言环境
    //     rootDict.SetString("CFBundleDevelopmentRegion","zh_CN");
    //     //一些权限声明
	// 	rootDict.SetString("NSCameraUsageDescription", "App需要您的同意,才能访问相机");
	// 	rootDict.SetString("NSLocationWhenInUseUsageDescription", "防作弊房间App需要您的同意,才能在使用期间访问位置");
	// 	rootDict.SetString("NSMicrophoneUsageDescription", "App需要您的同意,才能访问麦克风");
	// 	// 保存修改
	// 	File.WriteAllText(plistPath, plist.WriteToString());

	// 	#endregion
	// 	#endif
	// }
}

