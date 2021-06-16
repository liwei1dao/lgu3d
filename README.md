# lgu3d Unity3d开发框架
在一次次的项目研发中大家应该都发现了，即使完全不相同的游戏在初期研发中依然都要实现大量的基础功能模块.为此我将这些与业务低耦合的功能抽离出来形成了自己的代码库，这就是lgu3d的由来，之后在不断的累计中我将逐渐模块化标准化从而实现了这套u3d的快速开发框架
## lgu3d功能介绍
 lgu3d 的设计核心时采用 Module+Component 的设计模式, 一个Module下可以挂载多个Component，Module负责管理这些组件以及提供对外接口，Component实现具体的模块业务,这一设计模式 参考u3d的对象组件挂在模式也就是(ECS)模式
 1. Module lgu3d的Module其实功能非常强大的,leu3d为了使Module的绝对低耦合,不仅是代码层面上的分离设计同时我们还做到了资源上的额分离设计,模块中我们使用得到的相关资源我们可以根据自己的需求放在属于自己的模块目录中，且Module对象自带了资源加载的内部接口，且在编辑器模式下和发布模式下一样使用,同样在做模块迁移时也只需要迁移模块目录即可，并且Module除了自带资源加载接口以外还有计时器接口，协程接口,声音播放接口等等...
 2. Component lgu3d的Component比较简单，除了自身的生命周期函数意外，也就是在创建时可以通过泛型约束来确定组件只能挂载在那个Module下，以及内部MyModule字段可以自建访问自己所属Module对象,模块内组件之间通信需要通过Module实现,
lgu3d 除了基本的框架设计之外还有一大优点，框架内部集成许多软件基础服务模块。比如:VersionManagerModule(版本管理模块),ResourceModule(资源管理加载组件),ViewManagerModule(界面管理模块),ServerModule(服务端通信模块),TimerModule(计时器模块),CoroutineModule(协程模块)，EventModule(事件模块)...
3. 同时框架集成了ulua系统，重构ulua系统到LuaManagerModule(lua管理模块)下,
lgu3d内部集成模块大部分都支持c#和lua双版本,在lua模式下开发依然遵循Module+Component开发模式，LuaManagerModule模块下重新编译了lua底层库添加了对proto3的协议支持,
4. 由于lgu3d是公用外联库，所有项目是不允许修改lgu3d框架下的代码的,但是在实际项目开发中框架却未必完全适用所有项目，所以我们采用了外部模块覆盖内部模块的资源打包方式，例如 lua项目中我需要重写lua启动代码，添加自己项目的启动模块，这时需要修改LuaManagerModule模块下的main.lua代码，如果肢解修改内部代码可能会影响其他项目运行代码，这时我们只需要在框架外部自己的app模块目录集下创建一个LuaManagerModule的目录,在此目录的相对目录下新建一个main.lua脚本即可，这个lua脚本在运行时就会自动覆盖掉内部的main.lua,此方式适用于所有的资源文件
5. lgu3d 除了集成大量内部基础服务模块意外还集成了相关的工具集，比如:PackingTools(资源打包给工具),ExeclImportTools(数据导入导出工具 现在支持Exec,xml,json 自动转换成asset文件),除了界面工具意外我们还重构了模块管理器属性面板，在模块管理器属性面板下我们能看到所有的模块以及组件的运行状态，以及配合Attribute元数据标记可以查看和修改运行中模块和组件上的变量数据参数
6. lgu3d 更是继承了许多扩展对象或接口，比如unity3d 的UI 元素，Sharder...
## lgu3d目录介绍
一套框架好不好先看目录结构,目录结构的好坏直接关系到一个项目的长期维护工作，好的框架其目录结构即使不需要文档也能大致看出功能分布，以及基本设计构想
1. lgu3d/Config 主要是配置项目的一些全局属性 例如:打包资源文件类型,日志输出路径，临时资源下载目录...
2. lgu3d/Core lgu3d核心框架设计代码，里面包含了基础模块和组件的设计代码，以及通用组件集
3. lgu3d/Editor lgu3d 工具和属性面板相关代码
4. lgu3d/Extend 各种扩展方法，扩展对象集
5. lgu3d/Modules 内部模块集
6. lgu3d/Plugins 一些必要的dll文件
7. lgu3d/ThirdPlugins 第三方插件的一个集合
## lgu3d的安装
lgu3d并不是一个完整的unity3d项目，只是u3d项目的一部分,我们使用lgu3d有多种方式可以实现
1. 我们可以将代码下载并拖入到u3d项目中 ‘Assets/lgu3d/’ 目录下,
2. 采用git submodule 的方式克隆到项目的‘Assets/lgu3d/’目录下，这种方式便于后期框架更新之后能快速同步,比较适合公司多项目开发同时(推荐)
## lgu3d的使用
lgu3d 的使用非常简单，安装完框架之后 我们在 Assets 目录下创建app目录,同时在app目录下创建main.cs 脚本,叫脚本挂在在场景中的一个空对象上即可,挂在上去之后对象属性面板会显示日志以及资源模式项目选项,在开发模式下我们悬着debug资源模式即可,(如启动报错可能是应为资源打包面板没有刷新，可以参考[配置资源编译工具](#配置资源编译工具))
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lgu3d;

public class main : Main
{
  protected override void StartApp()
  {
    //先启动资源管理模块
    Manager_ManagerModel.Instance.StartModule<ResourceModule>((module) =>
    {
      //更新需要启动内部模块
      Manager_ManagerModel.Instance.StartModule<TimerModule>();
      Manager_ManagerModel.Instance.StartModule<CoroutineModule>();
      Manager_ManagerModel.Instance.StartModule<SoundModule>();
      Manager_ManagerModel.Instance.StartModule<ViewManagerModule>(null, new Vector2(720, 1280));
      Manager_ManagerModel.Instance.StartModule<SceneModule>();
      //启动自己的业务模块
      Manager_ManagerModel.Instance.StartModule<自己的业务模块>();
    });
  }
}
```
## 创建自己的业务模块
lgu3d 创建自己的模块 需要注意 模块目录名称必须以Module结尾，这样子资源打包工具才能搜索到你
1. Assets 目录下创建app目录,在app目录下创建 一个自己的模块目录 例如:DemoModule
2. 在DemoModule目录下创建Script,Prefab,Image目录,在Script目录下创建DemoModule.cs 和 DemoMainViewComp.cs
3. 创建好MainView.prefab界面对象放在Prefab目录下，图片资源放在Image下即可，在main.cs下启动我们的模块，接下载在main.cs,去资源编译面板刷新下[配置资源编译工具](#配置资源编译工具)，这是运行项目应该就可以看到我们的页面显示在场景中了
   
    DemoModule.cs
    ```
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using lgu3d;

    public class DemoModule : ManagerContorBase<DemoModule>
    {
      public DemoMainViewComp MainViewComp;
      public override void Load(params object[] _Agr)
      {
        ResourceComp = AddComp<Module_ResourceComp>();
        MainViewComp = AddComp<DemoMainViewComp>();
        base.Load(_Agr);
      }
    }
    ```
    DemoMainViewComp.cs
    ```
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using UnityEngine;
    using lgu3d;

    public class DemoMainViewComp : Model_BaseViewComp<DemoModule>
    {
      public override void Load(ModelBase module, params object[] agr)
      {
        base.Load(module, "MainView");  //界面MainView.prefab
        UIGameobject.OnAddClick("demobutt", demobuttclick);
        LoadEnd();
      }
    }
    private void demobuttclick()
    {
      Debug.Log("demobuttclick");
    }
    ```
## 配置资源编译工具
lgu3d 内置的资源编译工具是框架核心功能之一,它提供能了框架在编译环境下和发布环境下模块加载资源的配置信息，没有这些配置信息模块在启动时将无法找到自己的资源文件
1. 打开资源编辑工具unity3d 菜单栏-> Windos -> lgu3d
2. 选中 打包工具，点击 + 按钮,添加模块目录,先添加lgu3d/Modules路径进去，这时右侧面板会检索处此目录下的模块列表，之后添加app目录，这样就可以业务模块也检索到资源列表下,这时基本上在编辑器模式下就可以运行看效果了，（注意:一定姚先添加框架内部模块集再添加app模块集，这关系到之前介绍到的外部资源可覆盖内部资源的功能）
3. 当有项目中创建了新的资源文件时，需要点击刷新按钮 这样新的资源信息就会被框架收录进去，不然运行编辑器是会提示资源文件找不到的错误


## Core基础框架设计介绍
lgu3d 的基础框架设计相对比较简单,模块和组件都是4和生命周期函数,模块和组件大致都有load,start,close等声明周期函数,load主要是来初始化自身,start函数就可以去访问外部接口，这样确保模块和组件在在使用外部接口是 其他模块组件都是已经初始化的状态,

1. 基础模块组件,模块除了基础的声明周期函数意外 还额外添加了一些常用的接口，当然这些常用的接口使用需要模块初始化相应的组件，而组件也是除了基础的周期函数意外还有泛型模块字段可以很轻松的访问当自己的挂在模块对象
  ```
    public interface IModule
    {
        void Load(params object[] agr);                                                    //模块初始化
        bool LoadEnd();                                                                    //加载完毕
        void Start(params object[] agr);                                                   //模块启动
        void Close();                                                                      //模块关闭
        void ShowInspector();                                                              //模块属性面板             
    }

    public abstract class ModelBase : IModule {
        public string ModuleName;                                                   //模块名称
        public ModelBaseState State = ModelBaseState.Close;                         //模块状态
        protected List<ModelCompBase> MyComps = new List<ModelCompBase>();          //组件列表
        protected Module_TimerComp TimerComp;                                       //计时器组件 （需要则初始化）
        protected Module_SoundComp SoundComp;                                       //声音组件 （需要则初始化）
        protected Module_CoroutineComp CoroutineComp;                               //协程组件（需要则初始化）
        protected Module_ResourceComp ResourceComp;                                 //资源管理组件（需要则初始化）

        public ModelBase()
        {
            State = ModelBaseState.Close;
        }

        public virtual void Load(params object[] agr)
        {
            State = ModelBaseState.Loading;
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Load(this, agr);
            }
            LoadEnd();
        }
        public virtual bool LoadEnd()
        {
            if (State >= ModelBaseState.LoadEnd) //模块已经加载成功了
                return false;
            for (int i = 0; i < MyComps.Count; i++)
            {
                if (MyComps[i].State != ModelCompBaseState.LoadEnd)
                {
                    return false;
                }
            }
            if (State < ModelBaseState.LoadEnd)
            {
                State = ModelBaseState.LoadEnd;
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual void Start(params object[] agr)
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Start(agr);
            }
            State = ModelBaseState.Start;
        }
        public virtual void Close()
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Close();
            }
            MyComps.Clear();
            State = ModelBaseState.Close;
        }
        //
        .....
        //
      }



    public interface IModelCompBase
    {
        void Load(ModelBase module, params object[] agr);                                  //组件初始化
        void Start(params object[] agr);                                                   //组件启动
        void Close();                                                                      //组件关闭
        void ShowInspector();                                                              //自定义监控面板
    }

    /// <summary>
    /// 模块组件基类
    /// </summary>
    public abstract class ModelCompBase: IModelCompBase
    {
        protected ModelBase MyModule;                                                           //挂在模块
        public ModelCompBaseState State = ModelCompBaseState.Close;                             //组件状态
        public virtual void Load(ModelBase module, params object[] agr)
        {
            MyModule = module;
            State = ModelCompBaseState.Loading;
        }
        protected virtual void LoadEnd()
        {
            State = ModelCompBaseState.LoadEnd;
            MyModule.LoadEnd();
        }
        public virtual void Start(params object[] agr)
        {
            State = ModelCompBaseState.Start;
        }
        public virtual void Close()
        {
            MyModule = null;
            State = ModelCompBaseState.Close;
        }
    }

    public abstract class ModelCompBase<C> : ModelCompBase where C: ModelBase, new()
    {
        protected new C MyModule;

        public override void Load(ModelBase _ModelContorl, params object[] agr)
        {
            MyModule = _ModelContorl as C;
            base.Load(_ModelContorl, agr);
        }                                  
    }
  ```
## Core基础框架通用组件介绍
lgu3d 除了内置基础模块模块之外还有大量的基础通用组件，以方便业务模块的开发

1. Module_BaseSceneComp 此组件为模块场景组件，配合内置ServerModule使用，通过此组件可以优雅的跳转自己模块的场景,使用中重构场景加载函数和场景卸载函数,注意 在加载和写在中需要设置进度值不然场景进度模块无法正常结束的
```
  using System.Collections;
  using System.Collections.Generic;
  using lgu3d;
  using UnityEngine;

  public class DemoSceneComp : Module_BaseSceneComp<DemoModule>
  {
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
      SceneModule.Instance.ChangeScene(this);
    }

    public override string GetSceneName()
    {
      return "demo";
    }

    public override IEnumerator LoadScene()
    {
      yield return 1;
      Process = 1;
      LoadEnd();
    }

    public override IEnumerator UnloadScene()
    {
      yield return 1;
      Process = 1;
    }
  }
```
2. Module_BaseViewComp 此组件为模块UI组件,配合ViewManagerModules使用,通过此组件我们可以优雅的自动加载UI界面以及设置UI显示优先级，
```
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using UnityEngine;
    using lgu3d;

    public class DemoMainViewComp : Model_BaseViewComp<DemoModule>
    {
      public override void Load(ModelBase module, params object[] agr)
      {
        base.Load(module, "MainView");  //界面MainView.prefab
        //UIGameobject 就是我们UI的根对象，可以通过这个对象找到UI界面中的所有元素
        UIGameobject.OnAddClick("demobutt", demobuttclick);
        LoadEnd();
      }
    }
    private void demobuttclick()
    {
      Debug.Log("demobuttclick");
    }
```
3. Module_CoroutineComp 此组件为协程组件,配合CoroutineModule使用，可装配到模块身上使其协程接口可以使用
```
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using lgu3d;

  public class DemoModule : ManagerContorBase<DemoModule>
  {
    public override void Load(params object[] _Agr)
    {
      CoroutineComp = AddComp<Module_CoroutineComp>();
      base.Load(_Agr);
    }
  }
```
4. Module_DownloadComp 此组件主要用于下载资源，详情可参考VersionManagerModule
5. Module_MusicPlayerComp 已过时 使用 Module_SoundComp 代替
6. Module_ResourceComp 资源管理组件,配合ResourceModule使用,可装配到模块身上使其资源加载接口可以使用
```
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using lgu3d;

  public class DemoModule : ManagerContorBase<DemoModule>
  {
    public override void Load(params object[] _Agr)
    {
      ResourceComp = AddComp<Module_ResourceComp>();
      base.Load(_Agr);
    }
  }
```
7. Module_SoundComp 声音播放组件，配合SoundModule使用，可装配到模块身上使其音频播放接口可以使用
```
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using lgu3d;

  public class DemoModule : ManagerContorBase<DemoModule>
  {
    public override void Load(params object[] _Agr)
    {
      SoundComp = AddComp<Module_SoundComp>();
      base.Load(_Agr);
    }
  }
```
8. Module_TimerComp 计时器组件，配合TimerModule使用，可装配到模块身上使其计时器接口可以使用
```
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using lgu3d;

  public class DemoModule : ManagerContorBase<DemoModule>
  {
    public override void Load(params object[] _Agr)
    {
      TimerComp = AddComp<Module_TimerComp>();
      base.Load(_Agr);
    }
  }
```
