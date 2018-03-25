using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomScript : MonoBehaviour {

    public int zoom_up_max = 20;

    public void Zoom(int zoom)
    {
        if ( ( Camera.main.orthographicSize < 20 && zoom >= 0) || ( Camera.main.orthographicSize > 1 && zoom < 0) )
        Camera.main.orthographicSize += zoom;
    }
}
