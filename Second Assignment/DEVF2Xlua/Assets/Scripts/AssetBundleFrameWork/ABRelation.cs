using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW
{
	public class ABRelation
	{
        //当前AsseetBundel 名称
        private string _ABName;
        //本包所有的依赖包集合
        private List<string> _LisAllDependenceAB;
        //本包所有的引用包集合
        private List<string> _lisAllReferenceAB;

        // 构造函数，abName为AB包名称
        public ABRelation(string abName)
        {
            if (!string.IsNullOrEmpty(abName))
            {
                _ABName = abName;
            }
            _LisAllDependenceAB = new List<string>();
            _lisAllReferenceAB = new List<string>();
        }

        //增加依赖关系
        public void AddDependence(string abName)
        {
            if (!_LisAllDependenceAB.Contains(abName))
            {
                _LisAllDependenceAB.Add(abName);
            }
        }

        //移除依赖关系
        // true；　此AssetBundel 没有依赖项
        // false;  此AssetBundel 还有其他的依赖项
        public bool RemoveDependece(string abName)
        {
            if (_LisAllDependenceAB.Contains(abName))
            {
                _LisAllDependenceAB.Remove(abName);
            }
            if (_LisAllDependenceAB.Count > 0)
            {
                return false;
            }
            else {
                return true;
            }
        }

        //获取所有依赖关系
        public List<string> GetAllDependence()
        {
            return _LisAllDependenceAB;
        }

        // 引用依赖关系
        public void AddReference(string abName)
        {
            if (!_lisAllReferenceAB.Contains(abName))
            {
                _lisAllReferenceAB.Add(abName);
            }
        }

        //移除依赖关系
        public bool RemoveReference(string abName)
        {
            if (_lisAllReferenceAB.Contains(abName))
            {
                _lisAllReferenceAB.Remove(abName);
            }
            if (_lisAllReferenceAB.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //获取所有引用关系
        public List<string> GetAllReference()
        {
            return _lisAllReferenceAB;
        }
    }
}


