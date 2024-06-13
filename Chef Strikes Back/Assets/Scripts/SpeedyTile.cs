using UnityEngine;
using System.Collections;

public class SpeedyTile : MonoBehaviour
{
    [SerializeField] private float _speedUpParameter;
    private Vector2 _SpeedDirection = new Vector2(0.894427f, 0.447214f);

    

    public Vector2 GetSpeed()
    {
        return _SpeedDirection * _speedUpParameter;
    }

}
