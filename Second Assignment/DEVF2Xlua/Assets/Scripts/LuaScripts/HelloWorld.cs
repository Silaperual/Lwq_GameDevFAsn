using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLua.LuaEnv luaenv = new XLua.LuaEnv();
        luaenv.DoString("CS.UnityEngine.Debug.Log('这是小刘同学打的Hello World')");
        luaenv.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }
    }
}
