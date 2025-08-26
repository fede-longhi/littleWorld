using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 movingDirection;

    void FixedUpdate()
    {
        transform.position += movingDirection * moveSpeed * Time.deltaTime;
    }

    public void Move(Vector2 direction)
    {
        movingDirection = direction;
    }
}