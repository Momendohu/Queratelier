using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Test : MonoBehaviour {

    private int hp;

	// Use this for initialization
	void Start () {
        hp = 10;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            hp--;
        }

        if (hp <= 0) Destroy(this.gameObject);
    }
}
