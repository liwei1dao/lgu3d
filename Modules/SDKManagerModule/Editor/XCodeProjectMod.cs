using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class XCodeProjectMod
{
   [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	{
		//Debug.Log ("path: " + path);
		if (buildTarget != BuildTarget.iOS) {
			return;
		}
		#if UNITY_IOS
		#region 修改工程
		string projPath = PBXProject.GetPBXProjectPath (path);
		//Debug.Log ("projPath: " + projPath);
		PBXProject proj = new PBXProject ();
		string fileText = File.ReadAllText (projPath);
		proj.ReadFromString (fileText);
		string targetName = PBXProject.GetUnityTargetName ();//Unity-iPhone
		string targetGuid = proj.TargetGuidByName (targetName);
        proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
        proj.AddBuildProperty (targetGuid, "OTHER_LDFLAGS", "-ObjC -force_load");
		proj.AddBuildProperty (targetGuid, "OTHER_LDFLAGS", "\"$(SRCROOT)/Libraries/lgu3d/Modules/SDKManagerModule/Plugins/WeChat/IOS/libWeChatSDK.a\"");
        //修改内容
        proj.AddFrameworkToProject(targetGuid, "AudioToolBox.framework", false);
        proj.AddFrameworkToProject(targetGuid, "CoreAudio.framework", false);
        proj.AddFrameworkToProject(targetGuid, "CoreLocation.framework", false);
        proj.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);
        proj.AddFrameworkToProject(targetGuid, "Security.framework", false);
        proj.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
        proj.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
        proj.AddFrameworkToProject(targetGuid, "libsqlite3.0.tbd", false);
        proj.AddFrameworkToProject(targetGuid, "libc++.tbd", false);
        proj.AddFrameworkToProject(targetGuid, "libz.tbd", false);
        proj.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
        proj.AddFrameworkToProject(targetGuid, "AVFoundation.framework", false);

        // save changed
        File.WriteAllText (projPath, proj.WriteToString ());
		#endregion

		#region 修改plist
		string plistPath = Path.Combine(path, "Info.plist");
		Debug.Log ("plistPath: " + plistPath);
		PlistDocument plist = new PlistDocument();
		string plistFileText = File.ReadAllText(plistPath);
		plist.ReadFromString(plistFileText);
		PlistElementDict rootDict = plist.root;

		rootDict.SetString("NSCameraUsageDescription", "App需要您的同意,才能访问相机");
		rootDict.SetString("NSLocationWhenInUseUsageDescription", "防作弊房间App需要您的同意,才能在使用期间访问位置");
		rootDict.SetString("NSMicrophoneUsageDescription", "App需要您的同意,才能访问麦克风");

		// add url scheme for wechat
		PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");
		PlistElementDict wxUrl = urlTypes.AddDict();
		wxUrl.SetString("CFBundleTypeRole","Editor");
		wxUrl.SetString("CFBundleURLName", "weixin");
		PlistElementArray wxUrlScheme = wxUrl.CreateArray("CFBundleURLSchemes");
		wxUrlScheme.AddString("wx1c943dc722b2da25");
        wxUrlScheme.AddString("zhujimajiang");
		wxUrlScheme.AddString("xianliao41QPTeLYs2h77tOi");
        // for wechat
        PlistElementArray queriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
        queriesSchemes.AddString("wechat");
		queriesSchemes.AddString("weixin");
		queriesSchemes.AddString("weixinULAPI");
		queriesSchemes.AddString("xianliao");
		// 保存修改
		File.WriteAllText(plistPath, plist.WriteToString());
		#endregion
		#endif
	}
}

