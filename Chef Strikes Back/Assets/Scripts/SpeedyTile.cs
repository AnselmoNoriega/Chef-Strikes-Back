using UnityEngine;

public class SpeedyTile : MonoBehaviour
{
    [SerializeField] private float _speedUpParameter;
    private Vector2 _SpeedDirection = new Vector2(0.894427f, 0.447214f);

    public void EnterTreadmill()
    {
        ServiceLocator.Get<Player>().FloorSpeed = _SpeedDirection * _speedUpParameter;
    }
}
