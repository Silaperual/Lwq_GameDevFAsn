using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;
using ABFW;


namespace LuaFramework
{
    public class LuaHelper 
    {
        //本类静态实例
        private static LuaHelper _Instance;
        //Lua 环境
        private LuaEnv _luaEnv = new LuaEnv();
        //缓存lua文件名称与对应的lua信息。
        private Dictionary<string, byte[]> _DicLuaFileArray = new Dictionary<string, byte[]>();


        private LuaHelper()
        {
            //私有构造函数
            _luaEnv.AddLoader(customLoader);
        }

        //得到帮助类实例
        public static LuaHelper GetInstance()
        {
            if (_Instance==null)
            {
                _Instance = new LuaHelper();
            }
            return _Instance;
        }

        // 得到lua环境
        public LuaEnv GetLuaEnv()
        {
            if (_luaEnv!=null)
            {
                return _luaEnv;
            }
            else {
                Debug.LogError(GetType()+ "/GetLuaEnv()/出现严重错误！   _luaEnv==null!!!   ");
                return null;
            }
        }

        //执行lua代码
        public void DoString(string chunk, string chunkName = "chunk", LuaTable env = null)
        {
            _luaEnv.DoString(chunk, chunkName, env);
        }

        // 调用lua中的方法
        public object[] CallLuaFunction(string luaScriptName,string luaMethodName,params object[] args)
        {
            LuaTable  luaTab=_luaEnv.Global.Get<LuaTable>(luaScriptName);
            LuaFunction luaFun = luaTab.Get<LuaFunction>(luaMethodName);
            return luaFun.Call(args);
        }

        // 自定义调取lua文件内容
        private byte[] customLoader(ref string fileName)
        {
            //获取lua所在目录
            //string luaPath = PathTools.GetABOutPath() + PathTools.LUA_DEPLOY_PATH;//Original
            string luaPath = PathTools.GetABOutPath() + HotUpdateProcess.HotUpdatePathTool.LUA_DEPLOY_PATH;

            //缓存判断处理： 根据lua文件路径，获取lua的内容
            if (_DicLuaFileArray.ContainsKey(fileName))
            {
                //如果在缓存中可以查找成功，则直接返回结果。
                return _DicLuaFileArray[fileName];
            }
            else {
                return ProcessDIR(new DirectoryInfo(luaPath), fileName);
            }
        }

        // 根据lua文件名称，递归取得lua内容信息,且放入缓存集合
        private byte[] ProcessDIR(FileSystemInfo fileSysInfo, string fileName)
        {
            DirectoryInfo dirInfo=fileSysInfo as DirectoryInfo;
            FileSystemInfo[] files=dirInfo.GetFileSystemInfos();

            foreach (FileSystemInfo item in files)
            {
                FileInfo fileInfo = item as FileInfo;
                //表示一个文件夹
                if (fileInfo == null)
                {
                    //递归处理
                    ProcessDIR(item, fileName);
                }
                //表示文件本身
                else {
                    //得到文件本身，去掉后缀
                    string tmpName = item.Name.Split('.')[0];
                    if (item.Extension==".meta" || tmpName!= fileName)
                    {
                        continue;
                    }
                    //读取lua文件内容字节信息
                    byte[] bytes = File.ReadAllBytes(fileInfo.FullName);
                    //添加到缓存集合中
                    _DicLuaFileArray.Add(fileName,bytes);
                    return bytes;
                }

            }
            return null;
        }

        // 给指定对象，动态添加“BaseLuaUIForm”脚本
        public void AddBaseLuaUIForm(GameObject go)
        {
            go.AddComponent<BaseLuaUIForm>();
        }
    }
}


