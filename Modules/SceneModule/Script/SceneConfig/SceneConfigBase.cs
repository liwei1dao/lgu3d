﻿
namespace lgu3d
{
    [System.Serializable]
    public class SceneDataBase : ConfigDataBase<int>
    {
        public string SceneName;
    }

    public class SceneTableDataBase : ConfigTableDataBase<int, SceneDataBase>
    {

    }
}

