using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCountUpdater : MonoBehaviour {

    private Text waveCount;

    private void Start()
    {
        waveCount = GetComponent<Text>();
        References.GetEnemySpawnerManager().OnNightBeat += OnNightBeat;
        References.GetEnemySpawnerManager().OnNightStart += OnNightStart;
    }

    public void OnNightBeat(int nightBeat)
    {
        waveCount.text = "Next Wave: " + (nightBeat + 2);
    }

    public void OnNightStart(int wave)
    {
        waveCount.text = "Wave: " + (wave + 1);
    }
}
