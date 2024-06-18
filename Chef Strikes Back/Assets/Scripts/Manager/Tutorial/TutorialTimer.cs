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
    [SerializeField] private RectTransform[] clockHands;
    [SerializeField] private Light2D worldLight;
    private float lightStartValue;

    [SerializeField] private float elapsedTime;
    private float elapsTimeStart;
    private TimeSpan timePlaying;

    [SerializeField] private SceneControl sceneControl;

    [SerializeField] private List<SpawningTimer> _spawningTimes;

    private bool _shouldRun = true;
    private Vector3[] _rotationTime = new Vector3[2];

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
        elapsTimeStart = elapsedTime;
        lightStartValue = worldLight.falloffIntensity;
        _rotationTime[(int)ClockHands.SmallHand] = new Vector3(0.0f, 0.0f, 360 / (elapsTimeStart * 60));
        _rotationTime[(int)ClockHands.BigHand] = new Vector3(0.0f, 0.0f, 360 / (elapsTimeStart / 12 * 60));
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimeChangeBasedOnTimer();

        if (!_shouldRun)
        {
            return;
        }
        elapsedTime -= Time.deltaTime / 60;
        worldLight.falloffIntensity += (lightStartValue / elapsTimeStart) * Time.deltaTime / 60;

        clockHands[(int)ClockHands.SmallHand].Rotate(_rotationTime[(int)ClockHands.SmallHand] * Time.deltaTime);
        clockHands[(int)ClockHands.BigHand].Rotate(_rotationTime[(int)ClockHands.BigHand] * Time.deltaTime);

        if (elapsedTime < 0)
        {
            ServiceLocator.Get<GameManager>().SetKillCount(ServiceLocator.Get<Player>().KillCount);
            ServiceLocator.Get<GameManager>().SaveMoney(ServiceLocator.Get<Player>().Money);
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

    public void SetTimeState(bool active)
    {
        _shouldRun = active;
    }  

    public bool GetTimeState()
    {
        return _shouldRun;
    }  
}
