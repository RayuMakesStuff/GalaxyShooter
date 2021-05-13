using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Properties")] 
    [SerializeField][Range(2.0f, 15.0f)] private float _speed = 4.0f;
    [SerializeField][Range(0, 5)] private int _lives = 3; 
    [SerializeField][Range(1.0f, 5.0f)] private float _shiftSpeedBoost = 3.5f;
    [SerializeField][Range(0.2f, 1.0f)] private float _laserOffset;
    private int _currentAmmo;
    [SerializeField][Range(10, 20)] private int _maximumAllowedAmmo = 15;
    
    [Header("Damage Indicators")]
    [SerializeField] private GameObject _leftDamageVisualizer;
    [SerializeField] private GameObject _rightDamageVisualizer;

    [Header("Position Data")] 
    private Transform _playerSpawnPosition;
    private Transform _leftPlayerBorder;
    private Transform _rightPlayerBorder;
    private Transform _topPlayerBorder;
    private Transform _bottomPlayerBorder;
    
    [Header("Debug Values")] 
    private float _startSpeedValue;
    // start speed value to save the initial speed value
    // -> prevent shift speed boost to interfere 

    [Header("Game Objects")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private bool _isShieldActive;

    [Header("Laser Cooldown System")]
    [SerializeField] [Range(0.1f, 1.5f)] private float _cooldownTime = 0.25f;
    private float _nextFire = 0.0f;

    [Header("NoAmmo Sound - Cooldown System")] 
    [SerializeField] [Range(0.4f, 0.6f)] private float _playbackCooldownTime = 0.5f;
    private float _nextPlayback = 0.0f;
    
    [Header("Triple Shot")]
    [SerializeField][Range(2.0f, 10.0f)] private float _tripleShotDuration = 5.0f;
    private bool _isTripleShotActive;
    [SerializeField] private GameObject _tripleLaserPrefab;

    [Header("Speed Boost")]
    [SerializeField][Range(2.0f, 10.0f)] private float _speedBoostDuration;
    private bool _isSpeedBoostActive;
    private bool _shiftSpeedBoostActive = false;
    
    [Header("Shield")]
    private int _shieldCounter = 0;
    
    [Header("Audio and Sound Effects")]
    [SerializeField] private AudioClip _laserFireSound;
    [SerializeField] private AudioClip _noAmmoSound;
    [SerializeField] private AudioClip _gameOverSound;
    private AudioSource _audioSource;
    private AudioSource _backgroundAudioSource;
    private AudioSource _noAmmoAudioSource;
    private AudioSource _gameOverAudioSource;
    
    [Header("UI Elements")] 
    private int _score;

    private CameraShake _cameraShake;

    // ========================================================

    private void Start()
    {
        FindGameObjects();
        NullChecking();
        ResetSpawnPosition();
        UpdateDamageVisualizer();

        _startSpeedValue = _speed;
        _currentAmmo = _maximumAllowedAmmo;
        _score = 0;
        _uiManager.UpdateLives(_lives);

        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
    }

    private void Update()
    {
        Movement();
        PlayerBorders();
        InstantiateLaser();
        ShiftSpeedBoost();
    }

    private void FindGameObjects()
    {
        _playerSpawnPosition = GameObject.Find("PlayerSpawnPosition").transform;
        _leftPlayerBorder = GameObject.Find("PlayerBorder_left").transform;
        _rightPlayerBorder = GameObject.Find("PlayerBorder_right").transform;
        _topPlayerBorder = GameObject.Find("PlayerBorder_top").transform;
        _bottomPlayerBorder = GameObject.Find("PlayerBorder_bottom").transform;
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _backgroundAudioSource = GameObject.Find("BackgroundMusic_AudioManager").GetComponent<AudioSource>();
        _noAmmoAudioSource = GameObject.Find("NoAmmo_AudioManager").GetComponent<AudioSource>();
        _gameOverAudioSource = GameObject.Find("GameOver_AudioManager").GetComponent<AudioSource>();
    }

    private void NullChecking()
    {
        if (_playerSpawnPosition == null)
            Debug.LogError("'_playerSpawnPosition' is NULL! Have you named the GameObject 'PlayerSpawnPosition'?");

        if (_topPlayerBorder == null)
            Debug.LogError("'_topPlayerBorder' is NULL! Have you named the GameObject 'PlayerBorder_top'?");
        
        if (_rightPlayerBorder == null)
            Debug.LogError("'_rightPlayerBorder' is NULL! Have you named the GameObject 'PlayerBorder_right'?");

        if (_leftPlayerBorder == null)
            Debug.LogError("'_leftPlayerBorder' is NULL! Have you named the GameObject 'PlayerBorder_left'?");

        if (_bottomPlayerBorder == null)
            Debug.LogError("'_bottomPlayerBorder' is NULL! Have you named the GameObject 'PlayerBorder_bottom'?");

        if (_spawnManager == null)
            Debug.LogError("'_spawnManager' is NULL! Have you named the GameObject 'SpawnManager'?");
    }

    private void ResetSpawnPosition()
    {
        transform.position = GameObject.Find("PlayerSpawnPosition").transform.position;
    }

    private void Movement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalMovement, verticalMovement, 0) * (_speed * Time.deltaTime));
    }

    private void PlayerBorders()
    {
        var position = transform.position;

        // left border
        if (position.x < _leftPlayerBorder.transform.position.x)
            position = new Vector3(_rightPlayerBorder.transform.position.x, transform.position.y, 0);

        // right border
        if (position.x > _rightPlayerBorder.transform.position.x)
            position = new Vector3(_leftPlayerBorder.transform.position.x, transform.position.y, 0);

        // y-restriction clamping
        position = new Vector3(position.x, Mathf.Clamp(position.y, _bottomPlayerBorder.transform.position.y,
            _topPlayerBorder.transform.position.y), 0);
        transform.position = position;
    }

    private void InstantiateLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _currentAmmo > 0)
        {
            if (_isTripleShotActive)
            {
                Instantiate(_tripleLaserPrefab, transform.position + (_tripleLaserPrefab.transform.up * _laserOffset), Quaternion.identity);
            }
            
            else
            {
                Instantiate(_laserPrefab, transform.position + (_laserPrefab.transform.up * _laserOffset), Quaternion.identity);
            }
            
            // Cooldown System
            _nextFire = _cooldownTime + Time.time;
            _currentAmmo--;
            _audioSource.clip = _laserFireSound;
            _audioSource.Play();
            
            if (_currentAmmo == 0) // Play sound when ammo reaches 0
            {
                _audioSource.clip = _noAmmoSound;
                _audioSource.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && _currentAmmo <= 0 && Time.time > _nextPlayback)
        {
            _nextPlayback = _playbackCooldownTime + Time.time;
            _noAmmoAudioSource.clip = _noAmmoSound;
            _noAmmoAudioSource.Play();
        }
    }

    public void Damage()
    {
        StartCoroutine(_cameraShake.Shake(0.5f, 0.5f));
        
        if (_isShieldActive)
        {
            _shieldCounter--;
            UpdateShieldVisualizer();
            
            if (_shieldCounter == 0)
            {
                _isShieldActive = false; 
                _shieldVisualizer.gameObject.SetActive(false);
            }

            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);
        
        UpdateDamageVisualizer();

        if (_lives <= 0)
        {
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
            _backgroundAudioSource.Stop();
            _gameOverAudioSource.clip = _gameOverSound;
            _gameOverAudioSource.Play();
        }
    }
    
    private void ShiftSpeedBoost()
    {
        if (_isSpeedBoostActive == false && Input.GetKeyDown(KeyCode.LeftShift) && _shiftSpeedBoostActive == false)
        {
            _shiftSpeedBoostActive = true;
            _speed += _shiftSpeedBoost;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && _shiftSpeedBoostActive)
        {
            _speed -= _shiftSpeedBoost;
            _shiftSpeedBoostActive = false;
        }
    }

    public void UpdateDamageVisualizer()
    {
        switch (_lives)
        {
            case 3:
                _leftDamageVisualizer.gameObject.SetActive(false);
                _rightDamageVisualizer.gameObject.SetActive(false);
                break;
            case 2:
                _leftDamageVisualizer.gameObject.SetActive(true);
                _rightDamageVisualizer.gameObject.SetActive(false);
                break;
            case 1:
                _leftDamageVisualizer.gameObject.SetActive(true);
                _rightDamageVisualizer.gameObject.SetActive(true);
                break;
            default:
                _leftDamageVisualizer.gameObject.SetActive(false);
                _rightDamageVisualizer.gameObject.SetActive(false);
                break;
        }
    }

    public void ActivateTripleShot()
    {
        StartCoroutine(TripleShotRoutine());
    }

    private IEnumerator TripleShotRoutine()
    {
        _isTripleShotActive = true;
        yield return new WaitForSeconds(_tripleShotDuration);
        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoost()
    {
        StartCoroutine(SpeedBoostRoutine());
    }
    
    private IEnumerator SpeedBoostRoutine()
    {
        _isSpeedBoostActive = true; 
        _speed = _startSpeedValue;
        _shiftSpeedBoostActive = false; // Disable the ShiftSpeedBoost()
        _speed = 10;
        yield return new WaitForSeconds(_speedBoostDuration);
        _speed = _startSpeedValue;
        _isSpeedBoostActive = false;
    }
    
    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldVisualizer.gameObject.SetActive(true);
        _shieldCounter++;
        UpdateShieldVisualizer();

        if (_shieldCounter > 3)
            _shieldCounter = 3;
    }

    private void UpdateShieldVisualizer()
    {
        switch (_shieldCounter)
        {
            case 3:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                break;
            case 2:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float) 0.6);
                break;
            case 1:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float) 0.3);
                break;
            case 0:
                _isShieldActive = false; 
                _shieldVisualizer.gameObject.SetActive(false);
                break;
        }
    }
    
    public void AddScore(int pointsToAdd)
    {
        _score += pointsToAdd;
    }
    
    public int GetScore()
    {
        return _score;
    }

    public void SetMaximumAmmo()
    {
        _currentAmmo = _maximumAllowedAmmo;
    }
    
    public int GetCurrentAmmo()
    {
        return _currentAmmo;
    }

    public int GetMaximumAmmoAmount()
    {
        return _maximumAllowedAmmo;
    }
    
    public int GetCurrentShieldCounter()
    {
        return _shieldCounter;
    }
    
    public void AddAmmo(int ammoToAdd)
    {
        _currentAmmo += ammoToAdd;
        ResetMaximumAmmo();
    }
    
    private void ResetMaximumAmmo()
    {
        if (_currentAmmo >= _maximumAllowedAmmo)
        {
            _currentAmmo = _maximumAllowedAmmo;
        }
    }
    
    public void AddLive() 
    { 
        if (_lives < 3)
            _lives++;
    }

    public int GetLives()
    {
        return _lives;
    }
}
