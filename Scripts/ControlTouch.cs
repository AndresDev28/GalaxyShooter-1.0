using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTouch : MonoBehaviour
{
    private float _controlX, _controlY;
    private Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _controlX = touchPos.x - transform.position.x;
                    _controlY = touchPos.y - transform.position.y;
                    break;

                case TouchPhase.Moved:
                    _rigidBody.MovePosition(new Vector2(touchPos.x - _controlX, touchPos.y - _controlY));
                    break;

                case TouchPhase.Ended:
                    _rigidBody.velocity = Vector2.zero;
                    break;
            }
        }
    }
}
