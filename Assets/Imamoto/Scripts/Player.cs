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
    private CharacterController charactercontroller;

    private Vector3 velocity;
    private float speed = 5f;
    private float damageInterval;
    private bool isInvisible = false;

    private Animator animator;



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
        charactercontroller = GetComponent<CharacterController>();
        velocity = Vector3.zero;
        nowState = animator.GetCurrentAnimatorStateInfo(0);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Move();

        Shooting();

        if(damageInterval < 1f)
        {
            isInvisible = true;
        }
        else
        {
            isInvisible = false;
        }

        shotInterval += Time.deltaTime;
        damageInterval += Time.deltaTime;

        //Debug.Log(input);
	}

    //================================================================================================================================

    private void Move()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        //方向キーが多少押されている
        if (input.magnitude > 0f)
        {
            animator.SetFloat("Speed", input.magnitude);

            transform.rotation = Quaternion.LookRotation(input);

            charactercontroller.Move(input * speed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void Shooting()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (shotInterval > 0.25f)
            {
                GameObject bullets = Instantiate(bullet,muzzle.transform.position, muzzle.transform.rotation) as GameObject;

                Vector3 force;

                force = this.gameObject.transform.forward * bullet_speed;

                //animator.SetInteger("AnimIndex", 1);

                //Rigidbodyに力を加えて発射
                bullets.GetComponent<Rigidbody>().AddForce(force);

                bullets.transform.position = muzzle.transform.position;

                shotInterval = 0;
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);

        if(collision.gameObject.CompareTag("Enemy"))
        {
            Knockback();
        }
    }

    private void Knockback()
    {
        if (damageInterval > 1f) {
            iTween.MoveTo(this.gameObject, iTween.Hash("position", transform.position - (transform.forward * 0.4f)));
            damageInterval = 0;
        }
        
    }

    //================================================================================================================================

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
