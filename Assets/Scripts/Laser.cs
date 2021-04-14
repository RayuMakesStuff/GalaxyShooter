using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Laser Properties")] 
    [SerializeField][Range(2.0f, 10.0f)] private float _speed = 6.0f;

    [Header("Position Data")]
    private Transform _laserDestroyPosition;
        
    // ===============================================

    private void Start()
    {
        FindGameObjects();
        NullChecking();
    }

    private void Update()
    {
        transform.Translate(Vector3.up * (_speed * Time.deltaTime));
        
        if (transform.position.y > _laserDestroyPosition.transform.position.y)
                Destroy(gameObject);
    }

    private void FindGameObjects()
    {
        _laserDestroyPosition = GameObject.Find("LaserDestroyPosition").transform;
    }

    private void NullChecking()
    {
        if (_laserDestroyPosition == null)
        {
            Debug.LogError("'_laserDestroyPosition' is NULL! " + 
                           "Have you named the GameObject 'LaserDestroyPosition'?");
        }
    }
}
