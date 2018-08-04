using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    GameObject explosionPrefab;

    [SerializeField]
    Vector3 velocity;
    [SerializeField]
    float power;
    [SerializeField]
    float speed;
    Vector3 lastPosition;

    [SerializeField]
    float lifeTime = 10;
    [SerializeField]
    float velocityLimit = 10;

    ITarget target;

    /// <summary>
    /// 回転速度(degree/second)
    /// </summary>
    [SerializeField]
    float rotateSpeed;
    [SerializeField]
    float rotateRatio = 0.02f;
    Color color;

    public void Initialize(Vector3 position, Quaternion rotation, ITarget target, Color color)
    {
        this.target = target;
        this.lastPosition = position;
        this.transform.SetPositionAndRotation(position, rotation);
        GetComponent<Renderer>().material.color = color;
        this.color = color;
    }

    void Update()
    {
        UpdateLife();
        UpdateRotation();
        UpdatePosition();
        CheckHit();
    }

    void UpdateLife()
    {
        if (lifeTime < 0)
        {
            Die();
        }
    }

    void UpdatePosition()
    {
        var t = this.transform;
        speed += power * Time.deltaTime;

        var direction = t.rotation * Vector3.forward;
        // Debug.DrawLine(t.position, t.position + direction * 10, Color.red);

        velocity = direction * speed;
        velocity = Vector3.ClampMagnitude(velocity, velocityLimit);
        var step = Vector3.ClampMagnitude(velocity * Time.deltaTime, 1.0f);
        lastPosition = transform.position;
        var position = lastPosition + step;

        transform.position = position;
    }


    void UpdateRotation()
    {
        // UpdateRotation1();
        // UpdateRotation2();
        UpdateRotation3();

    }


    /// <summary>
    // 球面補間は割合で指定する。
    //  var r = Quaternion.Slerp(mr, r1, rotateRatio);
    // 自分とターゲットのなす角によって回転量が変わってしまう。
    // ミサイルの能力として回転速度を指定したい。(degree/sec)。
    /// </summary>
    void UpdateRotation3()
    {
        var t = transform;
        var mp = t.position;
        var tp = target.Position;
        //Debug.DrawLine(mp, tp);
        var v1 = tp - mp;
        var from = t.rotation;
        var to = Quaternion.LookRotation(v1, Vector3.up);

        //var ratio = CalcRotateRatio1(from, to, rotateSpeed);
        var ratio = CalcRotateRatio2(from, to, rotateSpeed, this.speed);
        var r = Quaternion.Slerp(from, to, ratio);
        transform.rotation = r;
    }

    //
    // ミサイルは最初速度0から始めていて、その時も回転しているが、加速するまではあまり加速してほしくない。
    // 回転角度の計算パラメータに速度を追加する。
    //
    static float CalcRotateRatio2(Quaternion from, Quaternion to, float degreeSpeed, float speed)
    {
        var r1 = degreeSpeed * Time.deltaTime;
        var r2 = Quaternion.Angle(from, to);
        var ratio = 0.0f;

        if (r2 < Mathf.Epsilon || r2 <= r1)
        {
            ratio = 1.0f;
        }
        else
        {
            ratio = r1 / r2;
        }

        var adjusted = ratio * (speed / 20.0f);
        return adjusted;
    }

    // 目標角度と現在角度と回転能力から回転する割合を計算する。
    static float CalcRotateRatio1(Quaternion from, Quaternion to, float degreeSpeed)
    {
        var r1 = degreeSpeed * Time.deltaTime;
        var r2 = Quaternion.Angle(from, to);
        var ratio = 0.0f;

        if (r2 < Mathf.Epsilon || r2 <= r1)
        {
            ratio = 1.0f;
        }
        else
        {
            ratio = r1 / r2;
        }
        return ratio;
    }


    /// <summary>
    /// 徐々にその方向へ向かわせる。
    /// 球面補間を使用する。
    /// </summary>
    void UpdateRotation2()
    {
        var mp = transform.position;
        var tp = target.Position;
        //Debug.DrawLine(mp, tp);
        var v1 = tp - mp;
        var r1 = Quaternion.LookRotation(v1, Vector3.up);


        // すこしずつ回転する。
        var mr = transform.rotation;
        var r = Quaternion.Slerp(mr, r1, rotateRatio);


        transform.rotation = r;
    }

    /// <summary>
    /// ターゲットのポジションと、自分のポジションのベクトルを作る。
    /// その方向に向けたクォータニオンを作る。
    /// セットする。
    /// 即座にターゲットに向いて、飛んでいく。
    /// </summary>
    void UpdateRotation1()
    {
        var mp = transform.position;
        var tp = target.Position;
        //Debug.DrawLine(mp, tp);
        var v1 = tp - mp;
        var r = Quaternion.LookRotation(v1, Vector3.up);
        transform.rotation = r;
    }

    void CheckHit()
    {
        //CheckHit1();
        //CheckHit2();
        CheckHit3();
    }

    // RigidBodyを使わないで CheckBox を使って当たり判定をする。
    void CheckHit1()
    {
        var t = transform;
        var isHit = Physics.CheckBox(center: t.position, halfExtents: transform.localScale, orientation: t.rotation);
        if (isHit)
        {
            Explosion(t.position);
        }
    }

    // CheckBox -> BoxCast にして衝突時の詳細データを取得する
    void CheckHit2()
    {
        var t = transform;
        RaycastHit hitInfo;
        var direction = t.position - lastPosition;
        var length = direction.magnitude;
        var isHit = Physics.BoxCast(lastPosition, transform.localScale, direction, out hitInfo, t.rotation, length);
        Debug.DrawRay(lastPosition, direction, this.color, 1.0f);
        if (isHit)
        {
            Explosion(hitInfo.point);
        }
    }

    // 爆発が真ん中に来るように最近点を計算
    void CheckHit3()
    {
        var t = transform;
        RaycastHit hitInfo;
        var direction = t.position - lastPosition;
        var length = direction.magnitude;
        var isHit = Physics.BoxCast(lastPosition, transform.localScale, direction, out hitInfo, t.rotation, length);
        Debug.DrawRay(lastPosition, direction, this.color, 1.0f);
        if (isHit)
        {
            var point = hitInfo.collider.ClosestPoint(lastPosition);
            Explosion(point);
        }
    }


    void Explosion(Vector3 center)
    {
        var go = Instantiate(explosionPrefab);
        var t = this.transform;
        go.transform.position = center;

        Die();
    }


    void Die()
    {
        GameObject.Destroy(gameObject);
    }
}
