using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        for (var f = 0; f <= 360; f += 10)
        {
            var q = Quaternion.AngleAxis(f, new Vector3(1, 0, 0));
            Debug.LogFormat("{0} {1}", f, q);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
