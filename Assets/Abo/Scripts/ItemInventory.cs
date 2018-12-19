using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ItemInventory : MonoBehaviour {
    private GameObject mixObj;
    private GameObject[] baseObj = new GameObject[4];

    //=============================================================
    private IEnumerator ien;

    private Vector3 currentPos = Vector3.zero; //現在位置
    private Vector3 basePos = Vector3.zero; //基準の位置
    private Vector3 amountOfOpenMovement = new Vector3(0,200,0); //開くときの移動量
    private Vector3 amountOfCloseMovement = new Vector3(0,-200,0); //閉じるときの移動量
    private float movingSpeed = 0.1f; //移動スピード

    //=============================================================
    //一度だけ起動する形フラグ
    private bool openingOnce = false;
    private bool closingOnce = false;

    /// <summary>
    ///一度だけ起動する系フラグの初期化 
    /// </summary>
    private void OnceFlagInitialize () {
        openingOnce = false;
        closingOnce = false;
    }

    //=============================================================
    //インベントリの状態
    private enum EventState {
        CLOSE_WAIT = 0,
        OPENING = 1,
        OPEN_WAIT = 2,
        CLOSING = 3,

        SIZE = 4,
    }
    private EventState eventState = EventState.CLOSE_WAIT;

    //指示してる場所のid
    private enum IndexID {
        MIX = 0,
        BASE1 = 1,
        BASE2 = 2,
        BASE3 = 3,
        BASE4 = 4,

        SIZE = 5,
    }
    private IndexID indexID = IndexID.MIX;

    //=============================================================
    private void Init () {
        //参照をとる
        mixObj = transform.Find("Mix/Frame/M").gameObject;
        for(int i = 0;i < baseObj.Length;i++) {
            baseObj[i] = GameObject.Find("Base/Frame/B" + i);
        }
    }

    private void Awake () {
        Init();
    }

    private void Start () {
        //基準の位置の更新
        basePos = GetComponent<RectTransform>().localPosition;
    }

    private void Update () {
        //現在位置の取得
        if(ien != null) {
            currentPos = (Vector3)ien.Current;
        }

        switch(eventState) {
            case EventState.CLOSE_WAIT:
            StartOpening();
            break;

            case EventState.OPENING:
            OnceFlagInitialize();

            //インベントリが開ききったかの確認
            if(currentPos.sqrMagnitude == (basePos + amountOfOpenMovement).sqrMagnitude) {
                eventState = EventState.OPEN_WAIT;
            }
            break;

            case EventState.OPEN_WAIT:
            StartClosing();
            break;

            case EventState.CLOSING:
            OnceFlagInitialize();

            //インベントリが閉じきったかの確認
            if(currentPos.sqrMagnitude == basePos.sqrMagnitude) {
                eventState = EventState.CLOSE_WAIT;
            }
            break;

            default:
            break;
        }

        Debug.Log(eventState);

        //位置の更新
        GetComponent<RectTransform>().localPosition = currentPos;
    }

    //=============================================================
    /// <summary>
    /// インベントリを開く動作を開始
    /// </summary>
    private void StartOpening () {
        //一回だけ処理
        if(!openingOnce) {
            Vector3 pos = GetComponent<RectTransform>().localPosition;
            ien = MotionModule.PointToPointSmoothly(pos,pos + amountOfOpenMovement,movingSpeed);
            StartCoroutine(ien);
            eventState = EventState.OPENING;
            openingOnce = true;
        }
    }

    //=============================================================
    /// <summary>
    /// インベントリを閉じる動作を開始
    /// </summary>
    private void StartClosing () {
        //一度だけ処理
        if(!closingOnce) {
            Vector3 pos = GetComponent<RectTransform>().localPosition;
            ien = MotionModule.PointToPointSmoothly(pos,pos + amountOfCloseMovement,movingSpeed);
            StartCoroutine(ien);
            eventState = EventState.CLOSING;
            closingOnce = true;
        }
    }
}