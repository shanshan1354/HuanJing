using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;//跟随目标
    private Vector3 offset;//相机偏移量
    private Vector2 velocity;
    void Update()
    {
        if (target == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            //定义相机偏移量
            offset = transform.position - target.position;
        }
    }

    //相机跟随主角移动
    private void FixedUpdate()
    {
        if (target != null)
        {
            float posX = Mathf.SmoothDamp(transform.position.x, offset.x +
            target.position.x, ref velocity.x, 0.05f);
            float posY = Mathf.SmoothDamp(transform.position.y, offset.y +
                target.position.y, ref velocity.y, 0.05f);
            if (transform.position.y < posY)
            {
                transform.position = new Vector3(posX, posY, transform.position.z);
            }
        }
    }
}
