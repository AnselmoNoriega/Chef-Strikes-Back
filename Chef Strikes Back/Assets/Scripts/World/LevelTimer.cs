using System;
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

    [SerializeField] SceneControl sceneControl;

    public void Initialize()
    {
        timePlaying = TimeSpan.FromMinutes(elapsTime);
        elapsTimeStart = elapsTime;
        textTime.text = timePlaying.ToString("mm':'ss'.'ff");
        lightStartValue = worldLight.falloffIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimeChangeBasedOnTimer();
        if (elapsTime > 0.00f)
        {
            elapsTime -= Time.deltaTime / 60;
            worldLight.falloffIntensity += (lightStartValue / elapsTimeStart) * Time.deltaTime / 60;

            timePlaying = TimeSpan.FromMinutes(elapsTime);
            textTime.text = timePlaying.ToString("mm':'ss'.'ff");
        }
        else if (elapsTime < 0)
        {
            sceneControl.switchToGameOverScene();
        }
    }

    private void SpawnTimeChangeBasedOnTimer()
    {
        if (elapsTime <= 5f && elapsTime > 4.9f) 
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(5);
        else if (elapsTime <= 4.9f && elapsTime > 4.5f)
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(15);
        else if (elapsTime <= 4.5f && elapsTime > 3f)
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(10);
        else if (elapsTime <= 3.5f && elapsTime > 3f) 
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(5);
        else if (elapsTime <= 3f && elapsTime > 2.5f) 
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(10);
        else if (elapsTime <= 2.5f && elapsTime > 2f) 
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(10);
        else if (elapsTime <= 2f && elapsTime > 1.5f) 
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(10);
        else if (elapsTime <= 1.5 && elapsTime > 0.5f) 
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(5);
        else if (elapsTime <= 0.5f && elapsTime > 0f)
            ServiceLocator.Get<GameLoopManager>().ChangeSpawnTime(100000);
    }
}
