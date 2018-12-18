using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public static class MotionModule {
    /// <summary>
    /// 点から点に移動する
    /// </summary>
    public static IEnumerator PointToPoint (Vector3 from,Vector3 to,float timeLength) {
        Vector3 currentPos = Vector3.zero;

        float time = 0;
        //一定の時間が経過したら
        while(time < 1) {
            time += Time.fixedDeltaTime / timeLength;
            currentPos = Vector3.Lerp(from,to,time);

            yield return currentPos;
        }

        yield return to;
    }

    /// <summary>
    /// 点から点に移動する(滑らかに)
    /// </summary>
    public static IEnumerator PointToPointSmoothly (Vector3 from,Vector3 to,float speed,float thresfold) {
        Vector3 currentPos = Vector3.zero;

        //定めたゴールに近づいたら
        while(Vector3.SqrMagnitude(currentPos - to) < thresfold) {
            currentPos = Vector3.Lerp(from,to,speed);

            yield return currentPos;
        }

        yield return to;
    }

    /// <summary>
    /// 点から点に移動する(カーブ点あり)
    /// </summary>
    public static IEnumerator PointToPointCurve (Vector3 from,Vector3 to,float timeLength,Vector3 curvePoint,float curvePower) {
        Vector3 currentPos = Vector3.zero;
        Vector3 goal = to;

        float time = 0;
        //一定の時間が経過したら
        while(time < 1) {
            time += Time.fixedDeltaTime / timeLength;
            goal = to * (1 - Mathf.Sin(Mathf.Deg2Rad * time * 180)) + curvePoint * Mathf.Sin(Mathf.Deg2Rad * time * 180) * curvePower;
            currentPos = Vector3.Lerp(from,goal,time);

            yield return currentPos;
        }

        yield return to;
    }
}