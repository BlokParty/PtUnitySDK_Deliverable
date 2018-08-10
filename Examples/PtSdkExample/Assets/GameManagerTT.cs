using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTable;

public class GameManagerTT : MonoBehaviour {
    public PTPlayer player;
    public Transform playerTransform;

    private void Awake()
    {
        PTTableTop.Initialize(player);

        PTTableTop.OnConnected += (player) =>
        {

        };
    }
}
