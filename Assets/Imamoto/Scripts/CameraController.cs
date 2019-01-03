using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject Player;

    private Vector3 offset;
    private Vector3 prevPlayerPos;
    private Vector3 posVector;
    private Vector3 PlayerPos;
    public float scale = 3.0f;
    public float cameraSpeed = 2.0f;


	// Use this for initialization
	void Start () {
        offset = transform.position - Player.transform.position;
        prevPlayerPos = new Vector3(0, 0, -1);
        //CameraForward();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        //transform.position = Player.transform.position + offset;

        transform.position += Player.transform.position - PlayerPos;
        PlayerPos = Player.transform.position;

        CameraRotate();
        //スペースでカメラをキャラの後ろに移動
        //if (Input.GetKeyDown(KeyCode.Space)) CameraForward();
	}

    //カメラをキャラの後ろに移動させる
    private void CameraForward()
    {
        Vector3 currentPlayerPos = Player.transform.position;
        Vector3 backVecotr = (prevPlayerPos - currentPlayerPos).normalized;

        posVector = (backVecotr == Vector3.zero) ? posVector : backVecotr;

        Vector3 targetPos = currentPlayerPos + scale * posVector;
        targetPos.y = targetPos.y + 0.5f;

        this.transform.position = Vector3.Lerp(
            this.transform.position,
            targetPos,
            cameraSpeed * Time.deltaTime
        );

        this.transform.LookAt(Player.transform.position);
        prevPlayerPos = Player.transform.position;
    }

    private void CameraRotate()
    {
        Vector3 targetDir = Player.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (Input.GetKey(KeyCode.C))
        {
            transform.RotateAround(PlayerPos, Vector3.up, Time.deltaTime * 200f);
        }else if (Input.GetKey(KeyCode.X))
        {
            transform.RotateAround(PlayerPos, Vector3.up, -1 * Time.deltaTime * 200f);
        }else if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }
}
