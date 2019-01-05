using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Target_Player : EnemyBase {

	// Use this for initialization
	void Start () {
        hp = 10;
        speed = 5.0f;
        charaName = "3";
        CreatePoint = SearchEnemyCreatePoint();
    }
	
	// Update is called once per frame
	void Update () {
        MoveToTarget("Player");
	}
}
