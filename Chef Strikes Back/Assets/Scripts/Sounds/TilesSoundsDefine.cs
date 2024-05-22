using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Tile,
    Wood,
    Brick
}
public class TilesSoundsDefine : MonoBehaviour
{
    [SerializeField] private string tileClipName;
    [SerializeField] private string woodClipName;
    [SerializeField] private string brickClipName;
    [SerializeField] private GameObject player; // Reference to the player GameObject
    private bool isPlayingFootsteps = false;
    private Coroutine footstepsCoroutine;
    private Rigidbody2D playerRb;
    private string currentClipName;

    private void Start()
    {
        // Cache the Rigidbody2D component
        playerRb = player.GetComponent<Rigidbody2D>();
        currentClipName = tileClipName; // Default to tile sound
    }

    private void Update()
    {
        if (playerRb != null && IsPlayerMoving())
        {
            UpdateCurrentClipName();
            StartSound();
        }
        else
        {
            StopSound();
        }
    }

    private bool IsPlayerMoving()
    {
        // Check if the player is moving
        return Mathf.Abs(playerRb.velocity.x) > 0.1f || Mathf.Abs(playerRb.velocity.y) > 0.1f;
    }

    private void UpdateCurrentClipName()
    {
        Vector2 playerPosition = player.transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, 0.2f); // Slightly increased radius
        bool tilemapFound = false;

        foreach (var collider in colliders)
        {
            TilemapType tilemapType = collider.GetComponent<TilemapType>();
            if (tilemapType != null)
            {
                switch (tilemapType.tileType)
                {
                    case TileType.Tile:
                        currentClipName = tileClipName;
                        break;
                    case TileType.Wood:
                        currentClipName = woodClipName;
                        Debug.Log("Wood tile detected.");
                        break;
                    case TileType.Brick:
                        currentClipName = brickClipName;
                        break;
                }
                tilemapFound = true;
                Debug.Log("Tile type detected: " + tilemapType.tileType.ToString());
                break; // Exit loop once we've found a tilemap type
            }
            else
            {
                Debug.Log("Collider found but no TilemapType component: " + collider.name);
            }
        }

        if (!tilemapFound)
        {
            Debug.LogWarning("No tilemap detected under the player.");
        }
    }

    private void StartSound()
    {
        if (!isPlayingFootsteps)
        {
            isPlayingFootsteps = true;
            footstepsCoroutine = StartCoroutine(PlaySounds());
        }
    }

    private void StopSound()
    {
        if (isPlayingFootsteps)
        {
            isPlayingFootsteps = false;
            if (footstepsCoroutine != null)
            {
                StopCoroutine(footstepsCoroutine);
                footstepsCoroutine = null;
            }
        }
    }

    private IEnumerator PlaySounds()
    {
        var audioManager = ServiceLocator.Get<AudioManager>();
        while (isPlayingFootsteps)
        {
            audioManager.PlaySource(currentClipName);
            yield return new WaitForSeconds(1f);  // Adjust the interval as needed for your footstep sounds
        }
    }
}
