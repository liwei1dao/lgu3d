using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;

#if !NOT_UNITY
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
#endif

namespace lgu3d
{
    [CreateAssetMenu(fileName = "技能配置", menuName = "实体/技能配置")]
    [LabelText("技能配置")]
    public class SkillConfigObject
#if !NOT_UNITY
        : SerializedScriptableObject
#endif
    {
        [LabelText("技能ID"), DelayedProperty]
        public int Id;

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
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<SkillConfigObject>(assetPath);
                if (so != this)
                {
                    return;
                }
                var fileName = Path.GetFileName(assetPath);
                var newName = $"Skill_{this.Id}";
                if (!fileName.StartsWith(newName))
                {
                    //Debug.Log(assetPath);
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, newName);
                }
            }
        }
    }
}