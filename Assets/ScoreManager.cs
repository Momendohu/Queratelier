using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private int[] ItemSum = new int[4];

	// Use this for initialization
	void Start () {
        ItemSum = new int[] { 0, 0, 0, 0 };
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(ItemSum[0] + " " + ItemSum[1] + " " + ItemSum[2] + " " + ItemSum[3]);
	}

    public void ReciveItem(int ItemId)
    {
        ItemSum[ItemId]++;
    }
}
