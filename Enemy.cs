using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _pointsOnDestroy = 10;
    private Player _player;
    private Animator _anim;
    private bool _isDestroyed = false;
    private AudioSource _audioSource;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL.");
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(1);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            _audioSource.Play();
            _isDestroyed = true;
            StartCoroutine(DestroyAfterAnimation());
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(_pointsOnDestroy);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            _audioSource.Play();
            _isDestroyed = true;
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        while (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Explosion") && !_isDestroyed)
        {
            yield return null;
        }

        float animationLength = _anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        Destroy(gameObject);
    }
}
