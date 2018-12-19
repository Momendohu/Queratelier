using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ItemInventory : MonoBehaviour {
    //=============================================================
    private IEnumerator ien;

    private Vector3 AmountOfOpenMovement = new Vector3(0,200,0); //開くときの移動量
    private Vector3 AmountOfCloseMovement = new Vector3(0,-200,0); //閉じるときの移動量
    private float movingSpeed = 0.1f; //移動スピード

    private bool openingFlag = false; //開いている最中かどうか
    private bool closingFlag = false; //閉じている最中かどうか

    //指示してる場所のid
    private enum IndexID {
        MIX = 0,
        BASE1 = 1,
        BASE2 = 2,
        BASE3 = 3,
        BASE4 = 4,

        COUNT = 5,
    }
    private IndexID indexID = IndexID.MIX;

    //=============================================================
    private void Init () {
    }

    private void Awake () {
        Init();
    }

    private void Start () {
        //インベントリを開く動作を開始
        Vector3 pos = GetComponent<RectTransform>().localPosition;
        ien = MotionModule.PointToPointSmoothly(pos,pos + AmountOfOpenMovement,movingSpeed);
        StartCoroutine(ien);
        openingFlag = true;
    }

    private void Update () {
        if(Input.GetKeyDown(KeyCode.Space)) {
            //インベントリを閉じる動作を開始
            Vector3 pos = GetComponent<RectTransform>().localPosition;
            ien = MotionModule.PointToPointSmoothly(pos,pos + AmountOfCloseMovement,movingSpeed);
            StartCoroutine(ien);
            closingFlag = true;
        }

        GetComponent<RectTransform>().localPosition = (Vector3)ien.Current;
    }


}