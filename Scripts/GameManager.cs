using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public bool gameOver = true;
    private UI_Manager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Instantiate(player, Vector3.zero, Quaternion.identity);
                gameOver = false;
                _uiManager.HideTitleScreen();

            }
        }
    }
}
