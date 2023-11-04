using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    public GameObject explosionPrefab;
    [SerializeField]
    private AudioClip _explosionClip;
    private UI_Manager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        float randomX = Random.Range(-7.88f, 7.88f);

        Vector3 respawnPosition = new Vector3(randomX, 6.5f, 0);
        transform.position = respawnPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.LivesControl();
            }
        }
        else if (other.tag == "Laser")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            _uiManager.UpdateScore();
            Destroy(other.gameObject);
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_explosionClip, Camera.main.transform.position, 1f);
        Destroy(gameObject);
    }
}
