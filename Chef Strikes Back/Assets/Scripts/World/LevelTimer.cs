using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTime;
    [SerializeField]
    private Light2D worldLight;
    private float lightStartValue;

    [SerializeField]
    private float elapsTime;
    private float elapsTimeStart;
    private TimeSpan timePlaying;

    void Start()
    {
        timePlaying = TimeSpan.FromMinutes(elapsTime);
        elapsTimeStart = elapsTime;
        textTime.text = timePlaying.ToString("mm':'ss'.'ff");
        lightStartValue = worldLight.falloffIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if(elapsTime > 0.00f)
        {
            elapsTime -= Time.deltaTime / 60;
            worldLight.falloffIntensity += (lightStartValue / elapsTimeStart) * Time.deltaTime / 60;

            timePlaying = TimeSpan.FromMinutes(elapsTime);
            textTime.text = timePlaying.ToString("mm':'ss'.'ff");
        }
        else
        {
            
        }
    }
}
