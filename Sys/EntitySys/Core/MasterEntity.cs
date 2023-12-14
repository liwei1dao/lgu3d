using System;
using System.Collections.Generic;
using lgu3d;

namespace LG.EntitySys
{
    public sealed class MasterEntity : LGEntity
    {
        public ModuleBase GameModule { get; set; }
        public Dictionary<Type, List<LGEntity>> Entities { get; private set; } = new Dictionary<Type, List<LGEntity>>();
        public List<LGEntityComp> AllComponents { get; private set; } = new List<LGEntityComp>();
        public List<UpdateComponent> UpdateComponents { get; private set; } = new List<UpdateComponent>();
        public static MasterEntity Instance { get; private set; }
        public static MasterEntity Create()
        {
            if (Instance == null)
            {
                Instance = new MasterEntity();
                Instance.AddComponent<GameObjectComponent>();
            }
            return Instance;
        }
        public static void Destroy()
        {
            Destroy(Instance);
            Instance = null;
        }

        public override void LGUpdate(float time)
        {
            if (AllComponents.Count == 0)
            {
                return;
            }
            for (int i = AllComponents.Count - 1; i >= 0; i--)
            {
                var item = AllComponents[i];
                if (item.IsDisposed)
                {
                    AllComponents.RemoveAt(i);
                    continue;
                }
                if (item.Disable)
                {
                    continue;
                }
                item.LGUpdate(time);
            }
        }
    }
}