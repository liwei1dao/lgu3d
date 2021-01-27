--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
if jit then		
	if jit.opt then		
		jit.opt.start(3)				
	end		
	
	print("ver"..jit.version_num.." jit: ", jit.status())
	print(string.format("os: %s, arch: %s", jit.os, jit.arch))
end
if DebugServerIp then  
  require("mobdebug").start(DebugServerIp)
end

require "LuaManagerModule.ToLua.misc.functions"
Mathf		= require "LuaManagerModule.ToLua.UnityEngine.Mathf"
Vector3 	= require "LuaManagerModule.ToLua.UnityEngine.Vector3"
Quaternion	= require "LuaManagerModule.ToLua.UnityEngine.Quaternion"
Vector2		= require "LuaManagerModule.ToLua.UnityEngine.Vector2"
Vector4		= require "LuaManagerModule.ToLua.UnityEngine.Vector4"
Color		= require "LuaManagerModule.ToLua.UnityEngine.Color"
Ray			= require "LuaManagerModule.ToLua.UnityEngine.Ray"
Bounds		= require "LuaManagerModule.ToLua.UnityEngine.Bounds"
RaycastHit	= require "LuaManagerModule.ToLua.UnityEngine.RaycastHit"
Touch		= require "LuaManagerModule.ToLua.UnityEngine.Touch"
LayerMask	= require "LuaManagerModule.ToLua.UnityEngine.LayerMask"
Plane		= require "LuaManagerModule.ToLua.UnityEngine.Plane"
Time		= reimport "LuaManagerModule.ToLua.UnityEngine.Time"

list		= require "LuaManagerModule.ToLua.list"
utf8		= require "LuaManagerModule.ToLua.misc.utf8"

require "LuaManagerModule.ToLua.event"
require "LuaManagerModule.ToLua.typeof"
require "LuaManagerModule.ToLua.slot"
require "LuaManagerModule.ToLua.System.Timer"
require "LuaManagerModule.ToLua.System.coroutine"
require "LuaManagerModule.ToLua.System.ValueType"
require "LuaManagerModule.ToLua.System.Reflection.BindingFlags"

-- require "LuaManagerModule.ToLua.protobuf.wire_format"
-- require "LuaManagerModule.ToLua.protobuf.type_checkers"
-- require "LuaManagerModule.ToLua.protobuf.encoder"
-- require "LuaManagerModule.ToLua.protobuf.decoder"
-- require "LuaManagerModule.ToLua.protobuf.listener"
-- require "LuaManagerModule.ToLua.protobuf.containers"
-- require "LuaManagerModule.ToLua.protobuf.descriptor"
-- require "LuaManagerModule.ToLua.protobuf.text_format"
-- require "LuaManagerModule.ToLua.protobuf.protobuf"

protoc = require "LuaManagerModule.ToLua.protoc"  --pb 加载工具
serpent = require "LuaManagerModule.ToLua.serpent"  --pb 加载工具