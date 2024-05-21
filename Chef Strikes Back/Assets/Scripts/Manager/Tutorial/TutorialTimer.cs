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

public class TutorialTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private Light2D worldLight;
    private float lightStartValue;

    [SerializeField] private float elapsedTime;
    private float elapsTimeStart;
    private TimeSpan timePlaying;

    [SerializeField] private SceneControl sceneControl;

    [SerializeField] private List<SpawningTimer> _spawningTimes;

    private bool _shouldRun = true;

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
        if(!_shouldRun)
        {
            return;
        }

        elapsedTime -= Time.deltaTime / 60;
        worldLight.falloffIntensity += (lightStartValue / elapsTimeStart) * Time.deltaTime / 60;

        timePlaying = TimeSpan.FromMinutes(elapsedTime);
        textTime.text = timePlaying.ToString("mm':'ss");

        if (elapsedTime < 0)
        {
            ServiceLocator.Get<GameManager>().SetKillCount(ServiceLocator.Get<Player>().KillCount);
            ServiceLocator.Get<GameManager>().SaveMoney(ServiceLocator.Get<Player>().Money);
            sceneControl.GoToEndScene();
        }
    }

    public void SetTimeState(bool active)
    {
        _shouldRun = active;
    }  

    public bool GetTimeState()
    {
        return _shouldRun;
    }  
}
