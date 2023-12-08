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
    [CreateAssetMenu(fileName = "技能配置", menuName = "实体/基础技能配置")]
    [LabelText("技能配置")]
    public class SkillBaseConfig : SerializedScriptableObject
    {
        [TitleGroup("Base"), LabelText("技能ID"), DelayedProperty]
        public int Id;
        [TitleGroup("Base"), LabelText("技能输入类型"), OnValueChanged("SkillTargetInputTypeChange")]
        public SkillTargetInputType SkillTargetInputType;
        [TitleGroup("Base"), LabelText("技能释放时长"), SuffixLabel("毫秒", true)]
        public int ReleaseTime = 100;

        [TitleGroup("Base"), LabelText("技能冷却时长"), SuffixLabel("毫秒", true)]
        public int SkillCD = 1000;

        [TitleGroup("Bullet"), LabelText("子弹配置")]
        public bool BulletEnabled { get { return SkillBulletType != SkillBallisticsType.None; } }

        [TitleGroup("Bullet"), LabelText("子弹弹道类型")]
        public SkillBallisticsType SkillBulletType;

        [TitleGroup("Bullet"), ShowIf("BulletEnabled"), LabelText("子弹特效ID")]
        public string BulletEffectId;
        [TitleGroup("Bullet"), ShowIf("BulletEnabled"), LabelText("子弹销毁特效ID")]
        public string BulletDestroyedEffectId;
        [TitleGroup("Bullet"), ShowIf("BulletEnabled"), LabelText("子弹速度")]
        public float BulletSpeed;

        #region 子弹效果集合
        [TitleGroup("Bullet"), ShowIf("BulletEnabled"), LabelText("子弹效果列表")]
        [ListDrawerSettings(DefaultExpandedState = true, DraggableItems = false, ShowItemCount = false, HideAddButton = true)]
        [HideReferenceObjectPicker]
        public List<LGEffect> BulletEffects = new();
        [TitleGroup("Bullet"), ShowIf("BulletEnabled")]
        [HideLabel, OnValueChanged("AddBulletEffect"), ValueDropdown("BulletEffectTypeSelect")]
        public string BulletEffectTypeName = "(添加子弹效果)";
        public IEnumerable<string> BulletEffectTypeSelect()
        {
            var types = typeof(LGEffect).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(LGEffect).IsAssignableFrom(x))
                .Where(x => x.GetCustomAttribute<LGEffectAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<LGEffectAttribute>().Order)
                .Select(x => x.GetCustomAttribute<LGEffectAttribute>().EffectType);
            var results = types.ToList();
            results.Insert(0, "(添加子弹效果)");
            return results;
        }
        private void AddBulletEffect()
        {
            if (BulletEffectTypeName != "(添加子弹效果)")
            {
                var effectType = typeof(LGEffect).Assembly.GetTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => typeof(LGEffect).IsAssignableFrom(x))
                    .Where(x => x.GetCustomAttribute<LGEffectAttribute>() != null)
                    .Where(x => x.GetCustomAttribute<LGEffectAttribute>().EffectType == BulletEffectTypeName)
                    .FirstOrDefault();

                var effect = Activator.CreateInstance(effectType) as LGEffect;
                effect.Enabled = true;
                BulletEffects.Add(effect);
                BulletEffectTypeName = "(添加子弹效果)";
            }
        }
        #endregion



        #region 添加技能效果
        [TitleGroup("SkillEffect"), LabelText("技能效果列表")]
        [ListDrawerSettings(DefaultExpandedState = true, DraggableItems = false, ShowItemCount = false, HideAddButton = true)]
        [HideReferenceObjectPicker]
        public List<LGEffect> SkillEffects = new();
        [TitleGroup("SkillEffect")]
        [HideLabel, OnValueChanged("AddSkillEffect"), ValueDropdown("SkillEffectTypeSelect")]
        public string SkillEffectTypeName = "(添加技能效果)";

        public IEnumerable<string> SkillEffectTypeSelect()
        {
            var types = typeof(LGEffect).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(LGEffect).IsAssignableFrom(x))
                .Where(x => x.GetCustomAttribute<LGEffectAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<LGEffectAttribute>().Order)
                .Select(x => x.GetCustomAttribute<LGEffectAttribute>().EffectType);
            var results = types.ToList();
            results.Insert(0, "(添加技能效果)");
            return results;
        }

        private void AddSkillEffect()
        {
            if (SkillEffectTypeName != "(添加技能效果)")
            {
                var effectType = typeof(LGEffect).Assembly.GetTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => typeof(LGEffect).IsAssignableFrom(x))
                    .Where(x => x.GetCustomAttribute<LGEffectAttribute>() != null)
                    .Where(x => x.GetCustomAttribute<LGEffectAttribute>().EffectType == SkillEffectTypeName)
                    .FirstOrDefault();

                var effect = Activator.CreateInstance(effectType) as LGEffect;
                effect.IsSkillEffect = true;
                effect.SetSkillTargetInputType(SkillTargetInputType);
                effect.Enabled = true;
                SkillEffects.Add(effect);
                SkillEffectTypeName = "(添加技能效果)";
            }
        }
        #endregion

        /// <summary>
        /// 技能输入类型变化 重置效果作用目标
        /// </summary>
        private void SkillTargetInputTypeChange()
        {
            foreach (var beffect in BulletEffects)
            {
                beffect.SetSkillTargetInputType(SkillTargetInputType);
            }
            foreach (var seffect in SkillEffects)
            {
                seffect.SetSkillTargetInputType(SkillTargetInputType);
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
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<SkillBaseConfig>(assetPath);
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