using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")] 
    [SerializeField][Range(0.0f, 8.0f)] private float _speed = 5.0f;

    [Header("Position Data")] 
    private Transform _enemyDestroyPoint;
    
    [Header("Game Objects")]
    private Player _player;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    
    [Header("Audio and Sound Effects")]
    [SerializeField] private AudioClip _explosionSound;
    private AudioSource _audioSource;

    // ====================================================================
    
    private void Start()
    {
        FindGameObjects();
        NullChecking();
    }

    private void Update()
    {
        Movement();
    }

    private void FindGameObjects()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _enemyDestroyPoint = GameObject.Find("EnemyDestroyPosition").transform;
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void NullChecking()
    {
        if (_enemyDestroyPoint == null)
            Debug.LogError("'_enemyDestroyPoint' is NULL! Have you named your GameObject 'EnemyDestroyPosition'?");

        if (_player == null)
            Debug.LogError("'_player' is NULL! Have you named your GameObject 'Player'?");

        if (_animator == null)
            Debug.LogError("'_animator' is NULL! Have you assigned the Animation?");
        
        if (_audioSource == null)
            Debug.LogError("'_audioSource' is NULL! Have you added a 'Audio Source' component?");
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < _enemyDestroyPoint.transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _player.AddScore(10);
            EnemyDestroySequence();
        }


        if (other.CompareTag("Player"))
        {
            _player.Damage();
            EnemyDestroySequence();
        }
    }
    
    private void EnemyDestroySequence()
    {
        Destroy(this._boxCollider2D);
        _speed = 0f;
        _animator.SetTrigger("OnEnemyDeath");
        _audioSource.clip = _explosionSound;
        _audioSource.Play();
        Destroy(this.gameObject, 2.5f);
    }
}
