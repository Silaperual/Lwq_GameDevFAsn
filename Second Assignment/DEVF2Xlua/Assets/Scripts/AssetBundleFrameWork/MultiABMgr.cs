using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW
{
	public class MultiABMgr
	{
        //（下层）引用类： "单个AB包加载实现类"
        private SingleABLoader _CurrentSinglgABLoader;
        //“AB包实现类”缓存集合（作用： 缓存AB包，防止重复加载，即： “AB包缓存集合”）
        private Dictionary<string, SingleABLoader> _DicSingleABLoaderCache;
        //当前场景(调试使用)
        private string _CurrentScenesName;
        //当前AssetBundle 名称
        private string _CurrentABName;
        //AB包与对应依赖关系集合
        private Dictionary<string, ABRelation> _DicABRelation;
        //委托： 所有AB包加载完成。
        private DelLoadComplete _LoadAllABPackageCompleteHandel;

        // 构造函数
        // loadAllABPackCompleteHandle:（委托）是否调用完成
        public MultiABMgr(string scenesName,string abName,DelLoadComplete loadAllABPackCompleteHandle)
        {
            _CurrentScenesName = scenesName;
            _CurrentABName = abName;
            _DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            _DicABRelation = new Dictionary<string, ABRelation>();
            //委托
            _LoadAllABPackageCompleteHandel = loadAllABPackCompleteHandle;
        }

        // 完成指定AB包调用
        private void CompleteLoadAB(string abName)
        {
            Debug.Log(GetType() + abName + " loading successful");
            if (abName.Equals(_CurrentABName))
            {
                if (_LoadAllABPackageCompleteHandel!=null)
                {
                    _LoadAllABPackageCompleteHandel(abName);
                }
            }
        }

        // 加载AB包
        public IEnumerator LoadAssetBundeler(string abName)
        {
            //AB包关系的建立
            if (!_DicABRelation.ContainsKey(abName))
            {
                ABRelation abRelationObj = new ABRelation(abName);
                _DicABRelation.Add(abName, abRelationObj);
            }
            ABRelation tmpABRelationObj = _DicABRelation[abName];

            //得到指定AB包所有的依赖关系（查询Manifest清单文件）
            string[] strDependeceArray = ABManifestLoader.GetInstance().RetrivalDependce(abName);
            foreach (string item_Depence in strDependeceArray)
            {
                //添加“依赖”项
                tmpABRelationObj.AddDependence(item_Depence);
                //添加“引用”项    （递归调用）
                yield return LoadReference(item_Depence, abName);
            }

            //真正加载AB包
            if (_DicSingleABLoaderCache.ContainsKey(abName))
            {
                yield return _DicSingleABLoaderCache[abName].LoadAssetBundle();
            }
            else {
                _CurrentSinglgABLoader = new SingleABLoader(abName, CompleteLoadAB);
                _DicSingleABLoaderCache.Add(abName, _CurrentSinglgABLoader);
                yield return _CurrentSinglgABLoader.LoadAssetBundle();
            }

        }//Method_end

        // 加载引用AB包
        private IEnumerator LoadReference(string abName,string refABName)
        {
            //AB包已经加载
            if (_DicABRelation.ContainsKey(abName))
            {
                ABRelation tmpABRelationObj = _DicABRelation[abName];
                //添加AB包引用关系（被依赖）
                tmpABRelationObj.AddReference(refABName);
            }
            else {
                ABRelation tmpABRelationObj = new ABRelation(abName);
                tmpABRelationObj.AddReference(refABName);
                _DicABRelation.Add(abName, tmpABRelationObj);

                //开始加载依赖的包(这是一个递归调用)
                yield return LoadAssetBundeler(abName);
            }
        }

        // 加载（AB包中）资源
        public UnityEngine.Object LoadAsset(string abName, string assetName, bool isCache)
        {
            foreach (string item_abName in _DicSingleABLoaderCache.Keys)
            {
                if (abName== item_abName)
                {
                    return _DicSingleABLoaderCache[item_abName].LoadAsset(assetName, isCache);
                }
            }
            Debug.LogError(GetType()+ "Could not load ab: "+ abName+ " assetName: "+ assetName);
            return null;
        }

        // 释放本场景中所有的资源
        public void DisposeAllAsset()
        {
            try
            {
                //逐一释放所有加载过的AssetBundel 包中的资源
                foreach (SingleABLoader item_sABLoader in _DicSingleABLoaderCache.Values)
                {
                    item_sABLoader.DisposeALL();
                }
            }
            finally
            {
                _DicSingleABLoaderCache.Clear();
                _DicSingleABLoaderCache = null;

                //释放其他对象占用资源
                _DicABRelation.Clear();
                _DicABRelation = null;
                _CurrentABName = null;
                _CurrentScenesName = null;
                _LoadAllABPackageCompleteHandel = null;

                //卸载没有使用到的资源
                Resources.UnloadUnusedAssets();
                //强制垃圾收集
                System.GC.Collect();
            }

        }


    }
}


