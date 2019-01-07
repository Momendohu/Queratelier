using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;

    public static T Instance {
        get {
            if(instance == null) {
                //ヒエラルキー上からT型のスクリプトにアクセス
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null) {
                    Debug.LogError(typeof(T) + "がそもそもないよ");
                }
            }

            return instance;
        }
    }
}