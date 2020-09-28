﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW
{
	public class AssetBundleMgr:MonoBehaviour
	{
        //本类实例
        private static AssetBundleMgr _Instance;
        //场景集合
        private Dictionary<string, MultiABMgr> _DicAllScenes = new Dictionary<string, MultiABMgr>();
        //AssetBundle （清单文件） 系统类
        private AssetBundleManifest _ManifestObj = null;


        private  AssetBundleMgr(){}

        //得到本类实例
        public static AssetBundleMgr GetInstance()
        {
            if (_Instance==null)
            {
                _Instance = new GameObject("_AssetBundleMgr").AddComponent<AssetBundleMgr>();
            }
            return _Instance;
        }

        void Awake()
        {
            //加载Manifest清单文件
            StartCoroutine(ABManifestLoader.GetInstance().LoadMainifestFile());
        }


        /// 下载AssetBundel 指定包
        public IEnumerator LoadAssetBundlePack(string scenesName, string abName, DelLoadComplete loadAllCompleteHandle)
        {
            //参数检查
            if (string.IsNullOrEmpty(scenesName) || string.IsNullOrEmpty(abName))
            {
                Debug.LogError(GetType()+ "/LoadAssetBundlePack()/ScenesName Or abName is null ,请检查！");
                yield break;
            }

            //等待Manifest清单文件加载完成
            while (!ABManifestLoader.GetInstance().IsLoadFinish)
            {
                yield return null;
            }
            _ManifestObj = ABManifestLoader.GetInstance().GetABManifest();
            if (_ManifestObj==null)
            {
                Debug.LogError(GetType() + "/LoadAssetBundlePack()/_ManifestObj is null ,请先确保加载Manifest清单文件！");
                yield break;
            }

            //把当前场景加入集合中。
            if (!_DicAllScenes.ContainsKey(scenesName))
            {
                MultiABMgr multiMgrObj = new MultiABMgr(scenesName,abName, loadAllCompleteHandle);
                _DicAllScenes.Add(scenesName, multiMgrObj);
            }

            //调用下一层（“多包管理类”）
            MultiABMgr tmpMultiMgrObj = _DicAllScenes[scenesName];
            if (tmpMultiMgrObj==null)
            {
                Debug.LogError(GetType() + "/LoadAssetBundlePack()/tmpMultiMgrObj is null ,请检查！");
            }
            //调用“多包管理类”的加载指定AB包。
            yield return tmpMultiMgrObj.LoadAssetBundeler(abName);

        }

        public void LoadAssetBundlePackage(string scenesName, string abName, DelLoadComplete loadAllCompleteHandle)
        {
            StartCoroutine(LoadAssetBundlePack(scenesName, abName, loadAllCompleteHandle));
        }

        // 加载(AB 包中)资源
        //isCache是否加载缓存
        public UnityEngine.Object LoadAsset(string scenesName, string abName, string assetName  ,bool isCache)
        {
            if (_DicAllScenes.ContainsKey(scenesName))
            {
                MultiABMgr multObj = _DicAllScenes[scenesName];
                return multObj.LoadAsset(abName, assetName, isCache);
            }
            Debug.LogError(GetType()+ "/LoadAsset()/找不到场景名称，无法加载（AB包中）资源,请检查！  scenesName="+ scenesName);
            return null;
        }

        // 释放资源
        public void DisposeAllAssets(string scenesName)
        {
            if (_DicAllScenes.ContainsKey(scenesName))
            {
                MultiABMgr multObj = _DicAllScenes[scenesName];
                multObj.DisposeAllAsset();
            }
            else {
                Debug.LogError(GetType() + "/DisposeAllAssets()/找不到场景名称，无法释放资源，请检查！  scenesName=" + scenesName);
            }
        }

    }
}


