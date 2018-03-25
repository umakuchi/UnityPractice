using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneraterScript : MonoBehaviour {

    public GameObject prefab;           //生成物
    
    public int del_distance = 10;    //生成後消滅距離
    public int max_gen_count = 5;    //最高生成数
    public float gen_rate = 1f;      //生成感覚
    
    private float time = 0.0f;
    
	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
        time += Time.deltaTime;
        if(time > gen_rate)
        {
            time = 0.0f;
            GameObject obj = Instantiate(prefab,transform.position,transform.rotation);
            obj.GetComponent<EnemyScript>().gen_point = transform.position;
            obj.GetComponent<EnemyScript>().gen_range = del_distance;

            max_gen_count--;
            if (max_gen_count <= 0) { Destroy(gameObject); }
        }
	}
}
