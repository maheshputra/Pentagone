using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterPortal : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    IEnumerator LoadGameScene() {
        AsyncOperation gameScene = SceneManager.LoadSceneAsync(sceneName);
        while (gameScene.progress < 1)
        {
            //progressBar.fillAmount = gameScene.progress;
            yield return new WaitForEndOfFrame();
        }
    }



}
