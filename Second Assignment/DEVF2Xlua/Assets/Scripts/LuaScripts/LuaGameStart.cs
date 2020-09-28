using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuaFramework
{
    public class LuaGameStart : MonoBehaviour
    {
        void Start()
        {
            LuaHelper.GetInstance().DoString("require 'StartGame'");
        }
    }
}