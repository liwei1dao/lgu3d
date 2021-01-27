local Module = Class.define("SoundModule",BaseModel)

function Module:New(_csobj)
	self:super(Module,"New", _csobj);
end

function Module:Load(...)
    self.ModelPlayer = GameObject.New("SoundModule");
    self.BackMusicPlayer = self.ModelPlayer:AddComponent(typeof(UnityEngine.AudioSource))
    self.BackMusicPlayer.loop = true
    self.CSModelObj:LoadResourceComp()
    GameObject.DontDestroyOnLoad(self.ModelPlayer)
	self:super(Module,"Load", ...);
end


function Module:PlayBackMusicByName(MusicName,volume)
    if volume == nil then
        volume =  1
    end
    local Audio = self:LoadAudioClip(MusicName)
    self:PlayBackMusicByAudio(Audio,volume)

end

function Module:PlayBackMusicByAudio(Audio,volume)
    if volume == nil then
        volume =  1
    end
    self.BackMusicPlayer.clip = Audio
    self.BackMusicPlayer.volume = volume
    self.BackMusicPlayer:Play()
end

function Module:PlayEffectMusicByName(MusicName,Point,volume)
    if Point == nil then
        Point =  Vector3(0,0,0)
    end
    if volume == nil then
        volume =  1
    end
    local Audio = self:LoadAudioClip(MusicName)
    self.PlayEffectMusicByAudio(Audio,Point,volume)
end

function Module:PlayEffectMusicByAudio(Audio,Point,volume)
    if Point == nil then
        Point =  Vector3(0,0,0)
    end
    if volume == nil then
        volume =  1
    end
    UnityEngine.AudioSource.PlayClipAtPoint(Audio,Point,volume)
end

return Module