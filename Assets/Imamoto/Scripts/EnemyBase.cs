using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//敵ベーススクリプト
//敵スクリプト作成時は、このスクリプトを継承させること
public class EnemyBase : MonoBehaviour {

    public float hp;
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
        else if (other.tag == "DamageHit")
        {
            //ノックバック追加
            hp -= 10;
            Debug.Log("bomb");
        }

        if (hp <= 0) Destroy(this.gameObject);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Magic")
        {
            EnemyWarp_CreatePoint();
        }

    }

    //衝突維持判定
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

    protected void EnemyWarp_CreatePoint()
    {
        Debug.Log("ワープ成功");

        this.transform.position = new Vector3(0, 0, 0);
    }
}
