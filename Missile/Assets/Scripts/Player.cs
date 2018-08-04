using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ITarget
{
    public Vector3 Position { get { return transform.position; } }
    public Vector3 Velocity { get { return moveUnit.Velocity; } }

    public interface MissileShooter
    {
        void Fire(Vector3 position, Quaternion rotation, ITarget target, Color color);
    }

    [SerializeField]
    Vector3 initialPosition;

    [SerializeField]
    Quaternion initialRotation;

    [SerializeField]
    float power;

    [SerializeField]
    float moveTimeMin = 0.5f;

    [SerializeField]
    float moveTimeMax = 1.0f;

    [SerializeField]
    float moveRadius = 10;

    MoveUnit moveUnit = new MoveUnit();
    ITarget target;

    int fireCount;
    int fireNumber = 5;
    float fireIntervalCount;
    float fireIntervalTime = 0.2f;
    float fireWaitCount;
    float fireWaitTime = 3f;
    MissileShooter missileShooter;

    Color color;

    public void Initialize(ITarget target, MissileShooter missileShooter)
    {
        this.target = target;
        this.missileShooter = missileShooter;
        var t = transform;
        this.initialPosition = t.position;
        this.initialRotation = t.rotation;
        t.SetPositionAndRotation(initialPosition, initialRotation);
        this.moveUnit.Initialize(initialPosition);

        this.color = GetComponent<Renderer>().material.color;

    }


    void Fire()
    {
        var t = transform;
        this.missileShooter.Fire(t.position, t.rotation, this.target, this.color);

    }

    void Update()
    {
        UpdateMove();
        UpdateFire();
    }

    void UpdateMove()
    {
        moveUnit.Update(this.power, this.moveTimeMin, this.moveTimeMax, this.moveRadius);
        transform.position = moveUnit.Position;

        transform.LookAt(this.target.Position, Vector3.up);
    }

    void UpdateFire()
    {
        // 休憩
        var deltaTime = Time.deltaTime;
        fireWaitCount -= deltaTime;
        if (0 <= fireWaitCount)
        {
            return;
        }

        // 連射間隔
        fireIntervalCount -= deltaTime;
        if (0 <= fireIntervalCount)
        {
            return;
        }
        fireIntervalCount = fireIntervalTime;

        // 発射個数
        Fire();
        fireCount--;

        if (fireCount <= 0)
        {
            fireWaitCount = fireWaitTime;
            fireCount = fireNumber;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, moveRadius);
        Gizmos.DrawWireCube(this.moveUnit.DestinationPosition, Vector3.one);
    }
}
