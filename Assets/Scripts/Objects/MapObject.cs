using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapObject : MonoBehaviour
{
    const string KEY = "Map";

    // マップデータ
    [SerializeField]
    public class MapData
    {
        [SerializeField]
        public int map_id;
        public List<string> mapchips;
        public void setMapData(int arg_map_id, List<string> arg_mapchips)
        {
            map_id   = arg_map_id;
            mapchips = arg_mapchips;
        }
        public int getMapId()
        {
            return map_id;
        }
        public List<string> getMapchips()
        {
            return mapchips;
        }
    }

    // マップチップデータ
    [SerializeField]
    public class MapChip
    {
        [SerializeField]
        public int tile_id;
        public int chip_id;
        public Vector3 pos;
        public void setMapchip(int arg_tile_id, int arg_chip_id, Vector3 arg_pos)
        {
            tile_id = arg_tile_id;
            chip_id = arg_chip_id;
            pos = arg_pos;
        }
        public int getMapchipTileId()
        {
            return tile_id;
        }
        public int getMapchipChipId()
        {
            return chip_id;
        }
        public Vector3 getMapChipPos()
        {
            return pos;
        }
    }

    //-----------------------------------------------------
    /// <summary>
    /// マップデータを保存する
    /// </summary>
    /// <param name="map_id"></param>
    /// <param name="mapchips"></param>
    //-----------------------------------------------------
    public static void SaveMap(int map_id,List<MapChip> mapchips)
    {
        List<string> maps = new List<string>();
        foreach (MapChip mapchip in mapchips)
        {
            string serizlizedList2 = JsonUtility.ToJson(mapchip);
            maps.Add(serizlizedList2);
        }
        MapData md = new MapData();
        md.map_id = map_id;
        md.mapchips = maps;

        SaveData.SetClass<MapData>(KEY + map_id, md);
        SaveData.Save();
    }
    //-----------------------------------------------------
    /// <summary>
    /// マップデータをロードする
    /// </summary>
    /// <param name="map_id"></param>
    /// <returns>List<MapChip></returns>
    //-----------------------------------------------------
    public static List<MapChip> LoadMap(int map_id)
    {
        MapData md = SaveData.GetClass<MapData>(KEY + map_id, new MapData());

        // セーブデータが存在しない場合nullを返す
        if(md == null) { return null; }

        List<string> maps = md.mapchips;
        List<MapChip> mapchips = new List<MapChip>();
        
        if(maps == null) { return null; }

        foreach (string list in maps)
        {
            MapChip mp = JsonUtility.FromJson<MapChip>(list);
            mapchips.Add(mp);
        }

        return mapchips;
    }
}