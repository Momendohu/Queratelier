using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour {
    public GameObject Bomb;
    public GameObject DamageHit;

    //=============================================================
    private Rigidbody _rigifbody;

    private float invincibleIntervalLength = 1; //無敵時間の長さ
    private float invincibleInterval; //無敵時間

    private float BombLiveTimeLength = 1; //ボムが生きている時間

    //=============================================================
    private void Init () {
        _rigifbody = GetComponent<Rigidbody>();
    }

    private void Awake () {
        Init();
    }

    private void Start () {
    }

    private void Update () {
        invincibleInterval += Time.fixedDeltaTime;
    }

    //=============================================================
    /// <summary>
    /// 爆弾を生成する
    /// </summary>
    private GameObject GenerateBomb () {
        return Instantiate(Bomb,transform.position,Quaternion.identity);
    }

    //=============================================================
    /// <summary>
    /// ダメージ判定用オブジェクトを生成する
    /// </summary>
    private GameObject GenerateDamageHit () {
        return Instantiate(DamageHit,transform.position,Quaternion.identity);
    }

    //=============================================================
    /// <summary>
    /// 速度を加算する
    /// </summary>
    public void AddSpeed (Vector3 vec) {
        _rigifbody.velocity += vec;
    }

    //=============================================================
    private void OnCollisionEnter (Collision collision) {
        if(invincibleInterval >= invincibleIntervalLength) {
            Destroy(GenerateBomb(),BombLiveTimeLength);
            Destroy(GenerateDamageHit(),BombLiveTimeLength);
            Destroy(this.gameObject);
        }
    }
}