using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtelierManager : MonoBehaviour {

    private int hp;

	// Use this for initialization
	void Start () {
        hp = 100;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(hp);
	}

    /*
    public void Damage()
    {
        StartCoroutine("AtelierDamage");
    }

    public void DamageStop()
    {
        StopCoroutine("AtelierDamage");
    }

    
    public void Test()
    {
        Debug.Log("Test success");
    }
    */

    public IEnumerator AtelierDamage()
    {
        while (true)
        {
            hp--;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
