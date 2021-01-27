using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏对象模块
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{
    public interface IActorBase
    {
        void Load(ActorDataBase _Config);
        void Init();
        CP AddComp<CP>(params object[] _Agr) where CP : ActorCompBase, new();
        void RemoveComp(ActorCompBase Comp);
        void Destroy();
    }

    public abstract class ActorBase : MonoBehaviour,IActorBase
    {
        [System.NonSerialized]
        public ActorDataBase Config;
        protected List<ActorCompBase> MyComps = new List<ActorCompBase>();

        #region 框架函数
        public virtual void Load(ActorDataBase _Config)
        {
            Config = _Config;
        }

        public virtual void Init()
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Init();
            }
        }

        protected void Update()
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Updata(Time.deltaTime);
            }
        }
        public virtual CP AddComp<CP>(params object[] _Agr) where CP : ActorCompBase, new()
        {
            CP Comp = new CP();
            Comp.Load(this, _Agr);
            MyComps.Add(Comp);
            return Comp;
        }
        public virtual void RemoveComp(ActorCompBase Comp)
        {
            MyComps.Remove(Comp);
            Comp.Destroy();
        }
        public void Destroy()
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Destroy();
            }
            GameObject.Destroy(this);
        }
        #endregion

        #region 基础组件接口
        public ActorBaseAttributeComp AttributeComp;                //属性组件
        public AtcorBaseSkillReleaseComp SkillReleaseComp;          //技能释放组件
        public AtcorBaseSkillAcceptComp SkilllAcceptComp;           //技能承受组件
        #endregion
    }
}
