using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Check Values")]
    private bool _isGameOver;
    private bool _isGamePaused;

    [Header("Game Objects")]
    private UIManager _uiManager;

// ========================================================================================================

    private void Start()
    {
        FindGameObjects();
        NullChecking();
    }

    private void FindGameObjects()
    {
        _uiManager  = GameObject.Find("UIManager").GetComponent<UIManager>();
    }
    
    private void NullChecking()
    {
        if (_uiManager == null)
            Debug.LogError("'_uiManager' us NULL! Have you named the game object 'UIManager'?");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
            SceneManager.LoadScene(1);
        
        PauseGame();
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isGamePaused = true;
            _uiManager.OnGamePause();
        }
        
        if (_isGamePaused && Input.GetKeyDown(KeyCode.Escape))
        {
            _uiManager.OnGameContinue();
            _isGamePaused = false;
        }
    }
}
