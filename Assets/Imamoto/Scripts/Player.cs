using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    //プレイヤー側

    //入力値
    private Vector3 input;
    //rigidbody
    private Rigidbody rigidbody;
    //CharacterController
    //private CharacterController charactercontroller;

    private Vector3 velocity;
    private float speed = 8f;
    private float damageInterval = 1f;
    private float blinkInterval = 0.1f;
    private bool isInvisible = false;
    public GameObject Player_model;

    private Animator animator;
    private Animation animation;

    public Renderer bodyRenderer;
    public Renderer earphoneRenderer;
    public Renderer faceRenderer;
    public Renderer hairRenderer;
    public Renderer hair_topRenderer;
    public Renderer hat_defoRenderer;
    public Renderer legRenderer;

    private Vector3 cameraForward;
    private Vector3 moveForward;

    public bool ItemStorable = false;

    //=============================================================================================================================
    //弾

    public GameObject bullet;

    private float bullet_speed = 500f;

    //発射地点
    public Transform muzzle;

    private float shotInterval;

    AnimatorStateInfo nowState;

    //================================================================================================================================


    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        //charactercontroller = GetComponent<CharacterController>();
        velocity = Vector3.zero;
        nowState = animator.GetCurrentAnimatorStateInfo(0);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(damageInterval < 1f)
        {
            isInvisible = true;
            StartCoroutine("Blink");
            //Debug.Log("むてき");
        }
        else
        {
            isInvisible = false;
            StopCoroutine("Blink");
            RendererInitialize();
            //Debug.Log("むてきかいじょ");
        }

        //ダメージ硬直0.5秒で分岐
        //硬直時は操作不能
        if (damageInterval >= 0.5f)
        {
            Move();

            Shooting();
        }
        else
        {
            velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        //Debug.Log(rigidbody.velocity);

        //モデルがずれないように固定させる
        Player_model.transform.localPosition = new Vector3(0,0,0);

        shotInterval += Time.deltaTime;
        damageInterval += Time.deltaTime;

        //Debug.Log(ItemStorable);

	}

    //================================================================================================================================
    private void Move()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        //カメラの方向から平面ベクトル取得
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        moveForward = cameraForward * input.z + Camera.main.transform.right * input.x;

        //Debug.Log(input);

        //方向キーが多少押されている
        if (input.magnitude > 0f)
        {
            //キャラを移動方向に向かせる
            //transform.rotation = Quaternion.LookRotation(input);
            transform.rotation = Quaternion.LookRotation(moveForward);

            velocity += moveForward * speed;
            //charactercontroller.Move(input * speed * Time.deltaTime);
        }
        else
        {
            velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        animator.SetFloat("Speed", input.magnitude);
        rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);

        velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void Shooting()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (shotInterval > 0.2f)
            {

                GameObject bullets = Instantiate(bullet,muzzle.transform.position, muzzle.transform.rotation) as GameObject;

                Vector3 force;

                force = this.gameObject.transform.forward * bullet_speed;

                //animator.SetInteger("AnimIndex", 1);
                //animator.SetTrigger("Attack");

                //Rigidbodyに力を加えて発射
                bullets.GetComponent<Rigidbody>().AddForce(force);

                bullets.transform.position = muzzle.transform.position;

                shotInterval = 0;
            }

            //animator.SetTrigger("Attack");

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //敵接触時
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Knockback();
            //Debug.Log("knockback");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //アイテム受け渡し範囲内の時
        if(other.gameObject.tag == "AtelierItemSend")
        {
            //Debug.Log("アトリエアイテム受け渡し状態");
            ItemStorable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //アイテム受け渡し範囲から出たとき
        if(other.gameObject.tag == "AtelierItemSend")
        {
            ItemStorable = false;
        }
    }

    //ダメージ後ノックバック
    private void Knockback()
    {
        if (damageInterval > 1f) {
            //iTween.MoveTo(this.gameObject, iTween.Hash("position", transform.position - (transform.forward * 20f),"time", 0.1f));
            rigidbody.AddForce(transform.forward * -1 * 6, ForceMode.Impulse);
            damageInterval = 0;
        }
        
    }

    //Renderer初期化
    //無敵点滅後に必ず起動させること
    private void RendererInitialize()
    {
        bodyRenderer.enabled = true;
        earphoneRenderer.enabled = true;
        faceRenderer.enabled = true;
        hairRenderer.enabled = true;
        hair_topRenderer.enabled = true;
        hat_defoRenderer.enabled = true;
        legRenderer.enabled = true;
    }

    //================================================================================================================================

    private IEnumerator Blink()
    {
        while (true)
        {
            //renderComponent.enabled = !renderComponent.enabled;
            bodyRenderer.enabled = !bodyRenderer.enabled;
            earphoneRenderer.enabled = !earphoneRenderer.enabled;
            faceRenderer.enabled = !faceRenderer.enabled;
            hairRenderer.enabled = !hairRenderer.enabled;
            hair_topRenderer.enabled = !hair_topRenderer.enabled;
            hat_defoRenderer.enabled = !hat_defoRenderer.enabled;
            legRenderer.enabled = !legRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    /*
    private IEnumerator WaitAnimationEnd(string animatorName)
    {
        bool finish = false;
        while (!finish)
        {
            AnimatorStateInfo nowState = animator.GetCurrentAnimatorStateInfo(0);
            if (nowState.IsName(animatorName))
            {
                finish = true;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    */

}
