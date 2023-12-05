using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace lgu3d
{
    public abstract class SkillBaseConfig<C> : SerializedScriptableObject where C : SkillBaseConfig<C>
    {
        [LabelText("技能ID"), DelayedProperty]
        public int Id;
        [LabelText("技能名称")]
        public string Name = "Skill_";

        [LabelText("技能释放时长(毫秒)")]
        public int ReleaseTime = 100;

        [LabelText("技能冷却时长(毫秒)")]
        public int SkillCD = 1000;

        [FoldoutGroup("子弹配置")]
        [LabelText("子弹ID")]
        public string BulletId;
        [FoldoutGroup("子弹配置")]
        public LGBulletTarget LGBulletTarget;
        [FoldoutGroup("子弹配置")]
        public LGBulletTrack LGBulletTrack;


        [OnInspectorGUI("BeginBox", append: false)]
        [LabelText("效果列表"), Space(30)]
        [ListDrawerSettings(DefaultExpandedState = true, DraggableItems = false, ShowItemCount = false, HideAddButton = true)]
        [HideReferenceObjectPicker]
        public List<LGEffect> Effects = new();
        [OnInspectorGUI("EndBox", append: true)]
        [HorizontalGroup(PaddingLeft = 40, PaddingRight = 40)]
        [HideLabel, OnValueChanged("AddEffect"), ValueDropdown("EffectTypeSelect")]
        public string EffectTypeName = "(添加效果)";

        public IEnumerable<string> EffectTypeSelect()
        {
            var types = typeof(LGEffect).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(LGEffect).IsAssignableFrom(x))
                .Where(x => x.GetCustomAttribute<LGEffectAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<LGEffectAttribute>().Order)
                .Select(x => x.GetCustomAttribute<LGEffectAttribute>().EffectType);
            var results = types.ToList();
            results.Insert(0, "(添加效果)");
            return results;
        }

        private void AddEffect()
        {
            if (EffectTypeName != "(添加效果)")
            {
                var effectType = typeof(LGEffect).Assembly.GetTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => typeof(LGEffect).IsAssignableFrom(x))
                    .Where(x => x.GetCustomAttribute<LGEffectAttribute>() != null)
                    .Where(x => x.GetCustomAttribute<LGEffectAttribute>().EffectType == EffectTypeName)
                    .FirstOrDefault();

                var effect = Activator.CreateInstance(effectType) as LGEffect;
                effect.Enabled = true;
                Effects.Add(effect);

                EffectTypeName = "(添加效果)";
            }
        }








        protected void DrawSpace()
        {
            GUILayout.Space(20);
        }

        protected void BeginBox()
        {
            //GUILayout.Space(30);
            SirenixEditorGUI.DrawThickHorizontalSeparator();
            GUILayout.Space(10);
            //SirenixEditorGUI.BeginBox("技能表现");
        }

        protected void EndBox()
        {
            //SirenixEditorGUI.EndBox();
            GUILayout.Space(30);
            //SirenixEditorGUI.DrawThickHorizontalSeparator();
            //GUILayout.Space(10);
        }

        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            RenameFile();
        }
        private void RenameFile()
        {
            string[] guids = UnityEditor.Selection.assetGUIDs;
            int i = guids.Length;
            if (i == 1)
            {
                string guid = guids[0];
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<C>(assetPath);
                if (so != this)
                {
                    return;
                }
                var fileName = Path.GetFileName(assetPath);
                var newName = $"Skill_{this.Id}";
                if (!fileName.StartsWith(newName))
                {
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, newName);
                }
            }
        }
    }
}