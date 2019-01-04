using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    private int NowWave = 0;
    private float WaveTime = 0;

    public int[] Wave_EnemySum = new int[3];

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        WaveTimer();
	}

    private void WaveTimer()
    {
        WaveTime += Time.deltaTime;
    }

    public float GetWaveTimer()
    {
        return WaveTime;
    }

    public int GetNowWave()
    {
        return NowWave;
    }
}
