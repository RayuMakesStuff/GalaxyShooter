using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private Text _scoreText;
    
    [Header("Lives")]
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;
    
    [Header("Game Over")] 
    [SerializeField] private GameObject _gameOverStorage;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartGameText;

    [Header("Instructions")] 
    [SerializeField] private Text _howToStartText;
    
    [Header("Ammunition")] 
    [SerializeField] private Text _currentAmmoCountText;
    [SerializeField] private Text _maximumAmmoCountText;    
    
    [Header("Shields")] 
    [SerializeField] private Text _currentShieldsText;
    [SerializeField] private Text _maximumshieldsText;

    [Header("Game Objects")]
    private Player _player;
    private GameManager _gameManager;
    
    // ================================================================

    private void Start()
    {
        FindGameObjects();
        DisableGameObjectsOnStart();
    }

    private void Update()
    {
        _scoreText.text = "Score: " + _player.GetScore();
        _currentAmmoCountText.text = "Ammo: " + _player.GetCurrentAmmo();
        _maximumAmmoCountText.text = " / " + _player.GetMaximumAmmoAmount();
        _currentShieldsText.text = "Shields: " + _player.GetCurrentShieldCounter();
        _maximumshieldsText.text = " / 3";
        
        ChangeAmmoTextColor();
    }
    
    private void FindGameObjects()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _currentAmmoCountText = GameObject.Find("CurrentAmmo_text").GetComponent<Text>();
        _maximumAmmoCountText = GameObject.Find("MaximumAmmo_text").GetComponent<Text>();
        _currentShieldsText = GameObject.Find("CurrentShields_text").GetComponent<Text>();
        _maximumshieldsText = GameObject.Find("MaximumShields_text").GetComponent<Text>();
    }

    private void DisableGameObjectsOnStart()
    {
        _scoreText.gameObject.SetActive(false);
        _gameOverStorage.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
        _currentAmmoCountText.gameObject.SetActive(false);
        _maximumAmmoCountText.gameObject.SetActive(false);
        _currentShieldsText.gameObject.SetActive(false);
        _maximumshieldsText.gameObject.SetActive(false);  
    }

    public void DisableInstructionsText()
    {
        _howToStartText.gameObject.SetActive(false);
    }

    public void EnableUITextElements()
    {
        _currentAmmoCountText.gameObject.SetActive(true);
        _maximumAmmoCountText.gameObject.SetActive(true); 
        _currentShieldsText.gameObject.SetActive(true);
        _maximumshieldsText.gameObject.SetActive(true);  
        _scoreText.gameObject.SetActive(true);
    }
    
    private void ChangeAmmoTextColor()
    {
        if (_player.GetCurrentAmmo() <= 5)
        {
            _currentAmmoCountText.color = Color.red;
            _maximumAmmoCountText.color = Color.red;
        }
        
        else if (_player.GetCurrentAmmo() > 5)
        {
            _currentAmmoCountText.color = Color.white;
            _maximumAmmoCountText.color = Color.white;
        }
    }

    private IEnumerator GameOverSequence()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(1.0f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(1.0f);
        }
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
                _gameOverStorage.gameObject.SetActive(true);
                _gameOverText.gameObject.SetActive(true);
                _restartGameText.gameObject.SetActive(true);
                StartCoroutine(GameOverSequence());
                _gameManager.GameOver();
                break;
        }
    }
}
