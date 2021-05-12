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

public class Program : MonoBehaviour
{

    public static string log = "";
    public Text label;
    void Start()
    {
        try
        {
            IntPtr L = LuaDLL.luaL_newstate();
            IntPtr fn = Marshal.GetFunctionPointerForDelegate<LuaInterface.Lua_CSFunction>(CLog);
            LuaDLL.lua_pushcclosure(L, fn, 0);
            LuaDLL.lua_setglobal(L, "CLog");
            string luaStr = @"CLog('Hello World')";
            byte[] bytes = Encoding.UTF8.GetBytes(luaStr);
            int ret = LuaDLL.luaL_loadbufferx(L, bytes, bytes.Length, "chunk", null);
            if (ret == 0)
            {
                LuaDLL.lua_pcallk(L, 0, 0, 0, 0, IntPtr.Zero);
            }

            /*
            IntPtr L = LuaInterface.CreateLua();
            LuaInterface.RegisterFunc(L, "print", print);
            string luaStr = "print(\"hello\")";
            LuaInterface.DoString(L, luaStr);

            Console.Read();
            */
        }
        catch(System.Exception e)
        {
            log = e.ToString();
        }
    }

    private void Update()
    {
        label.text = log;
    }

    static int CLog(IntPtr L)
    {
        IntPtr strlen;
        IntPtr str = LuaDLL.lua_tolstring(L, 1, out strlen);
        if (str != IntPtr.Zero)
        {
            string ret = Marshal.PtrToStringAnsi(str, strlen.ToInt32());
            byte[] buffer = new byte[strlen.ToInt32() + 1];
            Marshal.Copy(str, buffer, 0, strlen.ToInt32() + 1);
            log = string.Format("C#Log: {0}", ret);
            UnityEngine.Debug.Log(log);
        }
        return 0;
    }
}
