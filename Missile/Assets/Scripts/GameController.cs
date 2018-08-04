using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Player player1 = null;

    [SerializeField]
    Player player2 = null;

    [SerializeField]
    MissileManager missileManager = null;


    void Awake()
    {
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        // キーを押して何かする
        // リセット
        if (Input.GetKeyUp(KeyCode.R))
        {
            Initialize();
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Reset"))
        {
            Initialize();
        }

    }

    void Initialize()
    {
        player1.Initialize(player2, missileManager);
        player2.Initialize(player1, missileManager);
    }

}
