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

    public GameObject Andy;
    public GameObject Bob;
    public GameObject Mac;
    public GameObject Cacy;
    public GameObject Helen;

    WaveManager wavemanager;

    // Use this for initialization
    void Start () {
        wavemanager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
	}
	
	// Update is called once per frame
	void Update () {
        NowWave = wavemanager.GetNowWave();
        NowWaveTime = wavemanager.GetWaveTimer();

        //EnemyInstantiate(Wave1_EnemyList);
    }

    private void EnemyInstantiate(List<EnemyList> NowList)
    {
        if(NowList[EnemyListIndex].List[0] <= NowWaveTime)
        {
            switch (NowList[EnemyListIndex].List[1])
            {
                case 0:
                    for (int i = 0; i < NowList[EnemyListIndex].List[2]; i++)
                    {
                        Instantiate(Andy, this.transform.position, Quaternion.identity);
                    }
                    break;

                case 1:
                    for (int i = 0; i < NowList[EnemyListIndex].List[2]; i++)
                    {
                        Instantiate(Bob, this.transform.position, Quaternion.identity);
                    }
                    break;

                case 2:
                    for (int i = 0; i < NowList[EnemyListIndex].List[2]; i++)
                    {
                        Instantiate(Mac, this.transform.position,Quaternion.identity);
                    }
                    break;

                case 3:
                    break;

                case 4:
                    break;

                case -1:
                    Debug.Log("エラー　未定義のエネミー呼び出し");
                    break;
            }
            if (NowList.Count >= EnemyListIndex + 1)
            {
                EnemyListIndex++;
            }
        }
        Debug.Log(EnemyListIndex);
        Debug.Log(NowList.Count);
    }
}
