using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int map_id = 1; // マップID

	void Start () {
        // マップ生成
        GameObject.Find("CreaterScript").GetComponent<CreaterScript>().Load(map_id);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
