using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;

    [SerializeField]
    private AudioClip _laser_ShotClip;
    
    private AudioSource _audioSource;

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        GameObject spawnManagerObject = GameObject.Find("Spawn_Manager");
        GameObject uiManagerObject = GameObject.Find("Canvas");
        GameObject gameManagerObject = GameObject.Find("Game_Manager");
        _audioSource = GetComponent<AudioSource>();

        if (spawnManagerObject != null)
        {
            _spawnManager = spawnManagerObject.GetComponent<SpawnManager>();
        }
        else
        {
            Debug.LogError("Spawn_Manager GameObject is NULL.");
        }

        if (uiManagerObject != null)
        {
            _uiManager = uiManagerObject.GetComponent<UIManager>();
        }
        else
        {
            Debug.LogError("Canvas GameObject is NULL.");
        }

        if (gameManagerObject != null)
        {
            _gameManager = gameManagerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("Game_Manager GameObject is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laser_ShotClip;
        }

        _shieldVisualizer.SetActive(false);
        _score = 0;
        _uiManager.UpdateScore(_score);
        _uiManager.UpdateLives(_lives);
    }

    private void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        float currentSpeed = _isSpeedBoostActive ? _speed * _speedMultiplier : _speed;
        transform.Translate(direction * currentSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 5), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

 private void FireLaser()
{
    _canFire = Time.time + _fireRate;

    if (_isTripleShotActive == true)
    {
        Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
    }
    else
    {
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        //play laser_shot audio clip
        _audioSource.Play();
    }
}


public void TripleShotActive()
{
    _isTripleShotActive = true;
    StartCoroutine(TripleShotPowerDownRoutine());
}

IEnumerator TripleShotPowerDownRoutine()
{
    yield return new WaitForSeconds(5.0f);
    _isTripleShotActive = false;
}

public void SpeedBoostActive()
{
    _isSpeedBoostActive = true;
    _speed *= _speedMultiplier;
    StartCoroutine(SpeedBoostPowerDownRoutine());
}

IEnumerator SpeedBoostPowerDownRoutine()
{
    yield return new WaitForSeconds(5.0f);
    _isSpeedBoostActive = false;
    _speed /= _speedMultiplier;
}

public void ShieldActive()
{
    _isShieldActive = true;
    _shieldVisualizer.SetActive(true);
}

public void AddScore(int points)
{
    _score += points;
    _uiManager.UpdateScore(_score);
}

public void Damage(int damageAmount)
{
    if (_isShieldActive)
    {
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
        return;
    }

    _lives -= damageAmount;

    //if lives is 2 enable the right engine else if lives is 1 enable the left engine
    if (_lives == 2)
    {
        _rightEngine.SetActive(true);
    }
    else if (_lives == 1)
    {
        _leftEngine.SetActive(true);
    }
    //update the lives on the screen
    _uiManager.UpdateLives(_lives);
}

}




   
