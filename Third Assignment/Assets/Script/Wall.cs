using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float friction;
    public float mass;
    public GameObject[] wall;
    private Vector3 pos;
    private Vector3 preV;
    private Vector3 prePos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //检测是否与墙碰撞
        foreach (GameObject wallObj in wall)
        {
            if (wallObj != null)
            {
                Vector3 wallPos = wallObj.transform.position;

                if (Vector3.Distance(pos, wallPos) < 1)
                {
                    Debug.Log("碰到墙了");
                    Vector3 v1 = Vector3.Reflect(preV, wallPos);
                    preV = v1;

                    transform.position = prePos;
                }
            }
        }
    }
}
