using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public static class ErrorManager {
    public enum ERROR_CODE {
        NONE = 0,
        AUDIO_SE_NONE = 1,
        AUDIO_BGM_NONE = 101,
    }

    /// <summary>
    /// エラーログを表示する
    /// </summary>
    public static void ErrorLog (ERROR_CODE code) {
        string str = "no message";
        switch(code) {
            case ERROR_CODE.NONE:
            str = "no message";
            break;

            case ERROR_CODE.AUDIO_SE_NONE:
            str = "存在しないSEです";
            break;

            case ERROR_CODE.AUDIO_BGM_NONE:
            str = "存在しないBGMです";
            break;

            default:
            break;
        }

        Debug.LogError(str);
    }
}