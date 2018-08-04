using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{

    [SerializeField]
    Transform player;
    [SerializeField]
    Transform target;

    [SerializeField]
    Vector3 positionOffset;


    [SerializeField]
    Vector3 targetPosition;
    [SerializeField]
    Vector3 targetVelocity;
    [SerializeField]
    float power = 0.1f;



    void Update()
    {
        var t = transform;
        {
            t.position = player.TransformPoint(this.positionOffset);
        }

        {
            var p1 = targetPosition;
            var p2 = this.target.position;
            targetPosition = Vector3.SmoothDamp(p1, p2, ref this.targetVelocity, this.power);
            t.LookAt(targetPosition);
        }
    }
}
