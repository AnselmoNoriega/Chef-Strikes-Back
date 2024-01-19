using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering.Universal;
public class LevelTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTime;
    [SerializeField]
    private Light2D worldLight;
    private float lightStartValue;

    [SerializeField]
    private float elapsedTime;
    private float elapsTimeStart;
    private TimeSpan timePlaying;

    [SerializeField] SceneControl sceneControl;

    [Serializable]
    public struct PhaseDefinition
    {
        public int StartTime;
        public int EndTime;
        public int SpawnTime;
    }
    private PhaseDefinition _currentPhase;
    [SerializeField] private List<PhaseDefinition> _phases = new();

    public void Initialize()
    {
        timePlaying = TimeSpan.FromMinutes(elapsedTime);
        elapsTimeStart = elapsedTime;
        textTime.text = timePlaying.ToString("mm':'ss'.'ff");
        lightStartValue = worldLight.falloffIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimeChangeBasedOnTimer();

        elapsedTime -= Time.deltaTime / 60;
        worldLight.falloffIntensity += (lightStartValue / elapsTimeStart) * Time.deltaTime / 60;

        timePlaying = TimeSpan.FromMinutes(elapsedTime);
        textTime.text = timePlaying.ToString("mm':'ss");

        if (elapsedTime < 0)
        {
            sceneControl.GoToEndScene();
        }
    }

    private void SpawnTimeChangeBasedOnTimer()
    {
        var loopManager = ServiceLocator.Get<GameLoopManager>();

        if (elapsedTime <= 3f && elapsedTime > 3.8f)
            loopManager.ChangeSpawnTime(10);
        else if (elapsedTime <= 3.8f && elapsedTime > 3.5f)
            loopManager.ChangeSpawnTime(15);
        else if (elapsedTime <= 3.5f && elapsedTime > 3f)
            loopManager.ChangeSpawnTime(8);
        else if (elapsedTime <= 3f && elapsedTime > 2.5f)
            loopManager.ChangeSpawnTime(10);
        else if (elapsedTime <= 2.5f && elapsedTime > 1.5f)
            loopManager.ChangeSpawnTime(8);
        else if (elapsedTime <= 1.5 && elapsedTime > 0.5f)
            loopManager.ChangeSpawnTime(10);
        else if (elapsedTime <= 0.5f && elapsedTime > 0f)
            loopManager.ChangeSpawnTime(int.MaxValue);
    }


}
