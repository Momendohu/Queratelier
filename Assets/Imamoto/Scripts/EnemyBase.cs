using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//敵ベーススクリプト
//敵スクリプト作成時は、このスクリプトを継承させること
public class EnemyBase : MonoBehaviour {

    public int hp;
    public float speed;
    public string charaName;
    private bool NowAttack = false;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //ターゲットを追いかける
    //ターゲットのタグが引数
    public void MoveToTarget(string target_name)
    {
        GameObject target = GameObject.FindGameObjectWithTag(target_name) as GameObject;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        agent.destination = target.transform.position;
    }

    //要改良
    //弾との衝突時
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            hp--;
        }

        if (hp <= 0) Destroy(this.gameObject);
    }

    //衝突判定
    protected void OnCollisionStay(Collision collision)
    {
        GameObject Atelier = GameObject.FindGameObjectWithTag("Atelier");
        AtelierManager atelier = Atelier.GetComponent<AtelierManager>();

        //Debug.Log(collision.gameObject.tag);
        //Debug.Log((this.transform.position-Atelier.transform.position).magnitude);

        if (collision.gameObject.tag == "Atelier")
        {
            //Debug.Log("collision");
            if (!NowAttack)
            {
                //atelier.Damage();
                atelier.StartCoroutine("AtelierDamage");
                NowAttack = true;
            }
            //atelier.Test();
        }

        //アトリエに接触せず、一定の距離離れている場合のみ
        if (collision.gameObject.tag != "Atelier" && (this.transform.position - Atelier.transform.position).magnitude >= 3.0f)
        {
            if (NowAttack)
            {
                //Debug.Log("test");
                //atelier.DamageStop();
                atelier.StopCoroutine("AtelierDamage");
                NowAttack = false;
            }
        }
    }
}
