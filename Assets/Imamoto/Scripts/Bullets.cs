using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour {

    public GameObject Bullet;
    private float time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time > 5) Destroy(this.gameObject);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") Destroy(this.gameObject);
    }
}
