using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ItemInventory : MonoBehaviour {
    //=============================================================
    private IEnumerator ien;

    private Vector3 AmountOfOpenMovement = new Vector3(0,200,0);
    private Vector3 AmountOfCloseMovement = new Vector3(0,-200,0);

    //=============================================================
    private void Init () {
    }

    private void Awake () {
        Init();
    }

    private void Start () {
        Vector3 pos = GetComponent<RectTransform>().localPosition;
        ien = MotionModule.PointToPointSmoothly(pos,pos + AmountOfOpenMovement,0.1f);
        StartCoroutine(ien);
    }

    private void Update () {
        if(false) {
            Vector3 pos = GetComponent<RectTransform>().localPosition;
            ien = MotionModule.PointToPointSmoothly(pos,pos + AmountOfCloseMovement,0.1f);
            StartCoroutine(ien);
        }

        GetComponent<RectTransform>().localPosition = (Vector3)ien.Current;
    }


}