using UnityEngine;


namespace lgu3d
{
    /// <summary>
    /// 音频播放器模块
    /// </summary>
    public class SoundModule : ManagerContorBase<SoundModule>
    {
        private SoundDataComp DataComp;
        public GameObject SoundPlayers { get; private set; }

        public override void Load(params object[] _Agr)
        {
            SoundPlayers = new GameObject("SoundPlayers");
            DataComp = AddComp<SoundDataComp>();
            Object.DontDestroyOnLoad(SoundPlayers);
            base.Load(_Agr);
        }

        public void InitModelSoundPlayer(string ModelName)
        {
            DataComp.InitModelMusicPLayers(ModelName);
        }

        /// <summary>
        /// 播放模块音乐/背景音乐
        /// </summary>
        /// <param name="ModelName"></param>
        /// <param name="Music"></param>
        /// <param name="IsBackMusic"></param>
        public AudioSource PlayMusic(string ModelName,AudioClip Music,bool IsBackMusic = false)
        {
            return DataComp.PlayMusic(ModelName, Music, IsBackMusic);
        }

        public AudioSource PlayMusic(string ModelName, AudioClip Music,float MusicValue, bool IsBackMusic = false)
        {
            return DataComp.PlayMusic(ModelName, Music, MusicValue, IsBackMusic);
        }


    }
}
