using UnityEngine;
public class MoveUnit
{
    public Vector3 DestinationPosition { get; private set; }
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get { return velocity;}}

    float count;
    Vector3 velocity;
    Vector3 initializePosition;

    public void Initialize(Vector3 initializePosition)
    {
        this.initializePosition = initializePosition;
    }

    public Vector3 GetMoveCenter(float moveRadius)
    {
        return initializePosition + Vector3.up * moveRadius;
    }

    public void Update(float power, float moveTimeMin, float moveTimeMax, float moveRadius)
    {
        count -= Time.deltaTime;
        if (count <= 0)
        {
            count = Random.Range(moveTimeMin, moveTimeMax);
            var center = GetMoveCenter(moveRadius);
            DestinationPosition = center + Random.insideUnitSphere * moveRadius;

        }

        var p1 = Position;
        var p2 = DestinationPosition;
        Position = Vector3.SmoothDamp(p1, p2, ref this.velocity, power);
    }
}
