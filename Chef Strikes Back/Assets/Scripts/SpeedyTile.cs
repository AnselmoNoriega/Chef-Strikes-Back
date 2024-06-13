using UnityEngine;
using System.Collections;

public class SpeedyTile : MonoBehaviour
{
    [SerializeField] private float _speedUpParameter;
    private Vector2 _SpeedDirection = new Vector2(0.894427f, 0.447214f);

    private AudioManager _audioManager;
    private bool _isPlayerOnTile = false;

    private void Start()
    {
        // Assuming AudioManager is a singleton or accessed via ServiceLocator
        _audioManager = ServiceLocator.Get<AudioManager>();
    }

    public Vector2 GetSpeed()
    {
        return _SpeedDirection * _speedUpParameter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isPlayerOnTile)
        {
            _isPlayerOnTile = true;
            StartCoroutine(CheckPlayerOnTile());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerOnTile = false;
        }
    }

    private IEnumerator CheckPlayerOnTile()
    {
        _audioManager.PlaySource("Conveyor-Belt");

        while (_isPlayerOnTile)
        {
            yield return null; // Wait for the next frame
        }

        _audioManager.StopSource("Conveyor-Belt");
    }
}
