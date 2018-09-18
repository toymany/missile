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

        // ターゲットの位置を少し遅れてついていく
        {
            var now = this.targetPosition;
            var to = this.target.position;
            this.targetPosition = Vector3.SmoothDamp(now, to, ref this.targetVelocity, this.targetPower);
        }

        // プレイヤーの位置とターゲットの位置からカメラの位置を決める
        {
            var now = t.position;
            var target_p = this.targetPosition;
            var player_p = this.player.position;
            var to_target = target_p - player_p;
            var q1 = Quaternion.LookRotation(to_target, Vector3.up);
            var to_p = player_p + q1 * this.positionOffset;
            t.position = to_p;
        }

        t.LookAt(this.targetPosition);
    }
}
