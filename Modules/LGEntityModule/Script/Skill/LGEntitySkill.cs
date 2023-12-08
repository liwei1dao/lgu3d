using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 技能对象
    /// </summary>
    public abstract class LGEntitySkill<C> : LGEntityCompBase, ILGSkill where C : SkillBaseConfig
    {
        protected C Config;
        public LGSkillState State { get; set; }
        protected LGSkillCD CD;
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
            State = LGSkillState.NoRelease;
            CD = new LGSkillCD(this);
        }

        public bool Condition()
        {
            return true;
        }

        public void Release()
        {

        }

        public virtual void Release_Entity(ILGEntity target)
        {

        }

        public virtual void Release_Direction(Vector3 Direction)
        {

        }

        public virtual void Release_Point(Vector3 Point)
        {

        }

        public virtual void Release_DynamicPoints(Transform Point)
        {

        }

        public abstract ILGBullet CreateBullet();

        public virtual void ReleaseEnd()
        {

        }



        public virtual void CdEnd()
        {
            State = LGSkillState.NoRelease;
        }

        public override void LGUpdate(float time)
        {
            if (State == LGSkillState.InCd)
            {
                CD.Update(time);
            }
        }

    }
}