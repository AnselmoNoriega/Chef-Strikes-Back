using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesSoundsDefine : MonoBehaviour
{
    [SerializeField] private string clipName;
    private Player _player;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _player = ServiceLocator.Get<Player>();
            if (_player.FloorSoundName != clipName)
            {
                ServiceLocator.Get<AudioManager>().StopSource(_player.FloorSoundName);
                _player.FloorSoundName = clipName;
                if (_player.IsWalking)
                {
                    ServiceLocator.Get<AudioManager>().PlaySource(_player.FloorSoundName);
                }
            }
        }
    }
}
