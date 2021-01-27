using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 子弹对象池
    /// </summary>
    public class SkillModuleBullerPoolsComp: ModelCompBase<SkillModule>
    {
        private List<BulletBase> BulletPools;

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            BulletPools = new List<BulletBase>();
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        public void Update(float time)
        {
            for (int i = 0; i < BulletPools.Count; i++)
            {
                BulletPools[i].Update(time);
            }
        }

        public void Add(BulletBase Bullet)
        {
            BulletPools.Add(Bullet);
        }

        public void Remove(BulletBase Bullet)
        {
            if (BulletPools.Contains(Bullet))
            {
                BulletPools.Remove(Bullet);
            }
        }
    }
}
