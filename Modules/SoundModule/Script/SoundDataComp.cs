using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{


  public class SoundDataComp : ModelCompBase<SoundModule>
  {
    private class ModelSoundPlayerData
    {
      public string ModelName;
      private GameObject ModelPlayer;
      public AudioSource BackMusicPlayer;
      public List<AudioSource> EffectMusicPlayer;

      public ModelSoundPlayerData(string _ModelName)
      {
        ModelName = _ModelName;
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
        EffectMusicPlayer.Add(MusicPlayer);
        return MusicPlayer;
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
      public void SetBackMusicValue(float MusicValue)
      {
        GetBackMusicPlayer().volume = MusicValue;
      }
      /// <summary>
      /// 设置特效音量
      /// </summary>
      /// <param name="MusicValue"></param>
      public void SetEffectMusicValue(float MusicValue)
      {
        for (int i = 0; i < EffectMusicPlayer.Count; i++)
        {
          EffectMusicPlayer[i].volume = MusicValue;
        }
      }
    }

    private Dictionary<string, ModelSoundPlayerData> ModelMusicPLayers;

    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      ModelMusicPLayers = new Dictionary<string, ModelSoundPlayerData>();
      base.Load(_ModelContorl, _Agr);
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

    public void SetEffectMusicValue(string ModelName, float MusicValue)
    {
      ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
      Player.SetEffectMusicValue(MusicValue);
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
  }

}
