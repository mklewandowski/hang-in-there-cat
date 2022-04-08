using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    MobileButton ButtonLeft;
    [SerializeField]
    MobileButton ButtonRight;

    Vector2 movementVector = new Vector2(0, 0);
    float gravity = -20f;
    float minX = -3.4f;
    float maxX = 3.4f;
    bool moveLeft;
    bool moveRight;
    float moveSpeed = 6f;

    // Update is called once per frame
    void Update()
    {
        if (Globals.CurrentGameState == Globals.GameState.Playing)
        {
            moveLeft = Input.GetKey(KeyCode.LeftArrow) || ButtonLeft.MovingLeft;
            moveRight = Input.GetKey(KeyCode.RightArrow) || ButtonRight.MovingRight;

            if (moveLeft)
                movementVector.x = moveSpeed * -1f;
            else if (moveRight)
                movementVector.x = moveSpeed;
            else
                movementVector.x = 0f;
            GetComponent<Rigidbody2D>().velocity = movementVector;
            //don't let cat go offscreen to the right or left
            if (transform.localPosition.x > maxX)
            {
                Vector2 boundedPos = new Vector2 (maxX, transform.localPosition.y);
                transform.localPosition = boundedPos;
            }
            if (transform.localPosition.x < minX)
            {
                Vector2 boundedPos = new Vector2 (minX, transform.localPosition.y);
                transform.localPosition = boundedPos;
            }
        }
        else if (Globals.CurrentGameState == Globals.GameState.Dying)
        {
            movementVector.x = 0f;
            movementVector.y += gravity * Time.deltaTime;
            GetComponent<Rigidbody2D>().velocity = movementVector;

            if (transform.localPosition.y < -10f)
            {
                movementVector.x = 0f;
                movementVector.y = 0f;
                GetComponent<Rigidbody2D>().velocity = movementVector;
                Globals.CurrentGameState = Globals.GameState.ShowScoreAllowRestart;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}
