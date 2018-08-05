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
    Vector3 velocity;

    [SerializeField]
    Vector3 targetPosition;

    [SerializeField]
    Vector3 targetVelocity;

    [SerializeField]
    float targetPower = 0.6f;

    [SerializeField]
    float positionPower = 0.6f;



    void Update()
    {
        var t = transform;

        {
            var now = targetPosition;
            var to = this.target.position;
            targetPosition = Vector3.SmoothDamp(now, to, ref this.targetVelocity, this.targetPower);
        }

        {
            var now = t.position;
            //var target_p = this.target.position;
            var target_p = targetPosition;
            var player_p = this.player.position;
            var to_target = target_p - player_p;
            var q1 = Quaternion.LookRotation(to_target, Vector3.up);
            var to_p = player_p + q1 * positionOffset;
            //t.position = Vector3.SmoothDamp(now, to_p, ref this.velocity, this.positionPower);
            t.position = to_p;
        }

        t.LookAt(targetPosition);
    }
}
