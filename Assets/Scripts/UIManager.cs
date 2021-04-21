using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private Text _scoreText;
    
    [Header("Lives")]
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;

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
    
    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives]; // Update the live visualization based on the currentLives index 
        
        switch (currentLives)
        {
            case 3:
                _livesImage.color = Color.white;
                break;
            case 2:
                _livesImage.color = Color.white;
                break;
            case 1:
                _livesImage.color = Color.red;
                break;
            case 0:
                // _gameOverText.gameObject.SetActive(true);
                // _restartGameText.gameObject.SetActive(true);
                // StartCoroutine(GameOverSequence());
                // _gameManager.GameOver();
                break;
        }
    }
}
