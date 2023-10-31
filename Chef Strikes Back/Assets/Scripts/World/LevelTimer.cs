using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private float lightStartValue;
    [SerializeField]    private float elapsTime;

    Object worldLight;

    private float elapsTimeStart;
    private TimeSpan timePlaying;

    //kingston - 9/10
    [SerializeField] SceneControl sceneControl;
    void Start()
    {
        timePlaying = TimeSpan.FromMinutes(elapsTime);
        elapsTimeStart = elapsTime;
        textTime.text = timePlaying.ToString("mm':'ss'.'ff");
        //lightStartValue = worldLight.falloffIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsTime > 0.00f)
        {
            elapsTime -= Time.deltaTime / 60;
            //worldLight.falloffIntensity += (lightStartValue / elapsTimeStart) * Time.deltaTime / 60;

            timePlaying = TimeSpan.FromMinutes(elapsTime);
            textTime.text = timePlaying.ToString("mm':'ss'.'ff");
        }
        else if (elapsTime < 0)
        {
            // scene switching when time is up.
            // kingston 9/10
            sceneControl.switchToGameOverScene();
        }
    }
}
