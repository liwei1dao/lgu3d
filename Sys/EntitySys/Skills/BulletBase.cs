using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{

    public abstract class BulletBase<E> : EntityBase<E>, IBullet where E : BulletBase<E>
    {
        public ISkillBase Skill;
        public Dictionary<string, object> Meta;

        public Dictionary<string, object> GetMeta()
        {
            return Meta;
        }

        public virtual void LGInit(IEntityBase entity, ISkillBase skill)
        {
            base.LGInit(entity);
            Skill = skill;
            Meta = new Dictionary<string, object>();
        }
        public virtual void Launch(IEntityBase target, Dictionary<string, object> meta)
        {
            Meta = meta;
        }
        public virtual void Launch(IEntityBase target, Transform targetTransform, Dictionary<string, object> meta)
        {
            Meta = meta;
        }
    }

    public abstract class MonoBulletBase<E> : MonoEntityBase<E>, IBullet where E : MonoBulletBase<E>
    {
        public ISkillBase Skill;
        public Dictionary<string, object> Meta;
        public Dictionary<string, object> GetMeta()
        {
            return Meta;
        }
        public virtual void LGInit(IEntityBase entity, ISkillBase skill)
        {
            base.LGInit(entity);
            Skill = skill;
        }

        public virtual void Launch(IEntityBase target, Dictionary<string, object> meta)
        {
            Meta = meta;
        }
        public virtual void Launch(IEntityBase target, Transform targetTransform, Dictionary<string, object> meta)
        {
            Meta = meta;
        }
    }
    public abstract class MonoBulletBase<E, D> : MonoEntityBase<E, D>, IBullet where E : MonoEntityBase<E, D> where D : class
    {
        public ISkillBase Skill;
        public Dictionary<string, object> Meta;
        public Dictionary<string, object> GetMeta()
        {
            return Meta;
        }
        public virtual void LGInit(IEntityBase entity, ISkillBase skill)
        {
            base.LGInit(entity);
            Skill = skill;
        }

        public virtual void Launch(IEntityBase target, Dictionary<string, object> meta)
        {
            Meta = meta;
        }
        public virtual void Launch(IEntityBase target, Transform targetTransform, Dictionary<string, object> meta)
        {
            Meta = meta;
        }
    }
}
