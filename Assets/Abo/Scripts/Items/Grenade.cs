using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour {
    public GameObject Bomb;

    //=============================================================
    private Rigidbody _rigifbody;

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

    }

    //=============================================================
    /// <summary>
    /// 爆弾を生成する
    /// </summary>
    private void GenerateBomb () {
        Instantiate(Bomb,transform.position,Quaternion.identity);
    }

    //=============================================================
    /// <summary>
    /// 速度を加算する
    /// </summary>
    public void AddSpeed (Vector3 vec) {
        _rigifbody.velocity += vec;
    }
}