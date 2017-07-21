using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float attackRate=5;
	private float attackTimer;

	public Transform bulletTopPosition;
	public Transform bulletLeftPosition;
	public Transform bulletRightPosition;
	public GameObject bullet1Prefab;
	public GameObject bullet2Prefab;

    private bool isMouseDown=false;
    private Vector3 lastMouseposition=Vector3.zero;

    private bool isdead = true; 
    private   bool isHaveSuperWeapon=false;
    private float weaponTimer = 10;
    private float weaponHoldTime=0;
    private Transform bulletHoder;
    private Animator animator;
    private BoxCollider2D collider;
    private AudioSource audio;

    private void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        audio = GetComponent<AudioSource>();
        bulletHoder = new GameObject("Bullet").transform;
    }
    // Update is called once per frame
    void Update () {
        if(isdead){
            attackTimer += Time.deltaTime;
            if (attackTimer >= 1f / attackRate)
            {
                attackTimer = 0;
                ShootBullet();
            }

            weaponHoldTime += Time.deltaTime;
            if (weaponHoldTime >= weaponTimer)
            {
                isHaveSuperWeapon = false;
            }
            PlayerMoving();
        }
    }
    void PlayerMoving()
    {
        if(Input.GetMouseButtonDown(0)){
            isMouseDown = true;
        }
        if(Input.GetMouseButtonUp(0)){
            isMouseDown = false;
            lastMouseposition = Vector3.zero;
        }
        if(isMouseDown==true){
            if(lastMouseposition != Vector3.zero){
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMouseposition;
                transform.position = transform.position + offset;
            }
            CheckPosition();
            lastMouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    void CheckPosition(){
        Vector3 position = transform.position;
        if (position.x > 2.2f) position.x = 2.2f;
        if (position.x <- 2.2f) position.x = -2.2f;
        if (position.y > 3.5f) position.y = 3.5f;
        if (position.y <- 3.5f) position.y =-3.5f;
        transform.position=position;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Award")
        {
            if (collision.gameObject.name == "award_type_0(Clone)") {
                isHaveSuperWeapon = true;
                weaponHoldTime = 0;
                Destroy(collision.gameObject);
            }
            else{
                GameManager._intance.AddBombCount();
                Destroy(collision.gameObject);
            }
        }else if (collision.tag == "Enemy")
        {
            StartCoroutine(PlayerDead());
            GameMenu._instance.GameEnd();
        }
    }
    IEnumerator PlayerDead()
    {
        audio.Play();
        collider.enabled = false;
        isdead = false;
        animator.Play("explosion");
        bool isDestory = false;
        while (!isDestory)
        {
            AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!stateinfo.IsName("explosion"))
            {
                yield return 1;
            }
            else
            {
                isDestory = true;
            }
        }
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        Destroy(gameObject);
    }
    void ShootBullet(){
        if(isHaveSuperWeapon ==true){
            GameObject go1= Instantiate(bullet1Prefab, bulletTopPosition.position, Quaternion.identity);
            GameObject go2 = Instantiate(bullet2Prefab, bulletLeftPosition.position, Quaternion.identity);
            GameObject go3 = Instantiate(bullet2Prefab, bulletRightPosition.position, Quaternion.identity);
            go1.transform.SetParent(bulletHoder);
            go2.transform.SetParent(bulletHoder);
            go3.transform.SetParent(bulletHoder);
        }
        else{
            GameObject go1 = Instantiate(bullet1Prefab, bulletTopPosition.position, Quaternion.identity);
            go1.transform.SetParent(bulletHoder);
        }
        
    }
}
