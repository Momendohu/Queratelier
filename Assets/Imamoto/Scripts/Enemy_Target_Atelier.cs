using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Target_Atelier : EnemyBase {

	// Use this for initialization
	void Start () {
        hp = 10;
        speed = 5.0f;
        charaName = "1";
	}
	
	// Update is called once per frame
	void Update () {
        MoveToTarget("Atelier");
	}


}
