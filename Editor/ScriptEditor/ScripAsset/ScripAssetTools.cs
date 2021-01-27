using UnityEditor;
using UnityEngine;

namespace lgu3d.Editor
{
    public static class ScripAssetTools
    {
        [MenuItem("Assets/lgu3d/ScripAsset/CarConfig", false, 2)]
        static void CreateCarConfig()
        {
            string TargetPath = EditorHelper.GetSelectedPathOrFallback();
            Debug.Log(TargetPath);
            ScriptableObject ddata = ScriptableObject.CreateInstance("CarTableCarConfig");
            AssetDatabase.CreateAsset(ddata, TargetPath + "/CarTableCarConfig.asset");
        }

    }
}

