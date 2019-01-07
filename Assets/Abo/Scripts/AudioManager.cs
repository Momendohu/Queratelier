using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {
    //=============================================================


    //=============================================================
    private void Init () {
    }

    private void Awake () {
        //Instance化をすでにしてるなら
        if(this != Instance) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    private void Start () {

    }

    private void Update () {

    }
}