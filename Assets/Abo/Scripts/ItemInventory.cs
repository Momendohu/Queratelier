using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour {
    public Sprite[] ItemImage;

    //=============================================================
    private GameObject mixObj;
    private GameObject[] baseObj = new GameObject[4];

    private GameObject mixObjDefault;
    private GameObject[] baseObjDefault = new GameObject[4];

    private GameObject mixObjImage;

    private GameObject[] holdItemNumObj = new GameObject[4];

    //=============================================================
    private IEnumerator ien;

    private Vector3 currentPos = Vector3.zero; //現在位置
    private Vector3 basePos = Vector3.zero; //基準の位置
    private Vector3 amountOfOpenMovement = new Vector3(0,200,0); //開くときの移動量
    private Vector3 amountOfCloseMovement = new Vector3(0,-200,0); //閉じるときの移動量
    private float movingSpeed = 0.3f; //移動スピード

    private bool onceSwitchIndex = false; //指示してるボックスが変更された時いちどだけ呼び出すフラグ

    private int[] selectedItem = { -1,-1 }; //選択したアイテム

    private int[] holdItemNum = { 4,4,4,4 };//持っているアイテムの数

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
    /// <summary>
    /// アイテムID
    /// </summary>
    private enum ItemID {
        NONE = -1,

        CANE = 0,
        ORANGE = 1,
        WALL = 2,
        BOMB = 3,

        MIX1 = 100,
        MIX2 = 101,
        MIX3 = 102,
        MIX4 = 103,

        MIX5 = 200,
        MIX6 = 201,
        MIX7 = 202,
        MIX8 = 203,

        MIX9 = 300,
        MIX10 = 301,
        MIX11 = 302,
        MIX12 = 303,

        MIX13 = 400,
        MIX14 = 401,
        MIX15 = 402,
        MIX16 = 403,
    }

    //=============================================================
    private void Init () {
        //参照をとる
        mixObj = transform.Find("Mix/Frame/M/Focus").gameObject;
        for(int i = 0;i < baseObj.Length;i++) {
            baseObj[i] = transform.Find("Base/Frame/B" + (i + 1) + "/Focus").gameObject;
        }

        mixObjDefault = transform.Find("Mix/Frame/M/Default").gameObject;
        for(int i = 0;i < baseObjDefault.Length;i++) {
            baseObjDefault[i] = transform.Find("Base/Frame/B" + (i + 1) + "/Default").gameObject;
        }

        mixObjImage = transform.Find("Mix/Frame/M/Image").gameObject;

        for(int i = 0;i < holdItemNumObj.Length;i++) {
            holdItemNumObj[i] = transform.Find("Base/Frame/B" + (i + 1) + "/HoldNum/Text").gameObject;
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

            //指示してるボックスに応じて処理を行う
            switch(indexID) {
                case IndexID.MIX:
                BoxObjSwitchActivate(mixObj);
                break;

                case IndexID.BASE1:
                BoxObjSwitchActivate(baseObj[0]);
                break;

                case IndexID.BASE2:
                BoxObjSwitchActivate(baseObj[1]);
                break;

                case IndexID.BASE3:
                BoxObjSwitchActivate(baseObj[2]);
                break;

                case IndexID.BASE4:
                BoxObjSwitchActivate(baseObj[3]);
                break;

                default:
                break;
            }

            //アイテムの選択
            if(InputModule.IsPushButtonDown(KeyCode.Space)) {
                //ミックスボックスを選択しているなら
                if(indexID == IndexID.MIX) {
                    //アイテムが何かしら選択されているなら決定してインベントリを閉じる
                    if(!(ChangeSelectedItemToItemID(selectedItem) == -1)) {
                        UseItem();
                        InitializeSelectItem();
                        StartClosing();
                    }
                }

                //アイテムを選択(ベースボックス選択時)
                if(SelectItem((int)indexID - 1)) {

                } else {

                }
            }

            ShiftIndex();
            ApplyMixBoxDisplay();
            DisplayHoldItemNum();

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
        //モーション処理
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
        //インベントリ操作の初期化
        onceSwitchIndex = false;
        BoxObjInitialize();
        indexID = IndexID.MIX;

        //モーション処理
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

    //=============================================================
    /// <summary>
    /// インベントリボックスのアクティブの切り替え
    /// </summary>
    private void BoxObjSwitchActivate (GameObject obj) {
        if(!onceSwitchIndex) {
            BoxObjInitialize();
            obj.SetActive(true);
            onceSwitchIndex = true;
        }
    }

    //=============================================================
    /// <summary>
    /// アイテムを選択
    /// </summary>
    private bool SelectItem (int num) {
        if(num == -1) {
            return false;
        }

        //アイテムを所持していない場合処理をスキップ
        if(holdItemNum[num] <= 0) {
            return false;
        }

        //すでにアイテムを選んでいるかを調べる
        for(int i = 0;i < selectedItem.Length;i++) {
            if(selectedItem[i] == num) {
                selectedItem[i] = -1;
                ChangeBaseBoxDisplay(indexID,false);
                return true;
            }
        }

        //選択アイテムの保存
        for(int i = 0;i < selectedItem.Length;i++) {
            if(selectedItem[i] == -1) {
                selectedItem[i] = num;
                ChangeBaseBoxDisplay(indexID,true);
                return true;
            } else {
                continue;
            }
        }

        return false;
    }

    //=============================================================
    /// <summary>
    /// アイテム選択の初期化
    /// </summary>
    private void InitializeSelectItem () {
        for(int i = 0;i < selectedItem.Length;i++) {
            selectedItem[i] = -1;
        }

        ChangeBaseBoxDisplay(IndexID.BASE1,false);
        ChangeBaseBoxDisplay(IndexID.BASE2,false);
        ChangeBaseBoxDisplay(IndexID.BASE3,false);
        ChangeBaseBoxDisplay(IndexID.BASE4,false);
    }

    //=============================================================
    /// <summary>
    /// ボックス表示の変更(選択したかどうかを見た目でわかるように)
    /// </summary>
    private void ChangeBaseBoxDisplay (IndexID indexID,bool isSelected) {
        switch(indexID) {
            case IndexID.BASE1:
            case IndexID.BASE2:
            case IndexID.BASE3:
            case IndexID.BASE4:
            baseObjDefault[(int)indexID - 1].GetComponent<ItemInventoryBoxDefault>().SwitchColor(isSelected);
            break;

            default:
            break;
        }
    }

    //=============================================================
    /// <summary>
    /// 合成アイテムボックス表示の変更
    /// </summary>
    private void ApplyMixBoxDisplay () {
        switch(ChangeSelectedItemToItemID(selectedItem)) {
            case (int)ItemID.NONE:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,0);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[0];
            break;

            case (int)ItemID.CANE:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[2];
            break;

            case (int)ItemID.ORANGE:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[3];
            break;

            case (int)ItemID.WALL:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[4];
            break;

            case (int)ItemID.BOMB:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[5];
            break;

            //00
            case (int)ItemID.MIX1:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //01
            case (int)ItemID.MIX2:
            case (int)ItemID.MIX5:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //02
            case (int)ItemID.MIX3:
            case (int)ItemID.MIX9:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //03
            case (int)ItemID.MIX4:
            case (int)ItemID.MIX13:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //11
            case (int)ItemID.MIX6:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //12
            case (int)ItemID.MIX7:
            case (int)ItemID.MIX10:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //13
            case (int)ItemID.MIX8:
            case (int)ItemID.MIX14:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //22
            case (int)ItemID.MIX11:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //23
            case (int)ItemID.MIX12:
            case (int)ItemID.MIX15:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //33
            case (int)ItemID.MIX16:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            default:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,0);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[0];
            break;
        }
    }

    //=============================================================
    /// <summary>
    /// 選択したアイテムからアイテムIDを割り出す
    /// </summary>
    private int ChangeSelectedItemToItemID (int[] selectedItem) {
        int item1 = -1;
        int item2 = -1;

        for(int i = 0;i < selectedItem.Length;i++) {
            if(selectedItem[i] != -1) {
                if(item1 == -1) {
                    item1 = selectedItem[i];
                } else {
                    item2 = selectedItem[i];
                }
            }
        }

        int itemId = -1;
        if(item2 != -1) {
            itemId = (item2 + 1) * 100 + item1;
        } else {
            itemId = item1;
        }

        return itemId;
    }

    //=============================================================
    /// <summary>
    /// アイテム所持数の表示
    /// </summary>
    private void DisplayHoldItemNum () {
        for(int i = 0;i < holdItemNumObj.Length;i++) {
            holdItemNumObj[i].GetComponent<Text>().text = "" + holdItemNum[i];
        }
    }

    //=============================================================
    /// <summary>
    /// アイテムを使用する
    /// </summary>
    private void UseItem () {
        for(int i = 0;i < selectedItem.Length;i++) {
            if(selectedItem[i] != -1) {
                holdItemNum[selectedItem[i]]--;
            }
        }

        CreateTestObj(GameObject.Find("Player").transform.position+Vector3.forward*2);
        switch(ChangeSelectedItemToItemID(selectedItem)) {
            case (int)ItemID.NONE:
            break;

            case (int)ItemID.CANE:
            break;

            case (int)ItemID.ORANGE:
            break;

            case (int)ItemID.WALL:
            break;

            case (int)ItemID.BOMB:
            break;

            //00
            case (int)ItemID.MIX1:
            break;

            //01
            case (int)ItemID.MIX2:
            case (int)ItemID.MIX5:
            break;

            //02
            case (int)ItemID.MIX3:
            case (int)ItemID.MIX9:
            break;

            //03
            case (int)ItemID.MIX4:
            case (int)ItemID.MIX13:
            break;

            //11
            case (int)ItemID.MIX6:
            break;

            //12
            case (int)ItemID.MIX7:
            case (int)ItemID.MIX10:
            break;

            //13
            case (int)ItemID.MIX8:
            case (int)ItemID.MIX14:
            break;

            //22
            case (int)ItemID.MIX11:
            break;

            //23
            case (int)ItemID.MIX12:
            case (int)ItemID.MIX15:
            break;

            //33
            case (int)ItemID.MIX16:
            break;

            default:
            break;
        }
    }


    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||(テスト用)
    //=============================================================
    /// <summary>
    /// テスト用オブジェクトを作成する
    /// </summary>
    private void CreateTestObj (Vector3 pos) {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.localScale *= 0.5f;
        obj.transform.position = pos;
        obj.AddComponent<Rigidbody>();
    }
}