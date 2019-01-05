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
    public string CreatePoint;
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

    //初期出現位置探索関数
    //使用する場所は、エネミー生成時のスタートで使用を想定
    public string SearchEnemyCreatePoint()
    {
        GameObject nearCreatePoint = null;
        float minDis = 1000f;

        GameObject[] CreatePoints = GameObject.FindGameObjectsWithTag("EnemyCreatePoint");

        foreach(GameObject Point in CreatePoints)
        {
            float dis = Vector3.Distance(transform.position,Point.transform.position);
            if(dis < minDis)
            {
                minDis = dis;
                nearCreatePoint = Point;
            }
        }

        switch (nearCreatePoint.name)
        {
            case "EnemyCreatePoint_UP":
                CreatePoint = "UP";
                break;

            case "EnemyCreatePoint_UNDER":
                CreatePoint = "UNDER";
                break;

            case "EnemyCreatePoint_RIGHT":
                CreatePoint = "RIGHT";
                break;

            case "EnemyCreatePoint_LEFT":
                CreatePoint = "LEFT";
                break;
        }

        return CreatePoint;
    }

    //要改良
    //弾との衝突時
    protected void OnTriggerEnter(Collider other)
    {
        WaveManager WaveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        if (other.tag == "Bullet")
        {
            hp--;
        }
        else if (other.tag == "DamageHit")
        {
            //ノックバック追加
            hp -= 10;
            Debug.Log("bomb");
        }else if (other.tag == "MagicHit")
        {
            EnemyWarp_CreatePoint(this.CreatePoint);
        }

        if (hp <= 0)
        {
            Destroy(this.gameObject);
            WaveManager.ReciveDestroyEnemySum();
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {

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

    protected void EnemyWarp_CreatePoint(string CreatePoint_argument)
    {
        //Debug.Log("ワープ成功");
        //this.transform.position = new Vector3(0, 0, 0);
        GameObject CreatePointObject = null;

        switch (CreatePoint_argument)
        {
            case "UP":
                CreatePointObject = GameObject.Find("EnemyCreatePoint_UP");
                break;

            case "UNDER":
                CreatePointObject = GameObject.Find("EnemyCreatePoint_UNDER");
                break;

            case "RIGHT":
                CreatePointObject = GameObject.Find("EnemyCreatePoint_RIGHT");
                break;

            case "LEFT":
                CreatePointObject = GameObject.Find("EnemyCreatePoint_LEFT");
                break;
        }

        this.transform.position = CreatePointObject.transform.position;
    }
}
