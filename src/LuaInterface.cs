using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class LuaInterface
{
    enum Lua_Type
    {
        LUA_TNONE = -1,
        LUA_TNIL = 0,
        LUA_TBOOLEAN = 1,
        LUA_TLIGHTUSERDATA = 2,
        LUA_TNUMBER = 3,
        LUA_TSTRING = 4,
        LUA_TTABLE = 5,
        LUA_TFUNCTION = 6,
        LUA_TUSERDATA = 7,
        LUA_TTHREAD = 8,
        LUA_NUMTYPES = 9
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int Lua_CSFunction(IntPtr L);

    public static IntPtr CreateLua()
    {
        return LuaDLL.luaL_newstate();
    }

    public static void RegisterFunc(IntPtr L, string name, Lua_CSFunction func)
    {
        IntPtr fn = Marshal.GetFunctionPointerForDelegate(func);
        LuaDLL.lua_pushcclosure(L, fn, 0);
        LuaDLL.lua_setglobal(L, name);
    }

    public static void DoString(IntPtr L, string s, string chunk = "Lua")
    {
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        int ret = LuaDLL.luaL_loadbufferx(L, bytes, bytes.Length, chunk, null);
        if (ret == 0)
        {
            LuaDLL.lua_pcallk(L, 0, 0, 0, 0, IntPtr.Zero);
        }
    }

    public static string lua_tostring(IntPtr L, int index)
    {
        IntPtr strlen;
        IntPtr str = LuaDLL.lua_tolstring(L, index, out strlen);
        if (str != IntPtr.Zero)
        {
            string ret = Marshal.PtrToStringAnsi(str, strlen.ToInt32());
            if (ret == null)
            {
                int len = strlen.ToInt32();
                byte[] buffer = new byte[len];
                Marshal.Copy(str, buffer, 0, len);
                return Encoding.UTF8.GetString(buffer);
            }
            return ret;
        }
        else
        {
            return null;
        }
    }
}
