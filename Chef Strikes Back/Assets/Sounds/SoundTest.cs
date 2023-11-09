using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public AudioSource Audiocontroller;
    public AudioSource BackgroundMusic;
    public AudioSource Footsteps;

    void Update()
    {
        

        // If you want to stop the winding up sound when the right mouse button is not held down,
        // you can add this condition
        

        // The rest of your code for other inputs (W, A, S, D) remains unchanged
        if (Input.GetKeyDown(KeyCode.W))
        {
            Footsteps.Play();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Footsteps.Play();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Footsteps.Play();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Footsteps.Play();
        }
    }

    private void Start()
    {
        BackgroundMusic.Play();
    }
}
 