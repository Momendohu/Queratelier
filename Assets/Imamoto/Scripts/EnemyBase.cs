using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour {

    public int hp;
    public float speed;
    public string charaName;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //ターゲットのタグが引数
    public void MoveToTarget(string target_name)
    {
        GameObject target = GameObject.FindGameObjectWithTag(target_name) as GameObject;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        agent.destination = target.transform.position;
    }

    //要改良
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            hp--;
        }

        if (hp <= 0) Destroy(this.gameObject);
    }
}
