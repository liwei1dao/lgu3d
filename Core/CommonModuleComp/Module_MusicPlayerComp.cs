using UnityEngine;

namespace lgu3d
{
    public class Module_MusicPlayerComp : ModelCompBase
    {
        #region 框架构造
        public override void Load(ModuleBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl);
            base.LoadEnd();
        }
        #endregion

        public void PlayBackMusic(string MusicName)
        {
            AudioClip Music = MyModule.LoadAsset<AudioClip>("Sound", MusicName);
            PlayBackMusic(Music);
        }

        public void PlayBackMusic(AudioClip Music)
        {
            SoundModule.Instance.PlayMusic(MyModule.ModuleName, Music, true);
        }

        public void PlayEffetcMusic(string MusicName)
        {
            AudioClip Music = MyModule.LoadAsset<AudioClip>("Sound", MusicName);
            PlayBackMusic(Music);
        }
        public void PlayEffetcMusic(AudioClip Music)
        {
            SoundModule.Instance.PlayMusic(MyModule.ModuleName, Music, false);
        }
    }
}
