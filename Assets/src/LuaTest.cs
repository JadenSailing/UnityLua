using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LuaTest : MonoBehaviour
{
    void Start()
    {
        try
        {
            IntPtr L = LuaDLL.luaL_newstate();

            IntPtr fn = Marshal.GetFunctionPointerForDelegate<LuaInterface.Lua_CSFunction>(CSPrint);
            LuaDLL.lua_pushcclosure(L, fn, 0);
            LuaDLL.lua_setglobal(L, "CSPrint");

            string luaStr = "CSPrint('Hello World')";
            byte[] bytes = Encoding.UTF8.GetBytes(luaStr);
            int ret = LuaDLL.luaL_loadbufferx(L, bytes, bytes.Length, "chunk", null);
            if (ret == 0)
            {
                int callRet = LuaDLL.lua_pcallk(L, 0, 0, 0, 0, IntPtr.Zero);
            }
        }
        catch(System.Exception e)
        {
            UnityEngine.Debug.LogError(e.ToString());
        }
    }

    static int CSPrint(IntPtr L)
    {
        IntPtr strlen;
        IntPtr str = LuaDLL.lua_tolstring(L, 1, out strlen);
        if (str != IntPtr.Zero)
        {
            string ret = Marshal.PtrToStringAnsi(str, strlen.ToInt32());
            UnityEngine.Debug.Log(ret);
        }
        return 0;
    }
}
