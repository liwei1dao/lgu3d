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

        [LabelText("技能释放时长"), SuffixLabel("毫秒", true)]
        public int ReleaseTime = 100;

        [LabelText("技能冷却时长(毫秒)"), SuffixLabel("毫秒", true)]
        public int SkillCD = 1000;

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