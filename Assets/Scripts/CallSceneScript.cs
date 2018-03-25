using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CallSceneScript : MonoBehaviour
{
    // ソースから呼ぶ場合
    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // ボタン等で呼び出す場合
    public void LoadScenePub(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}