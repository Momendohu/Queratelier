using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Beam : MonoBehaviour {
    private void OnParticleCollision (GameObject other) {
        //パーティクルがぶつかったときの処理

        Debug.Log("パーティクルエネミー衝突");

        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("パーティクルエネミー衝突");
            EnemyBase enamybase = other.GetComponent<EnemyBase>();
            enamybase.hp -= 10;
        }
    }
}