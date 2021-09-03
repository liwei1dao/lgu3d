using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEditor;

using BindType = ToLuaMenu.BindType;
using System.Reflection;

public static class CustomSettings
{
  public static string saveDir = LuaConfig.ukuaDir + "Source/Generate/";
  public static string toluaBaseType = LuaConfig.ukuaDir + "ToLua/BaseType/";
  public static string injectionFilesPath = LuaConfig.ukuaDir + "ToLua/Injection/";

  //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
  //unity 有些类作为sealed class, 其实完全等价于静态类
  public static List<Type> staticClassTypes = new List<Type>
    {
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
        typeof(UnityEngine.QualitySettings),
        typeof(UnityEngine.GL),
        typeof(UnityEngine.Graphics),
    };

  //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
  public static DelegateType[] customDelegateList =
  {
        _DT(typeof(Action)),
        _DT(typeof(UnityEngine.Events.UnityAction)),
        _DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Action<int>)),
        _DT(typeof(System.Comparison<int>)),
        _DT(typeof(System.Func<int, int>)),
    };

  //在这里添加你要导出注册到lua的类型列表
  public static BindType[] customTypeList =
  {                
        //------------------------为例子导出--------------------------------
        //_GT(typeof(TestEventListener)),
        //_GT(typeof(TestProtol)),
        //_GT(typeof(TestAccount)),
        //_GT(typeof(Dictionary<int, TestAccount>)).SetLibName("AccountMap"),
        //_GT(typeof(KeyValuePair<int, TestAccount>)),
        //_GT(typeof(Dictionary<int, TestAccount>.KeyCollection)),
        //_GT(typeof(Dictionary<int, TestAccount>.ValueCollection)),
        //_GT(typeof(TestExport)),
        //_GT(typeof(TestExport.Space)),
        //-------------------------------------------------------------------        
                        
        _GT(typeof(LuaInjectionStation)),
        _GT(typeof(InjectType)),
        _GT(typeof(Debugger)).SetNameSpace(null),          

#if USING_DOTWEENING
        _GT(typeof(DG.Tweening.DOTween)),
        _GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
        _GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.LoopType)),
        _GT(typeof(DG.Tweening.PathMode)),
        _GT(typeof(DG.Tweening.PathType)),
        _GT(typeof(DG.Tweening.Ease)),
        _GT(typeof(DG.Tweening.RotateMode)),
        _GT(typeof(DG.Tweening.Core.DOTweenComponent)),
        _GT(typeof(Component))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions))
            .AddExtendType(typeof(lgu3d.GameObjectExtend))
            .AddExtendType(typeof(lgu3d.LuaHelpTools)),
        _GT(typeof(RectTransform))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions46))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions43))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions50))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Transform))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions46))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions43))
            .AddExtendType(typeof(DG.Tweening.ShortcutExtensions50))
            .AddExtendType(typeof(lgu3d.UIDisplayTween))
            .AddExtendType(typeof(lgu3d.GameObjectExtend))
            .AddExtendType(typeof(lgu3d.LuaHelpTools)),
        _GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        //_GT(typeof(LineRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        //_GT(typeof(TrailRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),    
#else                                   
        _GT(typeof(Component)).AddExtendType(typeof(lgu3d.GameObjectExtend)),
        _GT(typeof(Transform)).AddExtendType(typeof(lgu3d.GameObjectExtend)),
        _GT(typeof(RectTransform)).AddExtendType(typeof(lgu3d.GameObjectExtend)),
        _GT(typeof(Material)),
        _GT(typeof(Light)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Camera)),
        _GT(typeof(AudioSource)),
        //_GT(typeof(LineRenderer))
        //_GT(typeof(TrailRenderer))
#endif
      
        _GT(typeof(PlayerPrefs)),
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),
        _GT(typeof(GameObject))
            .AddExtendType(typeof(lgu3d.GameObjectExtend))
            .AddExtendType(typeof(lgu3d.LuaHelpTools)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(Application)),
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(Time)),
        _GT(typeof(Texture)),
        _GT(typeof(Texture2D)),
        _GT(typeof(Shader)),
        _GT(typeof(Renderer)),
        _GT(typeof(Screen)),
        _GT(typeof(CameraClearFlags)),
        _GT(typeof(AudioClip)),
        _GT(typeof(AssetBundle)),
        //_GT(typeof(ParticleSystem)),
        _GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),
        _GT(typeof(LightType)),
        _GT(typeof(SleepTimeout)),
#if UNITY_5_3_OR_NEWER && !UNITY_5_6_OR_NEWER
        _GT(typeof(UnityEngine.Experimental.Director.DirectorPlayer)),
#endif
        _GT(typeof(Animator)),
        _GT(typeof(Input)),
        _GT(typeof(KeyCode)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Space)),


        //_GT(typeof(MeshRenderer)),
#if !UNITY_5_4_OR_NEWER
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)), 
#endif

        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),
        _GT(typeof(CharacterController)),
        _GT(typeof(CapsuleCollider)),

        _GT(typeof(Animation)),
        _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),
        _GT(typeof(AnimationState)),
        _GT(typeof(AnimationBlendMode)),
        _GT(typeof(QueueMode)),
        _GT(typeof(PlayMode)),
        _GT(typeof(WrapMode)),

        _GT(typeof(QualitySettings)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(SkinWeights)),
        _GT(typeof(RenderTexture)),
        _GT(typeof(Resources)),
        _GT(typeof(LuaProfiler)),



        //-------------------------------------------------------------扩展添加-------------------------------------------------------------------------------------
         _GT(typeof(UnityEngine.Debug)),
        _GT(typeof(UnityEngine.Networking.UnityWebRequest)),
        _GT(typeof(UnityEngine.Networking.DownloadHandlerFile)),
        _GT(typeof(UnityEngine.Networking.DownloadHandlerBuffer)),
        _GT(typeof(UnityEngine.NetworkReachability)),
        _GT(typeof(UnityEngine.UI.GraphicRaycaster)),
        _GT(typeof(UnityEngine.UI.CanvasScaler)),
        _GT(typeof(UnityEngine.UI.CanvasScaler.ScaleMode)),
        _GT(typeof(UnityEngine.UI.Button)),
        _GT(typeof(UnityEngine.UI.InputField)),
        _GT(typeof(UnityEngine.UI.Text)),
        _GT(typeof(UnityEngine.UI.RawImage)),
        _GT(typeof(UnityEngine.UI.Image)),
        _GT(typeof(UnityEngine.UI.Slider)),
        _GT(typeof(UnityEngine.UI.ScrollRect)),
        _GT(typeof(UnityEngine.UI.Toggle)),
        _GT(typeof(UnityEngine.UI.ToggleGroup)),
        _GT(typeof(UnityEngine.TextAsset)),
        _GT(typeof(UnityEngine.Sprite)),
        _GT(typeof(UnityEngine.RuntimePlatform)),
        _GT(typeof(UnityEngine.EventSystems.EventSystem)),
        _GT(typeof(UnityEngine.EventSystems.StandaloneInputModule)),
        _GT(typeof(UnityEngine.RenderMode)),
        _GT(typeof(UnityEngine.RectTransform.Edge)),
        _GT(typeof(UnityEngine.Canvas)),
        _GT(typeof(UnityEngine.EventSystems.EventTriggerType)),
        _GT(typeof(UnityEngine.SceneManagement.SceneManager)),
        _GT(typeof(lgu3d.LuaModelControlBase)),
        _GT(typeof(lgu3d.AppConfig)),
        _GT(typeof(lgu3d.AppPlatform)),
        _GT(typeof(lgu3d.ZipTools)),
        _GT(typeof(lgu3d.FilesTools)),
        _GT(typeof(lgu3d.LuaHelpTools)),
        _GT(typeof(lgu3d.LuaManagerModule)),
        _GT(typeof(lgu3d.SDKManagerModule)),
        _GT(typeof(lgu3d.WeChatComp)),
        _GT(typeof(lgu3d.BaiDuComp)),
        _GT(typeof(lgu3d.PhoneComp)),
        _GT(typeof(LuaInterface.LuaByteBuffer)),
        _GT(typeof(lgu3d.ProtoType)),
        _GT(typeof(lgu3d.LuaServiceModule)),
        _GT(typeof(lgu3d.MessageType)),
        _GT(typeof(lgu3d.RegularTools)),
        _GT(typeof(lgu3d.CircleImage)),
    };

  public static List<Type> dynamicList = new List<Type>()
    {
        typeof(MeshRenderer),
#if !UNITY_5_4_OR_NEWER
        typeof(ParticleEmitter),
        typeof(ParticleRenderer),
        typeof(ParticleAnimator),
#endif

        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        typeof(SkinWeights),
        typeof(RenderTexture),
        typeof(Rigidbody),
    };

  //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
  //使用方法参见例子14
  public static List<Type> outList = new List<Type>()
  {

  };

  //ngui优化，下面的类没有派生类，可以作为sealed class
  public static List<Type> sealedList = new List<Type>()
  {
    /*typeof(Transform),
    typeof(UIRoot),
    typeof(UICamera),
    typeof(UIViewport),
    typeof(UIPanel),
    typeof(UILabel),
    typeof(UIAnchor),
    typeof(UIAtlas),
    typeof(UIFont),
    typeof(UITexture),
    typeof(UISprite),
    typeof(UIGrid),
    typeof(UITable),
    typeof(UIWrapGrid),
    typeof(UIInput),
    typeof(UIScrollView),
    typeof(UIEventListener),
    typeof(UIScrollBar),
    typeof(UICenterOnChild),
    typeof(UIScrollView),        
    typeof(UIButton),
    typeof(UITextList),
    typeof(UIPlayTween),
    typeof(UIDragScrollView),
    typeof(UISpriteAnimation),
    typeof(UIWrapContent),
    typeof(TweenWidth),
    typeof(TweenAlpha),
    typeof(TweenColor),
    typeof(TweenRotation),
    typeof(TweenPosition),
    typeof(TweenScale),
    typeof(TweenHeight),
    typeof(TypewriterEffect),
    typeof(UIToggle),
    typeof(Localization),*/
  };

  public static BindType _GT(Type t)
  {
    return new BindType(t);
  }

  public static DelegateType _DT(Type t)
  {
    return new DelegateType(t);
  }


  [MenuItem("Lua/Attach Profiler", false, 151)]
  static void AttachProfiler()
  {
    if (!Application.isPlaying)
    {
      EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
      return;
    }

    LuaClient.Instance.AttachProfiler();
  }

  [MenuItem("Lua/Detach Profiler", false, 152)]
  static void DetachProfiler()
  {
    if (!Application.isPlaying)
    {
      return;
    }

    LuaClient.Instance.DetachProfiler();
  }
}
