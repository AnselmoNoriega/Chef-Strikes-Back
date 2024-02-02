using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _screens;
    [SerializeField] private StatefulObject _statefulObj;

    public void SetScreenActive(string screenName)
    {
        _statefulObj.SetState(screenName);
    }
}
