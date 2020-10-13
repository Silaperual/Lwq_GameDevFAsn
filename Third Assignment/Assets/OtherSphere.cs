using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSphere : MonoBehaviour
{
    public float friction;
    public float mass;
    public float radius;

    [NonSerialized]
    public Vector3 currentV;
    public Vector3 curV;

    // Start is called before the first frame update
    void Start()
    {
        curV = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        //摩擦力
        Vector3 frictionDeltaV = -Time.deltaTime * friction * currentV.normalized;
        //防止摩擦力反向运动
        Vector3 finalV = currentV + frictionDeltaV;
        if (finalV.x * currentV.x <= 0)
            frictionDeltaV.x = -currentV.x;
        if (finalV.y * currentV.y <= 0)
            frictionDeltaV.y = -currentV.y;
        if (finalV.z * currentV.z <= 0)
            frictionDeltaV.z = -currentV.z;

        //应用加速度
        curV = currentV + frictionDeltaV;
        transform.Translate((curV + currentV) * Time.deltaTime / 2);
        currentV = curV;
        currentV.y = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            Vector3 v1 = currentV;
            ContactPoint contactPoint = collision.contacts[0];
            Vector3 newDir = curV;
            newDir = Vector3.Reflect(currentV, contactPoint.normal);
            currentV = newDir;
            Debug.Log("大球碰到了");
            //Quaternion rotation = Quaternion.FromToRotation(preV, newDir);

        }
        currentV.y = 0;
    }
}
