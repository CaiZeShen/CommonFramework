﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class MLuaResLoaderWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(MLuaResLoader), typeof(System.Object));
		L.RegFunction("GetResoures", GetResoures);
		L.RegFunction("New", _CreateMLuaResLoader);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Instance", get_Instance, null);
		L.RegVar("CallBackManager", get_CallBackManager, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateMLuaResLoader(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				MLuaResLoader obj = new MLuaResLoader();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: MLuaResLoader.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetResoures(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 6);
			MLuaResLoader obj = (MLuaResLoader)ToLua.CheckObject<MLuaResLoader>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			string arg1 = ToLua.CheckString(L, 3);
			string arg2 = ToLua.CheckString(L, 4);
			bool arg3 = LuaDLL.luaL_checkboolean(L, 5);
			LuaFunction arg4 = ToLua.CheckLuaFunction(L, 6);
			obj.GetResoures(arg0, arg1, arg2, arg3, arg4);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, MLuaResLoader.Instance);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CallBackManager(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			MLuaResLoader obj = (MLuaResLoader)o;
			MLuaResCallBackManager ret = obj.CallBackManager;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index CallBackManager on a nil value" : e.Message);
		}
	}
}
