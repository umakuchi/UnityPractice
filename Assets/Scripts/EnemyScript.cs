using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{

    Rigidbody2D rigidbody2D;
    public Vector3 gen_point;     // 生成元オブジェクト
    public GameObject explosion;  // 爆発エフェクト
    public GameObject item;       // 落とすアイテム

    public int speed        = 3;  // 速度
    public int direction    = 1;  // 向き
    public int attackPoint  = 10; // 攻撃力
    public int gen_range    = 10; // 生存範囲

    private LifeScript lifeScript;

    //メインカメラのタグ名　constは定数(絶対に変わらない値)
    private const string MAIN_CAMERA_TAG_NAME = "MainCamera";

    //カメラに映っているかの判定
    private bool _isRendered = false;
    
    // 初期設定
    void Start()
    {
        initObject();
    }

    // 初期化
    void initObject()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        lifeScript = GameObject.FindGameObjectWithTag("HP").GetComponent<LifeScript>();
        // 向きを指定する
        speed *= getDirection();
    }
    
    void Update()
    {
        // カメラに映っているときに行動し始める
        if (_isRendered)
        {
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        }

        // 影響範囲外に出た場合削除
        if (Vector2.Distance(transform.position,gen_point) > gen_range)
        {
            EnemyDestroy();
        }
    }

    // 向きを取得
    public int getDirection(int arg_inverted = 0)
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (arg_inverted > 0 && Player.transform.position.x < transform.position.x) { return -1; }
        return 1;
    }
    
    // 弾に当たったとき
    void OnTriggerEnter2D(Collider2D col)
    {
        if (_isRendered)
        {
            if (col.tag == "Bullet")
            {
                EnemyDestroy();
            }
        }
    }

    // Enemy消滅処理
    public void EnemyDestroy(int item_f = 0)
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, transform.rotation);

        // アイテムフラグが建っている場合ランダムでアイテムを落とす
        if (item != null && item_f == 1)
        {
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(item, transform.position, transform.rotation);
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        //Playerとぶつかった時
        if (col.gameObject.tag == "Player")
        {
            //LifeScriptのLifeDownメソッドを実行
            lifeScript.LifeDown(attackPoint);
        }
    }

    //Rendererがカメラに映ってる間に呼ばれ続ける
    void OnWillRenderObject()
    {
        //メインカメラに映った時だけ_isRenderedをtrue
        if (Camera.current.tag == MAIN_CAMERA_TAG_NAME)
        {
            _isRendered = true;
        }
    }
}