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
    //[SerializeField]
    //private List<EnemyList> Wave2_EnemyList = new List<EnemyList>();
    //[SerializeField]
    //private List<EnemyList> Wave3_EnemyList = new List<EnemyList>();

    [SerializeField]
    private int[,] array;

    private int NowWave = 0;

    // Use this for initialization
    void Start () {
        EnemyInstantiate(Wave1_EnemyList);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void EnemyInstantiate(List<EnemyList> NowList)
    {
        Debug.Log(NowList[0].List[0]);
    }
}
