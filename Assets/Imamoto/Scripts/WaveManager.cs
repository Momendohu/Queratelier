using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    private int NowWave = 0;
    private float WaveTime = 0;
    private int Wave_EnemySumIndex = 0;
    private int DestroyEnemySum = 0;

    public int[] Wave_EnemySum = new int[3];

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        WaveTimer();

        if (Wave_EnemySum[Wave_EnemySumIndex] <= DestroyEnemySum && Wave_EnemySumIndex <= 1)
        {
            DestroyEnemySum = 0;
            NowWave++;
            WaveTime = 0;
            Wave_EnemySumIndex++;
            Debug.Log(NowWave);
        }
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

    public void ReciveDestroyEnemySum()
    {
        DestroyEnemySum++;
    }
}
