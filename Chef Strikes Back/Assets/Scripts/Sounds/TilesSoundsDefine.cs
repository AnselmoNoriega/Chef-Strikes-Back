using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesSoundsDefine : MonoBehaviour
{
    [SerializeField] public string ClipName;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = ServiceLocator.Get<Player>();
        player.soundName = ClipName;
    }
}
