using System;

/*
 作者 : liwei1dao
 文件 : ModelContorlBase.cs
 描述 : 框架模块组件基类
 修改 : 模块组件
*/
namespace lgu3d
{
  public enum ModelCompBaseState
  {
    Close,          //关闭状态
    Loading,        //加载中状态
    LoadEnd,        //加载完成状态
    Start,          //启动状态
  }

  public interface IModelCompBase
  {
    void Load(ModuleBase module, params object[] agr);                                   //模块初始化
    void Start(params object[] agr);                                                   //模块启动
    void Close();
    void ShowInspector();                                                              //自定义监控面板
  }


  /// <summary>
  /// 模块组件基类
  /// </summary>
  public abstract class ModelCompBase : IModelCompBase
  {
    protected ModuleBase MyModule;                                                           //挂在模块
    public ModelCompBaseState State = ModelCompBaseState.Close;                             //组件状态
    public virtual void Load(ModuleBase module, params object[] agr)
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

    /// <summary>
    /// Inspector 属性界面
    /// </summary>
    public virtual void ShowInspector()
    {

    }
  }

  public abstract class ModelCompBase<C> : ModelCompBase where C : ModuleBase, new()
  {
    protected new C MyModule;

    public override void Load(ModuleBase module, params object[] agrs)
    {
      MyModule = module as C;
      base.Load(module, agrs);
    }
  }
}
