using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{


  public class SoundDataComp : ModelCompBase<SoundModule>
  {
    private class ModelSoundPlayerData
    {
      public string ModelName;
      private float MusicValue;
      private float SoundValue;
      private GameObject ModelPlayer;
      public AudioSource BackMusicPlayer;
      public List<AudioSource> EffectMusicPlayer;

      public ModelSoundPlayerData(string _ModelName)
      {
        ModelName = _ModelName;
        MusicValue = 1;
        SoundValue = 1;
        ModelPlayer = SoundModule.Instance.SoundPlayers.CreateChild(ModelName);
        BackMusicPlayer = null;
        EffectMusicPlayer = new List<AudioSource>();
      }

      public AudioSource GetBackMusicPlayer()
      {
        if (BackMusicPlayer == null)
        {
          BackMusicPlayer = ModelPlayer.AddComponent<AudioSource>();
          BackMusicPlayer.loop = true;
        }
        return BackMusicPlayer;
      }

      public AudioSource GetEffectMusicPlayer()
      {
        for (int i = 0; i < EffectMusicPlayer.Count; i++)
        {
          if (!EffectMusicPlayer[i].isPlaying)
          {
            return EffectMusicPlayer[i];
          }
        }
        AudioSource MusicPlayer = ModelPlayer.AddComponent<AudioSource>();
        MusicPlayer.volume = SoundValue;
        EffectMusicPlayer.Add(MusicPlayer);
        return MusicPlayer;
      }

      public void StopBackMusic()
      {
        GetBackMusicPlayer().Stop();
      }

      public void PauseBackMusic()
      {
        GetBackMusicPlayer().Pause();
      }
      public void UnPauseBackMusic()
      {
        GetBackMusicPlayer().UnPause();
      }
      /// <summary>
      /// 设置背景音量
      /// </summary>
      /// <param name="MusicValue"></param>
      public void SetBackMusicValue(float musicValue)
      {
        MusicValue = musicValue;
        GetBackMusicPlayer().volume = musicValue;
      }
      /// <summary>
      /// 获取背景音量
      /// </summary>
      /// <param name="MusicValue"></param>
      public float GetBackMusicValue()
      {
        return MusicValue;
      }
      /// <summary>
      /// 设置特效音量
      /// </summary>
      /// <param name="soundValue"></param>
      public void SetEffectMusicValue(float soundValue)
      {
        SoundValue = soundValue;
        for (int i = 0; i < EffectMusicPlayer.Count; i++)
        {
          EffectMusicPlayer[i].volume = soundValue;
        }
      }
      /// <summary>
      /// 获取特效音量
      /// </summary>
      /// <param name="soundValue"></param>
      public float GetEffectMusicValue()
      {
        return SoundValue;
      }
    }

    private Dictionary<string, ModelSoundPlayerData> ModelMusicPLayers;

    public override void Load(ModuleBase _ModelContorl, params object[] agrs)
    {
      ModelMusicPLayers = new Dictionary<string, ModelSoundPlayerData>();
      base.Load(_ModelContorl, agrs);
      LoadEnd();
    }

    public void InitModelMusicPLayers(string ModelName)
    {
      if (!ModelMusicPLayers.ContainsKey(ModelName))
      {
        ModelMusicPLayers[ModelName] = new ModelSoundPlayerData(ModelName);
      }
    }

    public void PauseBackMusic(string ModelName)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      Player.PauseBackMusic();
    }
    public void UnPauseBackMusic(string ModelName)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      Player.UnPauseBackMusic();
    }

    public void SetBackMusicValue(string ModelName, float MusicValue)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      Player.SetBackMusicValue(MusicValue);
    }
    public float GetBackMusicValue(string ModelName)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      return Player.GetBackMusicValue();
    }
    public void SetEffectMusicValue(string ModelName, float MusicValue)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      Player.SetEffectMusicValue(MusicValue);
    }
    public float GetEffectMusicValue(string ModelName)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      return Player.GetEffectMusicValue();
    }
    public AudioSource PlayMusic(string ModelName, AudioClip Music, bool IsBackMusic)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      AudioSource AudioPlayer = null;
      if (IsBackMusic)
      {
        AudioPlayer = Player.GetBackMusicPlayer();
        AudioPlayer.clip = Music;
        AudioPlayer.Play();
      }
      else
      {
        AudioPlayer = Player.GetEffectMusicPlayer();
        AudioPlayer.PlayOneShot(Music);
      }

      return AudioPlayer;
    }

    public AudioSource PlayMusic(string ModelName, AudioClip Music, float MusicValue, bool IsBackMusic)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      AudioSource AudioPlayer = null;
      if (IsBackMusic)
      {
        AudioPlayer = Player.GetBackMusicPlayer();
        AudioPlayer.volume = MusicValue;
        AudioPlayer.clip = Music;
        AudioPlayer.Play();
      }
      else
      {
        AudioPlayer = Player.GetEffectMusicPlayer();
        AudioPlayer.volume = MusicValue;
        AudioPlayer.PlayOneShot(Music);
      }
      return AudioPlayer;
    }
    public void StopBackMusic(string ModelName)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      Player.StopBackMusic();
    }
  }
}
