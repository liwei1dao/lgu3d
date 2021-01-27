using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{
    

    public class SoundDataComp : ModelCompBase<SoundModule>
    {
        private struct ModelSoundPlayerData
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


        public AudioSource PlayMusic(string ModelName, AudioClip Music, bool IsBackMusic)
        {
            ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
            AudioSource AudioPlayer = null;
            if (IsBackMusic)
            {
                AudioPlayer = Player.GetBackMusicPlayer();
                AudioPlayer.Stop();
            }
            else
            {
                AudioPlayer = Player.GetEffectMusicPlayer();
            }
            AudioPlayer.PlayOneShot(Music);
            return AudioPlayer;
        }

        public AudioSource PlayMusic(string ModelName, AudioClip Music,float MusicValue, bool IsBackMusic)
        {
            ModelSoundPlayerData Player = ModelMusicPLayers[ModelName];
            AudioSource AudioPlayer = null;
            if (IsBackMusic)
            {
                AudioPlayer = Player.GetBackMusicPlayer();
                AudioPlayer.Stop();
            }
            else
            {
                AudioPlayer = Player.GetEffectMusicPlayer();
            }
            AudioPlayer.volume = MusicValue;
            AudioPlayer.PlayOneShot(Music);
            return AudioPlayer;
        }
    }

}
