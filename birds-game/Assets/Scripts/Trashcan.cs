using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Trashcan : MonoBehaviour
{
    [SerializeField] private float _maxSpawnTime = 5f;
    [SerializeField] private float _minSpawnTime = 2f;
    [SerializeField] private float _spawnTimer = 5f;
    [SerializeField] private List<GameObject> _trashPrefabs;
    [SerializeField] private Transform _spawnPosition;

    private Rigidbody2D _rigidbody2D;

    private bool _kicked = false;
    
    private float _currentTimer;
    void Start()
    {
        _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0)
        {
            var prefabIndex = Random.Range(0, _trashPrefabs.Count);
            var trash = Instantiate(_trashPrefabs[prefabIndex], _spawnPosition.position, Quaternion.identity);
            var trashRigidbody = trash.GetComponent<Rigidbody2D>();
            var force = new Vector2(-1, 0.5f);
            trashRigidbody.AddForce( force * 5f, ForceMode2D.Impulse);
            _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var birdLayer = LayerMask.NameToLayer("Player");
        
        if (collision.collider.gameObject.layer == ~birdLayer && !_kicked)
        {
            var birdPosition = collision.collider.gameObject.transform.position;
            var force = (birdPosition + transform.position) * 2f;
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
            //TODO: Damage bird    
        };
    }
}
