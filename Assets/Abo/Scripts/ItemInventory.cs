using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemInventory : MonoBehaviour {
    public Sprite[] ItemImage;
    public GameObject[] ItemPrefabs;

    //=============================================================
    private GameObject mixObj;
    private GameObject[] baseObj = new GameObject[4];

    private GameObject mixObjDefault;
    private GameObject[] baseObjDefault = new GameObject[4];

    private GameObject mixObjImage;

    private GameObject[] holdItemNumObj = new GameObject[4];

    private GameObject baseDisplayObject;

    //=============================================================
    private IEnumerator ien;

    private Vector3 currentPos = Vector3.zero; //現在位置
    private Vector3 basePos = Vector3.zero; //基準の位置
    private Vector3 amountOfOpenMovement = new Vector3(500,0,0); //開くときの移動量
    private Vector3 amountOfCloseMovement = new Vector3(-500,0,0); //閉じるときの移動量
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
    private IndexID indexID = IndexID.BASE1;

    //=============================================================
    /// <summary>
    /// アイテムID
    /// </summary>
    private enum ItemID {
        NONE = -1,

        MAGIC = 0,
        ORANGE = 1,
        WALL = 2,
        GRENADE = 3,

        MAGIC_MAGIC = 100, //今回使用しない 
        MAGIC_ORANGE = 101,
        MAGIC_WALL = 102,
        MAGIC_GRENADE = 103,

        ORANGE_MAGIC = 200,
        ORANGE_ORANGE = 201, //今回使用しない
        ORANGE_WALL = 202,
        ORANGE_GRENADE = 203,

        WALL_MAGIC = 300,
        WALL_ORANGE = 301,
        WALL_WALL = 302, //今回使用しない
        WALL_GRENADE = 303,

        GRENADE_MAGIC = 400,
        GRENADE_ORANGE = 401,
        GRENADE_WALL = 402,
        GRENADE_GRENADE = 403, //今回使用しない
    }

    //=============================================================
    private void Init () {
        //参照をとる
        mixObj = transform.Find("Mix/Frame/M/Focus").gameObject;
        for(int i = 0;i < baseObj.Length;i++) {
            baseObj[i] = transform.Find("Base/DisplayArea/DisplayObject/Frame/B" + (i + 1) + "/Focus").gameObject;
        }

        mixObjDefault = transform.Find("Mix/Frame/M/Default").gameObject;
        for(int i = 0;i < baseObjDefault.Length;i++) {
            baseObjDefault[i] = transform.Find("Base/DisplayArea/DisplayObject/Frame/B" + (i + 1) + "/Default").gameObject;
        }

        mixObjImage = transform.Find("Mix/Frame/M/Image").gameObject;

        for(int i = 0;i < holdItemNumObj.Length;i++) {
            holdItemNumObj[i] = transform.Find("Base/DisplayArea/DisplayObject/Frame/B" + (i + 1) + "/HoldNum/Text").gameObject;
        }

        baseDisplayObject = transform.Find("Base/DisplayArea/DisplayObject").gameObject;
    }

    private void Awake () {
        Init();
        BoxObjInitialize();
    }

    private void Start () {
        //基準の位置の更新
        basePos = baseDisplayObject.GetComponent<RectTransform>().localPosition;
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

            //アイテムの選択の解除
            if(InputModule.IsPushButtonDown(KeyCode.S)) {
                //選択しているアイテムがすでに選択されたものであるなら
                if(selectedItem.Where(x => x == (int)indexID - 1).Count() >= 1) {
                    SelectItem((int)indexID - 1);
                }
            }

            //アイテムの選択
            if(InputModule.IsPushButtonDown(KeyCode.W)) {
                //選択しているアイテムがすでに選択されたものであるなら
                if(selectedItem.Where(x => x == (int)indexID - 1).Count() >= 1) {
                    UseItem();
                    //InitializeSelectItem();
                    CheckRemainItemAndCloseBox();
                    StartClosing();
                } else {
                    SelectItem((int)indexID - 1);
                }
            }

            //インベントリを閉じる
            if(InputModule.IsPushButtonDown(KeyCode.Space)) {
                //InitializeSelectItem();
                CheckRemainItemAndCloseBox();
                StartClosing();
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

        /*if(Input.GetKey(KeyCode.L)) {
            GameObject.Find("Player").transform.eulerAngles += Vector3.up;
        }*/
        //Debug.Log(eventState);

        //位置の更新
        if(currentPos.sqrMagnitude != 0) {
            baseDisplayObject.GetComponent<RectTransform>().localPosition = currentPos;
        }
    }

    //=============================================================
    /// <summary>
    /// インベントリを開く動作を開始
    /// </summary>
    private void StartOpening () {
        //モーション処理
        Vector3 pos = baseDisplayObject.GetComponent<RectTransform>().localPosition;
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
        //BoxObjInitialize();
        //indexID = IndexID.BASE1;

        //モーション処理
        Vector3 pos = baseDisplayObject.GetComponent<RectTransform>().localPosition;
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
        if(InputModule.IsPushButtonDown(KeyCode.A)) {
            //フラグの初期化
            onceSwitchIndex = false;

            //指示してるボックスの変更
            indexID--;
            if(indexID < IndexID.MIX + 1) {
                indexID = IndexID.SIZE - 1;
            }
        }

        if(InputModule.IsPushButtonDown(KeyCode.D)) {
            //フラグの初期化
            onceSwitchIndex = false;

            //指示してるボックスの変更
            indexID++;
            if(indexID > IndexID.SIZE - 1) {
                indexID = IndexID.BASE1;
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
        if(!IsHoldItem(num)) return false;

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
    /// 特定の番号のアイテムを所持しているかどうか
    /// </summary>
    private bool IsHoldItem (int num) {
        if(holdItemNum[num] >= 1) {
            return true;
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
    /// アイテムを所持しているかどうかでアイテムボックスのアクティブの切り替えをする
    /// </summary>
    private void CheckRemainItemAndCloseBox () {
        for(int i = 0;i < holdItemNum.Length;i++) {
            if(holdItemNum[i] <= 0) {
                for(int j = 0;j < selectedItem.Length;j++) {
                    if(selectedItem[j] == i) {
                        selectedItem[j] = -1;
                    }
                }

                ChangeBaseBoxDisplay((IndexID)(i + 1),false);
            }
        }
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
            baseObj[(int)indexID - 1].GetComponent<ItemInventoryBox>().SwitchColor(isSelected);
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

            case (int)ItemID.MAGIC:
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

            case (int)ItemID.GRENADE:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[5];
            break;

            //00
            case (int)ItemID.MAGIC_MAGIC:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //01
            case (int)ItemID.MAGIC_ORANGE:
            case (int)ItemID.ORANGE_MAGIC:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //02
            case (int)ItemID.MAGIC_WALL:
            case (int)ItemID.WALL_MAGIC:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //03
            case (int)ItemID.MAGIC_GRENADE:
            case (int)ItemID.GRENADE_MAGIC:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //11
            case (int)ItemID.ORANGE_ORANGE:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //12
            case (int)ItemID.ORANGE_WALL:
            case (int)ItemID.WALL_ORANGE:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //13
            case (int)ItemID.ORANGE_GRENADE:
            case (int)ItemID.GRENADE_ORANGE:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //22
            case (int)ItemID.WALL_WALL:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //23
            case (int)ItemID.WALL_GRENADE:
            case (int)ItemID.GRENADE_WALL:
            mixObjImage.GetComponent<Image>().color = new Color(0,0,0,1);
            mixObjImage.GetComponent<Image>().sprite = ItemImage[1];
            break;

            //33
            case (int)ItemID.GRENADE_GRENADE:
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

        GameObject player = GameObject.Find("Player");
        GameObject generateObj = null;
        switch(ChangeSelectedItemToItemID(selectedItem)) {
            case (int)ItemID.NONE:
            break;

            case (int)ItemID.MAGIC:
            GameObject obj = CreateItem((int)ItemID.MAGIC,player.transform.position + GetForwardPosFromRotate(player.transform.eulerAngles) * 3);
            break;

            case (int)ItemID.ORANGE:
            generateObj = CreateItem((int)ItemID.ORANGE,player.transform.position + GetForwardPosFromRotate(player.transform.eulerAngles) * 3);
            generateObj.transform.eulerAngles = player.transform.eulerAngles;
            break;

            case (int)ItemID.WALL:
            generateObj = CreateItem((int)ItemID.WALL,player.transform.position + GetForwardPosFromRotate(player.transform.eulerAngles) * 3);
            generateObj.transform.eulerAngles = player.transform.eulerAngles;
            break;

            case (int)ItemID.GRENADE:
            generateObj = CreateItem((int)ItemID.GRENADE,player.transform.position + GetForwardPosFromRotate(player.transform.eulerAngles) * 1);
            generateObj.transform.eulerAngles = player.transform.eulerAngles;
            generateObj.GetComponent<Grenade>().AddSpeed((GetForwardPosFromRotate(player.transform.eulerAngles) + Vector3.up) * 5);
            break;

            //00
            case (int)ItemID.MAGIC_MAGIC:
            break;

            //01
            case (int)ItemID.MAGIC_ORANGE:
            case (int)ItemID.ORANGE_MAGIC:
            break;

            //02
            case (int)ItemID.MAGIC_WALL:
            case (int)ItemID.WALL_MAGIC:
            break;

            //03
            case (int)ItemID.MAGIC_GRENADE:
            case (int)ItemID.GRENADE_MAGIC:
            break;

            //11
            case (int)ItemID.ORANGE_ORANGE:
            break;

            //12
            case (int)ItemID.ORANGE_WALL:
            case (int)ItemID.WALL_ORANGE:
            break;

            //13
            case (int)ItemID.ORANGE_GRENADE:
            case (int)ItemID.GRENADE_ORANGE:
            break;

            //22
            case (int)ItemID.WALL_WALL:
            break;

            //23
            case (int)ItemID.WALL_GRENADE:
            case (int)ItemID.GRENADE_WALL:
            break;

            //33
            case (int)ItemID.GRENADE_GRENADE:
            break;

            default:
            break;
        }
    }

    //=============================================================
    /// <summary>
    /// 特定のオブジェクトの正面の座標を取得
    /// </summary>
    /// <returns></returns>
    private Vector3 GetForwardPosFromRotate (Vector3 rot) {
        return new Vector3(-Mathf.Sin(Mathf.Deg2Rad * -rot.y),0,Mathf.Cos(Mathf.Deg2Rad * -rot.y));
    }

    //=============================================================
    /// <summary>
    /// アイテムを生成する
    /// </summary>
    private GameObject CreateItem (int id,Vector3 pos) {
        GameObject obj = Instantiate(ItemPrefabs[id]) as GameObject;
        obj.transform.position = pos;

        return obj;
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