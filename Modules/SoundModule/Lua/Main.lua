module ("SoundModule", package.seeall);

ModelControl = Class.new(require "SoundModule.Module")

function New(_csobj)
	ModelControl:New(_csobj)
end

function Load(...)
	ModelControl:Load(...)
end

function LoadEnd()
	return ModelControl:GetEnd()
end

function Start(...)
	ModelControl:Start(...)
end

function Close()
	ModelControl:Close()
end

-----------------------------------------------------------------对外接口---------------------------------------------------------------------------------------------------
-- function PlayMusic(MusicName,IsBackMusic)
-- 	ModelControl:PlayMusic(MusicName,IsBackMusic)
-- end

-- function PlayMusicByAudio(Audio,IsBackMusic)
-- 	ModelControl:PlayMusicByAudio(Audio,IsBackMusic)
-- end

function PlayBackMusicByName(MusicName,volume)
	ModelControl:PlayBackMusicByName(MusicName,volume)
end

function PlayBackMusicByAudio(Audio,volume)
	ModelControl:PlayBackMusicByAudio(Audio,volume)
end

function PlayEffectMusicByName(MusicName,Point,volume)
	ModelControl:PlayEffectMusicByName(MusicName,Point,volume)
end

function PlayEffectMusicByAudio(Audio,Point,volume)
	ModelControl:PlayEffectMusicByAudio(Audio,Point,volume)
end