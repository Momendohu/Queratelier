using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Orange : MonoBehaviour {
    //=============================================================
    private float destroyTime = 5;

	//=============================================================
	private void Init(){
	}

	private void Awake () {
		Init();
	}

	private void Start () {
        Destroy(this.gameObject,destroyTime);
	}
	
	private void Update () {
		
	}
}