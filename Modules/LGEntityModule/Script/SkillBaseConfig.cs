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
        [TitleGroup("Base"), LabelText("技能ID"), DelayedProperty]
        public int Id;
        [TitleGroup("Base"), LabelText("技能名称")]
        public string Name = "Skill_";

        [TitleGroup("Base"), LabelText("技能输出类型")]
        public SkillTargetInputType SkillTargetInputType;

        [TitleGroup("Base"), LabelText("技能释放时长"), SuffixLabel("毫秒", true)]
        public int ReleaseTime = 100;

        [TitleGroup("Base"), LabelText("技能冷却时长(毫秒)"), SuffixLabel("毫秒", true)]
        public int SkillCD = 1000;



        #region 子弹配置
        [TitleGroup("Bullet")]
        [LabelText("子弹弹道类型")]
        public SkillBallisticsType SkillBulletType;
        [TitleGroup("Bullet")]
        [LabelText("子弹特效ID"), HideIf("SkillBulletType", SkillBallisticsType.None)]
        public string BulletEffectId;
        [TitleGroup("Bullet")]
        [LabelText("子弹销毁特效ID"), HideIf("SkillBulletType", SkillBallisticsType.None)]
        public string BulletDestroyedEffectId;
        [TitleGroup("Bullet")]
        [LabelText("子弹速度"), HideIf("SkillBulletType", SkillBallisticsType.None)]
        public float BulletSpeed;
        [TitleGroup("Bullet")]


        #region 效果集合
        [OnInspectorGUI("BeginBox", append: false)]
        [LabelText("子弹效果列表"), Space(30)]
        [ListDrawerSettings(DefaultExpandedState = true, DraggableItems = false, ShowItemCount = false, HideAddButton = true)]
        [HideReferenceObjectPicker]
        public List<LGEffect> BulletEffects = new();
        [OnInspectorGUI("EndBox", append: true)]
        [HorizontalGroup(PaddingLeft = 40, PaddingRight = 40)]
        [HideLabel, OnValueChanged("AddEffect"), ValueDropdown("EffectTypeSelect")]
        public string EffectTypeName = "(添加效果)";

        public IEnumerable<string> BulletEffectTypeSelect()
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

        private void AddBulletEffect()
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
                BulletEffects.Add(effect);

                EffectTypeName = "(添加效果)";
            }
        }
        #endregion

        #endregion



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