using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour {
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
    /// 速度を加算する
    /// </summary>
    public void AddSpeed (Vector3 vec) {
        _rigifbody.velocity += vec;
    }
}