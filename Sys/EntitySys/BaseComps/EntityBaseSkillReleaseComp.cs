using System;
using System.Collections.Generic;
using UnityEngine;
namespace lgu3d
{

    /// <summary>
    /// 技能释放接口
    /// </summary>
    public interface IEntityBaseSkillReleaseComp
    {
        bool Release(string skillName, params object[] agrs);
        void ReleaseEnd(string skillName);
    }

    public enum SkillReleaseCompState
    {
        Idle,
        InRelease,
    }

    /// <summary>
    /// 实体技能释放组件
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class EntityBaseSkillReleaseComp<E> : EntityCompBase<E>, IEntityBaseSkillReleaseComp where E : EntityBase
    {
        public SkillReleaseCompState ReleaseState;
        protected Dictionary<string, ISkillBase> Skills;

        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
            ReleaseState = SkillReleaseCompState.Idle;
            Skills = new Dictionary<string, ISkillBase>();
        }

        public virtual void AddSkill(string skillName, ISkillBase skill)
        {
            Skills[skillName] = skill;
        }
        public virtual void RemoveSkill(string skillName, ISkillBase skill)
        {
            Skills.Remove(skillName);
        }
        public virtual bool Release(string skillName, params object[] agrs)
        {
            if (ReleaseState == SkillReleaseCompState.InRelease) return false;
            ReleaseState = SkillReleaseCompState.InRelease;
            Skills[skillName].Release(agrs);
            return true;
        }

        protected virtual void Updata()
        {
            foreach (var item in Skills)
            {
                item.Value.Update(Time.deltaTime);
            }
        }

        public virtual void ReleaseEnd(string skillName)
        {
            foreach (var item in Skills)
            {
                if (item.Value.State == SkillState.InRelease)
                {
                    return;
                }
            }
            ReleaseState = SkillReleaseCompState.Idle;
        }
    }


    /// <summary>
    /// 实体技能释放组件
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class MonoEntityBaseSkillReleaseComp<E> : MonoEntityCompBase<E> where E : MonoEntityBase
    {
        public SkillReleaseCompState ReleaseState;
        protected Dictionary<string, ISkillBase> Skills;

        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            Skills = new Dictionary<string, ISkillBase>();
            ReleaseState = SkillReleaseCompState.Idle;
            base.LGInit(entity, agrs);
        }
        public virtual void AddSkill(string skillName, ISkillBase skill)
        {
            Skills[skillName] = skill;
        }
        public virtual void RemoveSkill(string skillName, ISkillBase skill)
        {
            Skills.Remove(skillName);
        }
        public virtual bool Release(string skillName, params object[] agrs)
        {
            if (ReleaseState == SkillReleaseCompState.InRelease) return false;
            ReleaseState = SkillReleaseCompState.InRelease;
            Skills[skillName].Release(agrs);
            return true;
        }

        private void Updata()
        {
            foreach (var item in Skills)
            {
                item.Value.Update(Time.deltaTime);
            }
        }

        public virtual void ReleaseEnd(string skillName)
        {
            foreach (var item in Skills)
            {
                if (item.Value.State == SkillState.InRelease)
                {
                    return;
                }
            }
            ReleaseState = SkillReleaseCompState.Idle;
        }
    }
}
