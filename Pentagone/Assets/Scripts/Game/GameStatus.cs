using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using UnityEngine.EventSystems;

public class GameStatus : MonoBehaviour
{
    public static GameStatus instance;
    [SerializeField] public bool isPaused;
    [SerializeField] private UIView gameplayMenu;
    [SerializeField] private EventSystem eventSystem;

    [Space(10)]
    [Header("PauseMenu")]
    [SerializeField] private GameObject firstSelectedGameObject;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        CheckPause();
    }

    private void CheckPause()
    {
        if (!gameplayMenu.IsHidden && !isPaused)
        {
            PauseGame();
        }
        else if (gameplayMenu.IsHidden && isPaused)
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        eventSystem.SetSelectedGameObject(firstSelectedGameObject);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        isPaused = false;
    }
}
