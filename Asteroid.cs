using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the asteroid on the z axis by 20 degrees per second
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

        // If the asteroid goes off the screen, destroy it
        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
    }

    // Check for Laser collision (trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            Instantiate(_explosionPrefab, explosionPosition, Quaternion.identity);
            
            Destroy(other.gameObject);

            _audioSource.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(DestroyAfterSound());
            
            _spawnManager.StartSpawning();
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        while (_audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
