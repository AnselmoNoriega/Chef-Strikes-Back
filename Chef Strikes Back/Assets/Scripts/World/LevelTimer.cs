using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTime;

    [SerializeField]
    private float elapsTime;
    private TimeSpan timePlaying;

    void Start()
    {
        timePlaying = TimeSpan.FromMinutes(elapsTime);
        textTime.text = timePlaying.ToString("mm':'ss'.'ff");
    }

    // Update is called once per frame
    void Update()
    {
        if(elapsTime > 0.00f)
        {
            elapsTime -= Time.deltaTime / 60;

            timePlaying = TimeSpan.FromMinutes(elapsTime);
            textTime.text = timePlaying.ToString("mm':'ss'.'ff");
        }
        else
        {
            
        }
    }
}
