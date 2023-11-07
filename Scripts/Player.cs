using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShootPrefab;
    [SerializeField]
    private float _fireRate = 0.25f;
    [SerializeField]
    private float _nextFire = 0.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _shieldGameobjetc;
    [SerializeField]
    private GameObject _thrustersGameobject;
    [SerializeField]
    private GameObject _boostThrustersGameobject;
    [SerializeField]
    private GameObject[] _engines;

    private int _hitCount = 0;
    private UI_Manager _uimanager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    public int lives = 3;

    public bool canTripleShoot = false;
    public bool speedBoost = false;
    public bool shieldActivate = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _uimanager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

        if (_uimanager != null)
        {
              _uimanager.UpdateLives(lives);
        }
          _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

           _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

          if (_spawnManager != null)
        {
               _spawnManager.StartSpawnRutine();
        }

        _audioSource = GetComponent<AudioSource>();

        _hitCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
         if (_gameManager.gameOver == false)
        {
            Movement();

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButton(0))
            {
                Shoot();
            }
        }
    }
    private void Movement()
    {
        if (speedBoost == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * Time.deltaTime * _speed * 2f * horizontalInput);

            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * Time.deltaTime * _speed * 2f * verticalInput);
        }
        else
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontalInput);

            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput);
        }

        //Restricciones límites de pantalla
        // if player position y > 0
        // set the position to x = x Current_position y = 0
        //...

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }

        if (transform.position.x < -8.2f)
        {
            transform.position = new Vector3(-8.2f, transform.position.y, 0);
        }
        else if (transform.position.x > 8.2f)
        {
            transform.position = new Vector3(8.2f, transform.position.y, 0);
        }
    }

    private void Shoot()
    {
        _audioSource.Play();

        if (Time.time > _nextFire)
        {
            if (canTripleShoot == true)
            {
                Instantiate(_tripleShootPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.92f, 0), Quaternion.identity);
            }

            //Sistema de cooldown
            _nextFire = Time.time + _fireRate;
        }
    }

    public void TripleShootPowerOn()
    {
        canTripleShoot = true;

        StartCoroutine(TripleShootPowerDownRutine());
    }
    public IEnumerator TripleShootPowerDownRutine()
    {
        yield return new WaitForSeconds(5.0F);

        canTripleShoot = false;
    }

    public void SpeedBoostPowerOn()
    {
        speedBoost = true;
        _thrustersGameobject.SetActive(false);
        _boostThrustersGameobject.SetActive(true);

        StartCoroutine(SpeedBoostDownRutine());
    }

    public IEnumerator SpeedBoostDownRutine()
    {
        yield return new WaitForSeconds(5.0f);

        speedBoost = false;
        _thrustersGameobject.SetActive(true);
        _boostThrustersGameobject.SetActive(false);
    }

    public void EnableShield()
    {
        shieldActivate = true;
        _shieldGameobjetc.SetActive(true);
    }

    public void LivesControl()
    {
        
        if (shieldActivate == true)
        {
            shieldActivate = false;
            _shieldGameobjetc.SetActive(false);
            return;
        }

        _hitCount++;

        if (_hitCount == 1)
        {
            _engines[0].SetActive(true);
        }
        else if (_hitCount == 2)
        {
            _engines[1].SetActive(true);
        }

        lives -= 1;
        _uimanager.UpdateLives(lives);
        

        if (lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uimanager.ShowTitleScreen();
            _uimanager.ResetScore();
            Destroy(gameObject);
        }
    }
}
