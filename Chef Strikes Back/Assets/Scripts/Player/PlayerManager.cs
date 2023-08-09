using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player playerStats;

    private void Awake()
    {
        playerStats.Awake();
    }
}
