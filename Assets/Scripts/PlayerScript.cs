using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour
{
    public float speed = 4f;        // スピード
    public float jump_power = 700;  // ジャンプ力ぅ
    public LayerMask ground_layer;  // 地面の判定用
    public LifeScript lifeScript;   // ライフオブジェクト
    public GameObject skillObj;     // スキルオブジェクト

    private Rigidbody2D rigidbody_2d;
    private Animator    anim;
    private Renderer    renderer;
    private ToggleGroup toggleGroup; // スキルトグルグループ

    // フリック操作用変数
    public Vector3 touchStartPos;
    public Vector3 touchEndPos;

    public Text clearText;          // ゲームクリアー時に表示するテキスト

    private bool isGrounded;        // 地面設置判定
    private bool gameClear = false; // ゲームクリアーしたら操作を無効にする

    private float old_x = 0; // 進めていないとき（壁にぶつかっているとき）

    int move = 0;

    
    void Start()
    {
        initObject();
    }

    // 初期化
    public void initObject()
    {
        anim = GetComponent<Animator>();
        rigidbody_2d = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
        Instantiate(skillObj, GameObject.Find("Canvas").transform);
        toggleGroup = GameObject.FindWithTag("ToggleParent").GetComponent<ToggleGroup>();
    }

    // 操作系
    void Update()
    {
        // ボタンクリック時は処理をしない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        isGrounded = Physics2D.Linecast(
        transform.position + transform.up * 1,
        transform.position - transform.up * 0.05f,
        ground_layer);

        //ゲームクリア時ジャンプさせない
        if (!gameClear)
        {
            Flick();
        }
        float velY = rigidbody_2d.velocity.y;
        bool isJumping = velY > 0.1f ? true : false;
        bool isFalling = velY < -0.1f ? true : false;
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);
    }


    // 移動など
    void FixedUpdate()
    {
        //左右に移動させない　main_cameraを動かさない
        if (!gameClear)
        {
            // 壁にぶつかったら死ぬ
            if (old_x == transform.position.x)
            {
                lifeScript.GameOver();
            }

            playerMove();

            old_x = transform.position.x;

            //現在のカメラの位置から8低くした位置を下回った時
            if (gameObject.transform.position.y < Camera.main.transform.position.y - 8)
            {
                //LifeScriptのGameOverメソッドを実行する
                lifeScript.GameOver();
            }
        }
        // ゲームクリア時
        else
        {
            //クリアーテキストを表示
            clearText.enabled = true;
            //アニメーションは走り
            anim.SetBool("Dash", true);
            //右に進み続ける
            rigidbody_2d.velocity = new Vector2(speed, rigidbody_2d.velocity.y);
            //5秒後にタイトル画面へ戻るCallTitleメソッドを呼び出す
            Invoke("CallTitle", 5);
        }
    }

    // プレイヤーの動き全般
    public void playerMove()
    {
        rigidbody_2d.velocity = new Vector2(speed, rigidbody_2d.velocity.y);
        anim.SetBool("Dash", true);

        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = transform.position.x + 4;
        Camera.main.transform.position = cameraPos;
    }

    /// <summary>
    /// Collision接触時
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter2D(Collision2D col)
    {
        //Enemyとぶつかった時にコルーチンを実行
        if (col.gameObject.tag == "Enemy")
        {
            startDamage();
        }
    }

    // Damage判定開始
    public void startDamage()
    {
        StartCoroutine("Damage");
    }

    // ダメージ用
    IEnumerator Damage()
    {
        //レイヤーをPlayerDamageに変更
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
        //while文を10回ループ
        int count = 10;
        while (count > 0)
        {
            //透明にする
            renderer.material.color = new Color(1, 1, 1, 0);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            //元に戻す
            renderer.material.color = new Color(1, 1, 1, 1);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            count--;
        }
        //レイヤーをPlayerに戻す
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    /// <summary>
    /// Trigger2D接触時
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        //タグがClearZoneであるTriggerにぶつかったら
        if (col.tag == "ClearZone")
        {
            //ゲームクリアー
            gameClear = true;
        }
    }

    /// <summary>
    /// タイトルに移動
    /// </summary>
    void CallTitle()
    {
        //タイトル画面へ
        SceneManager.LoadScene("Title");
    }

    // フリック操作
    void Flick()
    {

        // 押したとき
        if (Input.GetButtonDown("Fire1"))
        {
            // フリック開始位置を取得
            touchStartPos = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.z);
        }

        // 離したとき
        if (Input.GetMouseButtonUp(0))
        {
            // フリック開始位置を取得
            touchEndPos = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.z);


            //フリック処理
            GetDirection();
        }

    }

    // フリックされた方向を取得するメソッド
    void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;
        float flickDirection = 30; // フリック範囲
        string Direction = "";
        
        // 左右フリック判定
        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (flickDirection < directionX)
            {
                //右向きフック
                Direction = "right";
            }
            else if(-flickDirection >directionX)
            {
                Direction = "Left";
            }
        }

        // 上下フリック判定
        else if (Mathf.Abs(directionX) < Mathf.Abs(directionY))
        {
            if (flickDirection < directionY)
            {
                Direction = "up";
            }
            else if (-flickDirection >directionY)
            {
                Direction = "down";
            }
        }

        // タッチ判定
        else
        {
            // タッチを検出
            Direction = "touch";
        }
        
        //フリック時の処理
        switch (Direction)
        {
            case "right":

                break;
            case "left":

                break;
            case "up":
                Jump();
                break;
            case "down":

                break;
            case "touch":
                anim.SetTrigger("Shot");
                SkillScript skillScript = GetComponent<SkillScript>();
                Debug.Log(toggleGroup.ActiveToggles().First().name);
                skillScript.useSkill(toggleGroup.ActiveToggles().First().name);
                break;
        }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    public void Jump()
    {
        if (isGrounded)
        {
            anim.SetTrigger("Jump");
            isGrounded = false;
            rigidbody_2d.AddForce(Vector2.up * jump_power);
        }
    }
}