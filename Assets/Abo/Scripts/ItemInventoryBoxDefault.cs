using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryBoxDefault : MonoBehaviour {
    //=============================================================
    private GameObject frame;

    private Color defaultColor = new Color(1,1,1);
    private Color selectedColor = new Color(0.9411765f,0.6509804f,0.2196078f);

    //=============================================================
    private void Init () {
        frame = transform.Find("Frame").gameObject;
    }

    private void Awake () {
        Init();
    }

    //=============================================================
    /// <summary>
    /// 色を選択されているかどうかで変える
    /// </summary>
    public void SwitchColor (bool isSelected) {
        if(isSelected) {
            frame.GetComponent<Image>().color = selectedColor;
        } else {
            frame.GetComponent<Image>().color = defaultColor;
        }
    }
}