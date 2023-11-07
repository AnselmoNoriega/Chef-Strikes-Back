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
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Audiocontroller.Play();
        }

       /* if (Input.GetMouseButtonDown(0))
        {
            Audiocontroller.Play();
        }*/

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
 