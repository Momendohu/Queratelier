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

    private bool onceSwitchIndex = false; //指示してるボックスが変更された時いちどだけ呼び出すフラグ

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

    //指示してるボックスのid
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
        mixObj = transform.Find("Mix/Frame/M/Focus").gameObject;
        for(int i = 0;i < baseObj.Length;i++) {
            baseObj[i] = GameObject.Find("Base/Frame/B" + (i + 1) + "/Focus");
        }
    }

    private void Awake () {
        Init();
        BoxObjInitialize();
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
            if(InputModule.IsPushButtonDown(KeyCode.Space)) {
                StartOpening();
            }
            break;

            case EventState.OPENING:
            if(IsCompletedOpening()) eventState = EventState.OPEN_WAIT;
            break;

            case EventState.OPEN_WAIT:
            ShiftIndex();

            if(InputModule.IsPushButtonDown(KeyCode.Space)) {
                StartClosing();
            }

            //指示してるボックスに応じて処理を行う
            switch(indexID) {
                case IndexID.MIX:
                if(!onceSwitchIndex) {
                    BoxObjInitialize();
                    mixObj.SetActive(true);
                    onceSwitchIndex = true;
                }

                break;

                case IndexID.BASE1:
                if(!onceSwitchIndex) {
                    BoxObjInitialize();
                    baseObj[0].SetActive(true);
                    onceSwitchIndex = true;
                }
                break;

                case IndexID.BASE2:
                if(!onceSwitchIndex) {
                    BoxObjInitialize();
                    baseObj[1].SetActive(true);
                    onceSwitchIndex = true;
                }
                break;

                case IndexID.BASE3:
                if(!onceSwitchIndex) {
                    BoxObjInitialize();
                    baseObj[2].SetActive(true);
                    onceSwitchIndex = true;
                }
                break;

                case IndexID.BASE4:
                if(!onceSwitchIndex) {
                    BoxObjInitialize();
                    baseObj[3].SetActive(true);
                    onceSwitchIndex = true;
                }
                break;

                default:
                break;
            }
            break;

            case EventState.CLOSING:
            if(IsCompletedClosing()) eventState = EventState.CLOSE_WAIT;
            break;

            default:
            break;
        }

        //Debug.Log(eventState);

        //位置の更新
        if(currentPos.sqrMagnitude != 0) {
            GetComponent<RectTransform>().localPosition = currentPos;
        }
    }

    //=============================================================
    /// <summary>
    /// インベントリを開く動作を開始
    /// </summary>
    private void StartOpening () {
        Vector3 pos = GetComponent<RectTransform>().localPosition;
        ien = MotionModule.PointToPointSmoothly(pos,pos + amountOfOpenMovement,movingSpeed);
        StartCoroutine(ien);
        eventState = EventState.OPENING;
    }

    //=============================================================
    /// <summary>
    /// インベントリを閉じる動作を開始
    /// </summary>
    private void StartClosing () {
        Vector3 pos = GetComponent<RectTransform>().localPosition;
        ien = MotionModule.PointToPointSmoothly(pos,pos + amountOfCloseMovement,movingSpeed);
        StartCoroutine(ien);
        eventState = EventState.CLOSING;
    }

    //=============================================================
    /// <summary>
    /// インベントリが完全に開いたか
    /// </summary>
    private bool IsCompletedOpening () {
        if(currentPos.sqrMagnitude == (basePos + amountOfOpenMovement).sqrMagnitude) {
            return true;
        }

        return false;
    }

    //=============================================================
    /// <summary>
    /// インベントリが完全に閉じたか
    /// </summary>
    private bool IsCompletedClosing () {
        if(currentPos.sqrMagnitude == basePos.sqrMagnitude) {
            return true;
        }

        return false;
    }

    //=============================================================
    /// <summary>
    /// 指示してるボックスを変更する
    /// </summary>
    private void ShiftIndex () {
        if(InputModule.IsPushButtonDown(KeyCode.LeftArrow) || InputModule.IsPushButtonDown(KeyCode.A)) {
            //フラグの初期化
            onceSwitchIndex = false;

            //指示してるボックスの変更
            indexID--;
            if(indexID < 0) {
                indexID = IndexID.SIZE - 1;
            }
        }

        if(InputModule.IsPushButtonDown(KeyCode.RightArrow) || InputModule.IsPushButtonDown(KeyCode.D)) {
            //フラグの初期化
            onceSwitchIndex = false;

            //指示してるボックスの変更
            indexID++;
            if(indexID > IndexID.SIZE - 1) {
                indexID = IndexID.MIX;
            }
        }
    }

    //=============================================================
    /// <summary>
    /// インベントリボックスのアクティブ初期化
    /// </summary>
    private void BoxObjInitialize () {
        mixObj.SetActive(false);
        for(int i = 0;i < baseObj.Length;i++) {
            baseObj[i].SetActive(false);
        }
    }
}