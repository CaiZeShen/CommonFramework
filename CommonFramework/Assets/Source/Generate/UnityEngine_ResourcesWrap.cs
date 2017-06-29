﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_ResourcesWrap
{
	public static void Register(LuaState L)
	{
		L.BeginStaticLibs("Resources");
		L.RegFunction("FindObjectsOfTypeAll", FindObjectsOfTypeAll);
		L.RegFunction("Load", Load);
		L.RegFunction("LoadAsync", LoadAsync);
		L.RegFunction("LoadAll", LoadAll);
		L.RegFunction("GetBuiltinResource", GetBuiltinResource);
		L.RegFunction("UnloadAsset", UnloadAsset);
		L.RegFunction("UnloadUnusedAssets", UnloadUnusedAssets);
		L.EndStaticLibs();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindObjectsOfTypeAll(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			System.Type arg0 = ToLua.CheckMonoType(L, 1);
			UnityEngine.Object[] o = UnityEngine.Resources.FindObjectsOfTypeAll(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Load(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes<string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				UnityEngine.Object o = UnityEngine.Resources.Load(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<string, System.Type>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				System.Type arg1 = (System.Type)ToLua.ToObject(L, 2);
				UnityEngine.Object o = UnityEngine.Resources.Load(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Resources.Load");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAsync(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes<string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				UnityEngine.ResourceRequest o = UnityEngine.Resources.LoadAsync(arg0);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<string, System.Type>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				System.Type arg1 = (System.Type)ToLua.ToObject(L, 2);
				UnityEngine.ResourceRequest o = UnityEngine.Resources.LoadAsync(arg0, arg1);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Resources.LoadAsync");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAll(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes<string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				UnityEngine.Object[] o = UnityEngine.Resources.LoadAll(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<string, System.Type>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				System.Type arg1 = (System.Type)ToLua.ToObject(L, 2);
				UnityEngine.Object[] o = UnityEngine.Resources.LoadAll(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Resources.LoadAll");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBuiltinResource(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			System.Type arg0 = ToLua.CheckMonoType(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			UnityEngine.Object o = UnityEngine.Resources.GetBuiltinResource(arg0, arg1);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadAsset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 1);
			UnityEngine.Resources.UnloadAsset(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadUnusedAssets(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UnityEngine.AsyncOperation o = UnityEngine.Resources.UnloadUnusedAssets();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

