using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MissileManager : MonoBehaviour, Player.MissileShooter
{
    [SerializeField]
    Missile missilePrefab = null;

    public void Fire(Vector3 position, Quaternion rotation, ITarget target, Color color)
    {
        var missile = Missile.Instantiate(missilePrefab, transform);
        missile.Initialize(position, rotation, target,color);
    }
}
