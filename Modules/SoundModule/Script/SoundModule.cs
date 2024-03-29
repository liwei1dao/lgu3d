﻿using UnityEngine;


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
            SoundPlayers = new GameObject("SoundPlayers",typeof(AudioListener));
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
        /// <param name="ModelName">模块名称</param>
        /// <param name="Music">音效文件</param>
        /// <param name="IsBackMusic">是否是背景英语</param>
        public AudioSource PlayMusic(string ModelName,AudioClip Music,bool IsBackMusic = false)
        {
            return DataComp.PlayMusic(ModelName, Music, IsBackMusic);
        }

        /// <summary>
        /// 播放模块音乐/背景音乐
        /// </summary>
        /// <param name="ModelName">模块名称</param>
        /// <param name="Music">音效文件</param>
        /// <param name="MusicValue">音量大小</param>
        /// <param name="IsBackMusic">是否是背景英语</param>
        /// <returns></returns>
        public AudioSource PlayMusic(string ModelName, AudioClip Music,float MusicValue, bool IsBackMusic = false)
        {
            return DataComp.PlayMusic(ModelName, Music, MusicValue, IsBackMusic);
        }


    }
}
