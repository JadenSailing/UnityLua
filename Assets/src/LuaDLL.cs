using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static LuaInterface;

public class LuaDLL
{
    const string LUADLL = "lua5.4.dll";
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr luaL_newstate();
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int lua_setglobal(IntPtr L, string name);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void lua_pushcclosure(IntPtr luaState, IntPtr fn, int n);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int luaL_loadbufferx(IntPtr L, byte[] buff, int size, string name, byte[] mode);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int lua_pcallk(IntPtr L, int nArgs, int nResults, int errfunc, int context, IntPtr kFunc);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr lua_tolstring(IntPtr L, int index, out IntPtr strLen);
}
