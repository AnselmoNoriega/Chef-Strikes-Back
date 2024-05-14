using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesSoundsDefine : MonoBehaviour
{
    [SerializeField] private string ClipName;
    [SerializeField] private GameObject player; // Reference to the player GameObject
    private bool isPlayingFootsteps = false;
    private Coroutine footstepsCoroutine;
    private Rigidbody2D playerRb;

    private void Start()
    {
        // Cache the Rigidbody2D component
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (playerRb != null && IsPlayerMoving())
        {
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
            audioManager.PlaySource(ClipName);
            yield return new WaitForSeconds(1f);  // Adjust the interval as needed for your footstep sounds
        }
    }
}
