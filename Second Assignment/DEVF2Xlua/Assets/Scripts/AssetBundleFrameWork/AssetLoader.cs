using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW
{
    public class AssetLoader : System.IDisposable
    {
        //当前Assetbundle 
        private AssetBundle _CurrentAssetBundle;
        //缓存容器集合
        private Hashtable _Ht;

        //构造函数
        // abObj:给定WWW加载的AssetBundle 实例
        public AssetLoader(AssetBundle abObj)
        {
            if (abObj != null)
            {
                _CurrentAssetBundle = abObj;
                _Ht = new Hashtable();
            }
            else {
                Debug.Log(GetType()+"/ 构造函数 AssetBundle()/ 参数 abObj==null! ,请检查");
            }
        }

        // 加载当前包中指定的资源
        public UnityEngine.Object LoadAsset(string assetName,bool isCache=false)
        {
            return LoadResource<UnityEngine.Object>(assetName, isCache);
        }

        // 加载当前AB包的资源，带缓存
        private T LoadResource<T>(string assetName,bool isCache) where T : UnityEngine.Object
        {
            //是否缓存集合已经存在
            if (_Ht.Contains(assetName))
            {
                return _Ht[assetName] as T;
            }

            //正式加载
            T tmpTResource=_CurrentAssetBundle.LoadAsset<T>(assetName);
            //加入缓存集合
            if (tmpTResource != null && isCache)
            {
                _Ht.Add(assetName, tmpTResource);
            } else if (tmpTResource==null)
            {
                Debug.LogError(GetType()+ "/LoadResource<T>()/参数 tmpTResources==null, 请检查！");
            }

            return tmpTResource;
        }

        // 卸载指定的资源
        public bool UnLoadAsset(UnityEngine.Object asset)
        {
            if(asset!=null)
            {
                Resources.UnloadAsset(asset);
                return true;
            }
            Debug.LogError(GetType()+ "/UnLoadAsset()/参数 asset==null ,请检查！");
            return false;
        }

        // 释放当前AssetBundle内存镜像资源
        public void Dispose()
        {
            _CurrentAssetBundle.Unload(false);
        }

        // 释放当前AssetBundle内存镜像资源,且释放内存资源。
        public void DisposeALL()
        {
            _CurrentAssetBundle.Unload(true);
        }

        // 查询当前AssetBundle中包含的所有资源名称。
        public string[] RetriveAllAssetName()
        {
            return _CurrentAssetBundle.GetAllAssetNames();
        }
    }
}


