using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class ShotScript : MonoBehaviour {

    public GameObject Player;
    private Animator anim;
    public GameObject bullet;


    void Start () {
        // プレイヤーを取得
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = Player.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        // ボタンクリック時は処理をしない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        /*
        // 攻撃
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Shot");
            Instantiate(bullet, Player.transform.position + new Vector3(0f, 1.2f, 0f), Player.transform.rotation);
        }*/
    }
}
