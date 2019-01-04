using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class EnemyCreatePoint : MonoBehaviour {

    [System.SerializableAttribute]
    class EnemyList
    {
        public List<int> List = new List<int>();

        public EnemyList(List<int> list)
        {
            List = list;
        }
    }

    [SerializeField]
    private List<EnemyList> Wave1_EnemyList = new List<EnemyList>();
    [SerializeField]
    private List<EnemyList> Wave2_EnemyList = new List<EnemyList>();
    [SerializeField]
    private List<EnemyList> Wave3_EnemyList = new List<EnemyList>();

    private int NowWave = 0;
    private float NowWaveTime = 0;
    private int EnemyListIndex = 0;

    WaveManager wavemanager;

    // Use this for initialization
    void Start () {
        wavemanager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        EnemyInstantiate(Wave1_EnemyList);
	}
	
	// Update is called once per frame
	void Update () {
        NowWave = wavemanager.GetNowWave();
        NowWaveTime = wavemanager.GetWaveTimer();
	}

    private void EnemyInstantiate(List<EnemyList> NowList)
    {
        if(NowList[EnemyListIndex].List[0] <= NowWaveTime)
        {
            //switch(NowList[EnemyListIndex].List[1])
        }
    }
}
