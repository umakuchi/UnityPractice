using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreaterScript : MonoBehaviour {
    
    public GameObject[] chip;
    public Dropdown drop;
    private ToggleGroup toggleGroup; // スキルトグルグループ

    private void Start()
    {
        if (drop == null) { return; }

        List<string> strs = new List<string>();
        for(int i = 1; i < 10; i++)
        {
            strs.Add("MAP" + i);
        }

        drop.AddOptions(strs);
        toggleGroup = GameObject.FindWithTag("ToggleParent").GetComponent<ToggleGroup>();
    }

    // ロード
    public void Load (int map_id = 0) {

        if (drop != null) { map_id = drop.value + 1; }

        // 表示されているステージを削除
        GameObject[] chips = GameObject.FindGameObjectsWithTag("Chip");
        if (chips != null)
        {
            foreach (GameObject chip_obj in GameObject.FindGameObjectsWithTag("Chip"))
            {
                Destroy(chip_obj);
            }
        }

        List<MapObject.MapChip> mapchips = MapObject.LoadMap(map_id);
        if (mapchips == null) { return; }
        foreach(MapObject.MapChip mapchip in mapchips)
        {
            Instantiate(chip[mapchip.chip_id],mapchip.pos, new Quaternion());
        }
	}

    // セーブ
    public void Save(int id = 0)
    {
        GameObject[] chip_objects = GameObject.FindGameObjectsWithTag("Chip");
        List<MapObject.MapChip> mapchips = new List<MapObject.MapChip>();

        if(drop != null) { id = drop.value + 1; }

        foreach (GameObject chip_obj in chip_objects)
        {
            MapObject.MapChip mapchip = new MapObject.MapChip();
            mapchip.chip_id = chip_obj.GetComponent<ChipScript>().chip_id;
            mapchip.pos = chip_obj.transform.position;
            mapchips.Add(mapchip);
        }

        // セーブ
        MapObject.SaveMap(id, mapchips);
    }


	void Update () {
        // クリックしたとき
        if (Input.GetMouseButtonUp(0))
        {
            if (toggleGroup.ActiveToggles().First().name == "elace")
            {
                Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
                if (collition2d)
                {
                    Destroy(collition2d.transform.gameObject);
                }
            }
            else
            {
                generateChip(int.Parse(toggleGroup.ActiveToggles().First().name));
            }
        }
    }

    public void generateChip(int chip_id)
    {
        // ボタンクリック時は処理をしない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // クリックした位置座標
        Vector3 clickPosition;
        Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 既にある場所は置けない
        Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
        if (collition2d)
        {
            return;
        }

        // ここでの注意点は座標の引数にVector2を渡すのではなく、Vector3を渡すことである。
        // Vector3でマウスがクリックした位置座標を取得する
        clickPosition = Input.mousePosition;

        // 軸修正
        float x = Mathf.Round(Camera.main.ScreenToWorldPoint(clickPosition).x);
        float y = Mathf.Round(Camera.main.ScreenToWorldPoint(clickPosition).y);
        float z = 10.0f;

        // オブジェクト生成 : オブジェクト(GameObject), 位置(Vector3), 角度(Quaternion)
        // ScreenToWorldPoint(位置(Vector3))：スクリーン座標をワールド座標に変換する
        Instantiate(chip[chip_id], new Vector3(x, y, z), new Quaternion());
    }
}
