using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 模块音频组件
  /// </summary>
  public class Module_SoundComp : ModelCompBase
  {
    #region 框架构造
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      if (SoundModule.Instance == null)
      {
        Debug.LogError("SoundModule User but No Load");
        return;
      }
      base.Load(_ModelContorl);
      base.LoadEnd();
    }

    public override void Start(params object[] agr)
    {
      SoundModule.Instance.InitModelSoundPlayer(MyModule.ModuleName);
      base.Start(agr);
    }

    #endregion
    public void PauseBackMusic()
    {
      SoundModule.Instance.PauseBackMusic(MyModule.ModuleName);
    }
    public void UnPauseBackMusic()
    {
      SoundModule.Instance.UnPauseBackMusic(MyModule.ModuleName);
    }

    public void SetBackMusicValue(string ModelName, float soundValue)
    {
      SoundModule.Instance.SetBackMusicValue(MyModule.ModuleName, soundValue);
    }

    public void SetEffectMusicValue(string ModelName, float soundValue)
    {
      SoundModule.Instance.SetEffectMusicValue(MyModule.ModuleName, soundValue);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="ModelName"></param>
    /// <param name="Music"></param>
    /// <param name="MusicValue"></param>
    /// <param name="IsBackMusic"></param>
    /// <returns></returns>
    public AudioSource PlayMusic(string Music, bool IsBackMusic = false)
    {
      AudioClip music = MyModule.LoadAsset<AudioClip>("Sound", Music);
      return SoundModule.Instance.PlayMusic(MyModule.ModuleName, music, IsBackMusic);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="ModelName"></param>
    /// <param name="Music"></param>
    /// <param name="MusicValue"></param>
    /// <param name="IsBackMusic"></param>
    /// <returns></returns>
    public AudioSource PlayMusic(string Music, float MusicValue, bool IsBackMusic = false)
    {
      AudioClip music = MyModule.LoadAsset<AudioClip>("Sound", Music);
      return SoundModule.Instance.PlayMusic(MyModule.ModuleName, music, MusicValue, IsBackMusic);
    }

  }
}