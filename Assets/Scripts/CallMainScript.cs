using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CallMainScript : MonoBehaviour
{

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Main");
    }
}