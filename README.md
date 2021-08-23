# lgu3d
Unity3d 开发框架
# lgu3d 开发框架简介
一直想为这套框架取个高大上的名字，不过可惜这方面没啥天赋最后还是将就着先用这个名字吧，哈哈哈！
fw 这套框架最开始的初衷只是为了做个人知识积累的一个代码库来用的，至今它也是作为我的代码库来用的，只不过为了让里面的功能代码能快速被提取出来使用最后演变成了一套底层开发框架
- 框架结构
  * Config						框架配置脚本			
  * Core						框架基础核心设计模板
  * Editor						集成工具集
  * Extend					扩展对象集
  * Modules					模块仓库
  * ThirdPlugins			第三方插件库
# lgu3d 的导入
项目采用git 管理方式的话可以使用 git submodule 的方式导入项目到 “Asset/lgu3d” 工程目录下
[视屏教程-国内](https://v.qq.com/x/page/a3046v47msy.html)   
[视屏教程-YouTube](https://www.youtube.com/watch?v=G1NvPQO69XM)
- 框架地址
	* github 		https://github.com/liwei1dao/lgu3d.git
	* gitee			https://gitee.com/liwei1dao/lgu3d.git
# lgu3d 初始环境配置
项目成功导入之后我们需要做一件非常重要的事情，那就是配置初始环境，配置初始环境其实也比较简单就是需要使用框架自带打包工具集将框架的模块仓库以及我们项目之定义的模块全部注册到打包配置里面去，这样框架在运行对应的模块时才能找到对应的资源目录，
框架工具集：Window => lgu3d => 打包工具
添加 lgu3d /Modules	 目录进入左侧列表，右侧将会扫描出所有模块目录以及需要打包编译的文件列表
# lgu3d 模块与组件
lgu3d 的底层核心就是module,comp,module是comp的容器,管理各类功能以及业务组件以及协调各组件之间的接口调用
模块基础接口：
```
    //模块基础状态
    public enum ModelBaseState
    {
        Close = -1,          //关闭状态
        Loading = 1,        //加载中状态
        LoadEnd = 2,        //加载完成状态
        Start = 3,          //启动状态
    }
   //基础模块
    public interface IModule
    {
        void Load(params object[] agr);   //模块加载
        bool LoadEnd();                              //加载完毕
        void Start(params object[] agr);   //模块启动
        void Close();                                   //模块关闭
    }
    //扩展模块----------------------------------------------------------------------------
    //更新模块
    public interface IUpdataMode: IModule
    {
        void Update(float time);
    }
	//物理更新模块
    public interface IFixedUpdateMode : IModule
    {
        void FixedUpdate();
    }
	//场景管理模块
    public interface ISceneMode : IModule
    {
        IEnumerator LoadScene();
    }
模块基础抽象类	    
	public abstract class ModelBase : IModule{
        public string ModuleName;                                                   //模块名称
        public ModelBaseState State = ModelBaseState.Close;                         //模块状态
        protected List<ModelCompBase> MyComps = new List<ModelCompBase>();          //组件列表
        protected Module_TimerComp TimerComp;                                       //计时器组件 （需要则初始化）
        protected Module_CoroutineComp CoroutineComp;                               //协程组件（需要则初始化）
        protected Module_ResourceComp ResourceComp;                                 //资源管理组件（需要则初始化）
...
    //添加组件
     protected virtual CP AddComp<CP>(params object[] _Agr) where CP : ModelCompBase, new()
        {
            CP Comp = new CP();
            MyComps.Add(Comp);
            if (State > ModelBaseState.Close)
                Comp.Load(this, _Agr);
            if (State == ModelBaseState.Start)
                Comp.Start(this, _Agr);
            return Comp;
        }
    	//添加组件
        protected virtual ModelCompBase AddComp(ModelCompBase Comp, params object[] _Agr)
        {
            MyComps.Add(Comp);
            if (State > ModelBaseState.Close)
                Comp.Load(this, _Agr);
            if (State == ModelBaseState.Start)
                Comp.Start(this, _Agr);
            return Comp;
        }

        //移除组件
        protected virtual void RemoveComp(ModelCompBase Comp)
        {
            MyComps.Remove(Comp);
            Comp.Close();
        }
...
	}
```
组件

```
    //组件状态
    public enum ModelCompBaseState
    {
        Close,          //关闭状态
        Loading,        //加载中状态
        LoadEnd,        //加载完成状态
        Start,          //启动状态
    }
    //组件接口对象
    public interface IModelCompBase
    {
        void Load(ModelBase module, params object[] agr);                                   //模块初始化
        void Start(params object[] agr);                                                   				//模块启动
        void Close();																								//关闭组件	
        void ShowInspector();                                                             				 //自定义监控面板
    }
    //组件抽象基类
    public abstract class ModelCompBase: IModelCompBase{
        protected ModelBase MyModule;																			//模块对象
        public ModelCompBaseState State = ModelCompBaseState.Close;						//状态
    ...
    //组件load 完成之后需要主动调用此接口 否则module 状态不会进入start状态的
     	 protected virtual void LoadEnd()
        {
            State = ModelCompBaseState.LoadEnd;
            MyModule.LoadEnd();
        }
       ...
	}
	泛型模块组件基类 方便组件与模块资源的直接应用
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
# lgu3d 第一个app模块
创建一个自己的模块集合目录 如:appmodules,在appmodules 中创建 以Module 结尾的模块目录如:demoModule,模块默认目录结构
- demoModule
  * Script						c# 脚本目录
  * Lua						    Lua 脚本目录
  * Editor						模块编辑工具目录
  * Prefab						预制物集合默认目录
  * Image						图片资源集合默认目录
  * Scene						场景关卡集合默认目录
  * Sound						音频资源默认目录
- 以上目录之外还可以自定义自己的资源目录，资源模块的使用需要与资源打包工具的打包规则相应使用才可
创建模块CarDemoModule .cs 和CarDemoMianViewComp.cs,,"MianViewComp" 为预制界面的名称,放在Prefab下， **ok 之后 将appmodules 目录添加到资源编译集合中去**
```
//demo 模块
using lgu3d;
public class CarDemoModule : ManagerContorBase<CarDemoModule>
{

    private CarDemoMianViewComp MianViewComp;
    public override void Load(params object[] agr)
    {
        ResourceComp = AddComp<Module_ResourceComp>();
        MianViewComp = AddComp<CarDemoMianViewComp>();
        base.Load(agr);
    }
}
```

```
using UnityEngine;
using lgu3d;
using UnityEngine.UI;

public class CarDemoMianViewComp : Model_BaseViewComp<CarDemoModule>
{

    private Button demobutt;
    public override void Load(ModelBase module, params object[] agr)
    {
        base.Load(module, "MianViewComp");
        demobutt = UIGameobject.OnSubmit<Button>("demobutt");
        demobutt.onClick.AddListener(demobuttClick);
    }

    private void demobuttClick() {
        Debug.Log("liwei1dao");
    }
 }
```

在appmodules 下创建appmian.cs 挂载在main场景中的空对象mian上
```
using lgu3d;

public class AppMain : Main
{
    protected override void StartApp()
    {
        Manager_ManagerModel.Instance.StartModule<ResourceModule>((mpdule) =>
        {
            Manager_ManagerModel.Instance.StartModule<ViewManagerModule>(null,new Vector2(1920f,1080f));
            Manager_ManagerModel.Instance.StartModule<CarDemoModule>();
        });
    }
}
```
挂载在mian对象上之后可以看到
![在这里插入图片描述](https://images.gitee.com/uploads/images/2020/0115/174240_dbd3c95d_911904.png)		
* 是否显示日志 					选中之后可以将会在界面中显示日志面板，同时本地会创建一个日志文件
* 是否启动版本检测				方便临时测试 跳过版本检查直接进入游戏
* Editor								App资源加载方式有debug和release模式, release 模式 资源采用assetbundle
启动时由于没有打包资源请采用debug模式启动演示
# lgu3d 模块资源管理和资源打包工具以及版本管理模块
一套可以商用的开发框架必然少不了的一套功能就是这个了，有了这个 app 才能拥有一定的自我更新能录，即使采用非lua模式开发，但是我们依然可以更新大部分游戏素材，
- App更新三大件
  * Window => lgu3d => 打包工具            //资源打包工具  输出更新资源文件
  * ResourceModule								   	//资源管理模块，资源加载以及资源释放
  * VersionManagerModule							//版本管理检查模块 管理资源更新和下载
 
- 资源打包界面 可以根据自己的需求输出对应目录结构的资源文件
![在这里插入图片描述](https://images.gitee.com/uploads/images/2020/0115/174240_a6c016e5_911904.png)
- 资源管理模块   **我们框架通用组件中有一个 Module_ResourceComp 每个模块添加这个组件之后可以通过上面的接口直接加载自己模块下的任何资源文件**
```
//资源管理模块
public class ResourceModule : ManagerContorBase<ResourceModule>{
       /// <summary>
       /// 加载资源文件
       /// </summary>
       /// <typeparam name="T">加载资源类型</typeparam>
       /// <param name="bundleOrPath">资源相对路径</param>
       /// <param name="assetName">资源名称</param>
       /// <param name="IsSave">是否保存</param>
       /// <returns></returns>
	public T LoadAsset<T>(string ModelName, string BundlePath, string AssetName){
...}
	//加载lua文件
     public byte[] LoadLuaFile(string ModelName,string BundlePath, string AssetName){
     .....
     }
    //加载proto 文件
    public byte[] LoadProtoFile(string ModelName, string BundlePath, string AssetName){
   	...
    }
}
```
- VersionManagerModule  app启动子模块将可以检查到远端资源服务器与本地资源的差异，并开启更新下载流程 子模块有c#和lua 两个版本，可以根据自己的需求使用

```
    public class VersionManagerModule : ManagerContorBase<VersionManagerModule>
    {
    	...
        /// <summary>
        /// 开始App版本校验
        /// </summary>
    	 public void StartAppVersionCheck(string resUrl,Action Complete){
    	 }
    	/// <summary>
        /// 开始Module版本校验
        /// </summary> 
    	 public void StartModuleVersionCheck(string ModuleName, Action<bool> Complete){
    	 }
    	...
    }
```

# lgu3d 模块管理以及数据监控
模块通过Manager_ManagerModel 启动之后都会在ManagerModel 属性面板显示器器模块的一下相关信息
用户 Module以及Comp 脚本在运行中查看内存
![在这里插入图片描述](https://images.gitee.com/uploads/images/2020/0115/174240_f220bdee_911904.png)
- 模块与组件如果有需要实时监控内存变化的字段可以采用 u3dfw_Serialize 的元属性来标记
可以看到上边两个demo 字段都已经显示在了模块监控面板中

```
    [u3dfw_Serialize]
    private string demostr1 = "hello";

    [u3dfw_SerializeName("我是 hello2")]
    private string demostr2 = "hello";
```
![在这里插入图片描述](https://images.gitee.com/uploads/images/2020/0115/174240_25434c22_911904.png)
- 监控面板除了使用u3dfw_Serialize  还可以使用模块以及组件重构接口**ShowInspector**

```
    public override void ShowInspector() {
        GUILayout.BeginVertical();
        GUILayout.Label("liwei1dao");
        if (GUILayout.Button("按钮")) {
            Debug.Log("测试自定义属性面板");
        }
        GUILayout.EndVertical();
    }
```
![在这里插入图片描述](https://images.gitee.com/uploads/images/2020/0115/174240_ddda11a8_911904.png)
# lgu3d lua管理模块以及自定义lua模块
- lgu3d框架在后期扩展了lua开发模式,创建了LuaManagerModule, 作为框架启动lua开发的引擎模块
LuaManagerModule 在被启动之后会去执行自身模块下lua 目录下的 mian.lua 文件，当mian脚本被执行时就代表我们的代码运行进入到了lua层面上了，

- 为了规范以及保持我们module开发模式的设计方式，我在网上找到了一个lua类 实现的脚本class.lua 加上自己的稍微改进，以此为基础我重定义了 
 	* BaseModel.lua
 	* BaseModelComp.lua
 	* BaseModelViewComp.lua
 	* LuaGameObject.lua
    * LuaUIGameObject.lua
   	...

- 自定义自己的lua 模块
Main.lua 标准模板 CarDemoModule 改为自己的模块名称,当自定义模块创建写好之后可以在LuaManagerModule的mian.lua通过一下代码启动
**LuaManagerModule.Instance:StartLuaModel("CarDemoModule",nil,nil)**
```
module ("CarDemoModule", package.seeall);

ModelControl = Class.new(require "CarDemoModule.CarDemoModule")
function New(_csobj)
	ModelControl:New(_csobj)
end
function Instance()
	return ModelControl
end
function Load(...)
	ModelControl:Load(...)
end
function LoadEnd()
	return ModelControl:GetEnd()
end
function Start(...)
	ModelControl:Start(...)
end
function Close()
	ModelControl:Close()
end
```
CarDemoModule.lua   创建自己的模块脚本 更具自己的模块名称修改即可

```
-- Class.define 第一个参数为定了的类名 第二个参数表示继承与那个类
local CarDemoModule = Class.define("CarDemoModule",BaseModel);

function CarDemoModule:New(_csobj)
    self:super(CarDemoModule,"New", _csobj);
    --添加主界面组件 require "CarDemoModule.MainViewComp" 为脚本执行路径 "MainViewComp" 为界面预制对象名称 默认UI是放在Prefab 目录下的
    self.MainViewComp = self:AddComp("MainViewComp",require "CarDemoModule.MainViewComp","MainViewComp")
end

return CarDemoModule;
```
MainViewComp.lua 模块自己的一个ui组件

```
local CarDemoMainViewComp = Class.define("CarDemoMainViewComp",BaseModelViewComp)

function CarDemoMainViewComp:ctor(...)
    self:super(CarDemoMainViewComp,"ctor",...)
end
return CarDemoMainViewComp
```
# lgu3d 扩展集
- Extend 框架扩展集合
	* Attribute 	框架元属性扩展 用于模块属性检查面板的显示
	* Data 			各类数据容器（红黑树...）,以及数据结构转换和各类数据处理接口
	* Files			处理有关文件以及文件夹的操作接口
	* Json			josn 封装
	* Shader		自定义shader资源库
	* String  		有关字符串的各类操作接口
	* Tween 		插件扩展集
	* UI       		自定义UI元素库
	* Unity3d		unity3d 有关的扩展集合
	* Zip				zip 工具库