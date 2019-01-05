using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Target_Double : EnemyBase{

    public GameObject player;
    private float distance;

	// Use this for initialization
	void Start () {
        hp = 10;
        speed = 5.0f;
        charaName = "2";

        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        distance = (transform.position - player.transform.position).sqrMagnitude;

        if(distance < 50)
        {
            MoveToTarget("Player");
        }
        else
        {
            MoveToTarget("Atelier");
        }
	}
}
