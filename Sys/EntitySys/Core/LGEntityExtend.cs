using System;
using System.Linq;
using System.Collections.Generic;

namespace LG.EntitySys
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class EnableUpdateAttribute : Attribute
    {
        public EnableUpdateAttribute()
        {
        }
    }

    public abstract partial class LGEntity
    {
        public static MasterEntity Master => MasterEntity.Instance;
        public static bool EnableLog { get; set; } = false;

        public static long BaseRevertTicks { get; set; }

        public static long NewInstanceId()
        {
            if (BaseRevertTicks == 0)
            {
                var now = DateTime.UtcNow.Ticks;
                var str = now.ToString().Reverse();
                BaseRevertTicks = long.Parse(string.Concat(str));
            }
            BaseRevertTicks++;
            return BaseRevertTicks;
        }

        public static LGEntity NewEntity(Type entityType, long id = 0)
        {
            var entity = Activator.CreateInstance(entityType) as LGEntity;
            entity.InstanceId = NewInstanceId();
            if (id == 0) entity.Id = entity.InstanceId;
            else entity.Id = id;
            if (!Master.Entities.ContainsKey(entityType))
            {
                Master.Entities.Add(entityType, new List<LGEntity>());
            }
            Master.Entities[entityType].Add(entity);

            return entity;
        }
        private static void SetupEntity(LGEntity entity, LGEntity parent)
        {
            parent.SetChild(entity);
            entity.LGInit();
            entity.LGStart();
        }
        private static void SetupEntity(LGEntity entity, LGEntity parent, object initData)
        {
            parent.SetChild(entity);
            entity.LGInit(initData);
            entity.LGStart();
        }

        public static void Destroy(LGEntity entity)
        {
            try
            {
                entity.OnDestroy();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            entity.Dispose();
        }
    }
}