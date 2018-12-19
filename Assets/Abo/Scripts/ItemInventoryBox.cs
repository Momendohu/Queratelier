using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ItemInventoryBox : MonoBehaviour {
    //=============================================================
    private GameObject frame;
    private GameObject soakingObj;

    private Coroutine coroutine;

    private float sockSpeed = 0.1f; //ボックスをひたすスピード

    //ひたしたかどうか
    private bool isSoaked;
    public bool IsSoaked {
        get { return isSoaked; }
        set { isSoaked = value; }
    }

    //=============================================================
    private void Init () {
        frame = transform.Find("Frame").gameObject;
        soakingObj = transform.Find("Frame/SoakingObj").gameObject;
    }

    private void Awake () {
        Init();
    }

    private void OnEnable () {
        coroutine = StartCoroutine(SoakFrame(sockSpeed));
    }

    private void OnDisable () {
        StopCoroutine(coroutine);
        WipeFrame();
    }

    //=============================================================
    /// <summary>
    /// フレームをひたす
    /// </summary>
    private IEnumerator SoakFrame (float timeLength) {
        isSoaked = true;

        float time = 0;

        while(time < 1) {
            time += Time.fixedDeltaTime / timeLength;
            soakingObj.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(Vector2.zero,frame.GetComponent<RectTransform>().sizeDelta,time);

            yield return null;
        }

        soakingObj.GetComponent<RectTransform>().sizeDelta = frame.GetComponent<RectTransform>().sizeDelta;
    }

    //=============================================================
    /// <summary>
    /// フレームをふき取る
    /// </summary>
    private void WipeFrame () {
        isSoaked = false;
        soakingObj.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
    }
}