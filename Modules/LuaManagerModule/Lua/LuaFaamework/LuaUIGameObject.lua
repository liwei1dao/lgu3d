LuaUIGameObject = Class.define("LuaUIGameObject",LuaGameObject)

function LuaUIGameObject:ctor(obj,uilevel)
    self._UILevel = uilevel
    self:super(LuaUIGameObject,"ctor",obj);
end


function BaseModelViewComp:getter_UILevel()
	return self._UILevel
end