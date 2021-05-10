using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Asteroid : MonoBehaviour
{
    [Header("Asteroid Properties")] 
    [SerializeField][Range(-50.0f, 50.0f)] private float _rotationSpeed = 5.0f;
    
    [Header("Game Objects")] 
    [SerializeField] private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Player _player;

    [Header("Audio and Sound Effects")]
    [SerializeField] private AudioClip _explosionSound;
    private AudioSource _audioSource;
    
    // ========================================================================================================

    private void Start()
    {
        FindGameObjects();
        NullChecking();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * (_rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _uiManager.DisableInstructionsText();
            _uiManager.EnableUITextElements();
            _player.SetMaximumAmmo();
            _spawnManager.StartSpawning(); // Start the enemy and PowerUp spawning coroutines
            Destroy(this.gameObject, 0.25f);
            _audioSource.Play();
        }
    }

    private void FindGameObjects()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioSource = GameObject.Find("Explosion_AudioManager").GetComponent<AudioSource>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void NullChecking()
    {
        if (_spawnManager == null)
            Debug.LogError("'_spawnManager' is NULL! Have you named the GameObject 'SpawnManager'?");
        
        if (_audioSource == null)
            Debug.LogError("'_audioSource' is NULL! Have you added a 'Audio Source' component?");
        
        if (_uiManager == null)
            Debug.LogError("'_uiManager' us NULL! Have you named the game object 'UIManager'?");
    }
}
