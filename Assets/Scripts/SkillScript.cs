using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class SkillScript : MonoBehaviour {
    
    // 生成したいPrefab
    public GameObject bulletPrefab; //玉発射用
    public GameObject genPrefab;    //スポーン用
    // クリックした位置座標
    private Vector3 clickPosition;

    // スキルを使用
    public void useSkill(string skillName)
    {
        // ボタンクリック時は処理をしない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        switch (skillName)
        {
            //玉発射
            case "Shot":
                Shot();
                break;
            //自爆
            case "Destroy":
                destroy();
                break;
            // スポーンスキル
            case "Generate":
                Generate();
                break;
        }

    }

    //玉発射
    void Shot()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
    }

    //じばく
    void destroy()
    {
        Destroy(gameObject);
    }

    // 生成
    void Generate()
    {
        Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
        if (collition2d)
        {
            Debug.Log("おけない");
            return;
        }

        // ここでの注意点は座標の引数にVector2を渡すのではなく、Vector3を渡すことである。
        // Vector3でマウスがクリックした位置座標を取得する
        clickPosition = Input.mousePosition;
        // Z軸修正
        clickPosition.z = 10f;
        // オブジェクト生成 : オブジェクト(GameObject), 位置(Vector3), 角度(Quaternion)
        // ScreenToWorldPoint(位置(Vector3))：スクリーン座標をワールド座標に変換する
        Instantiate(genPrefab, Camera.main.ScreenToWorldPoint(clickPosition), genPrefab.transform.rotation);
    }
}
