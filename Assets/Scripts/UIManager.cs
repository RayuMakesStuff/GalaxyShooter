using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private Text _scoreText;

    [Header("Game Objects")]
    private Player _player;
    
    // ================================================================
    void Start()
    {
        FindGameObjects();
    }

    void Update()
    {
        _scoreText.text = "Score: " + _player.GetScore();
    }
    
    private void FindGameObjects()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
}
