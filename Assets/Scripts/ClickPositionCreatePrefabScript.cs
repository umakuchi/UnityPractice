using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickPositionCreatePrefabScript : MonoBehaviour
{
    // 生成したいPrefab
    public GameObject Prefab;
    // クリックした位置座標
    private Vector3 clickPosition;

    void Update()
    {
        // ボタンクリック時は処理をしない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Prefab == null) { return; }

        // マウス入力で左クリックをした瞬間
        if (Input.GetButtonDown("Fire1"))
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
            Instantiate(Prefab, Camera.main.ScreenToWorldPoint(clickPosition), Prefab.transform.rotation);
        }
    }
}