using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public struct SpawningTimer
{
    public float Time;
    public int SpawningTime;
}

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private Light2D worldLight;
    private float lightStartValue;

    [SerializeField] private float elapsedTime;
    private float elapsTimeStart;
    private TimeSpan timePlaying;

    [SerializeField] private SceneControl sceneControl;

    [SerializeField] private List<SpawningTimer> _spawningTimes;

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
            ServiceLocator.Get<GameManager>().SaveMoney();
            sceneControl.GoToEndScene();
        }
    }

    private void SpawnTimeChangeBasedOnTimer()
    {
        float time;
        for (int i = 0; i < _spawningTimes.Count; i++)
        {
            time = (elapsTimeStart * 60) - _spawningTimes[_spawningTimes.Count - 1 - i].Time;
            if (time >= (elapsedTime * 60))
            {
                var loopManager = ServiceLocator.Get<GameLoopManager>();
                loopManager.ChangeSpawnTime(_spawningTimes[_spawningTimes.Count - 1 - i].SpawningTime);
                return;
            }
        }
    }

    //60-0>59.999
    //60 - 5 > 54.999 
}
