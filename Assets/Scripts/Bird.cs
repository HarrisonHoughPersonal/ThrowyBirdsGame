﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* AUTHOR: Harrison Hough   
* COPYRIGHT: Harrison Hough 2019
* VERSION: 1.0
* SCRIPT: Bird Class
*/

/// <summary>
/// 
/// </summary>
public class Bird : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private CircleCollider2D circleCollider2D;
    public LayerMask birdLayer;
    private float mass;
    public float Mass { get { return mass; } }

    [SerializeField]
    private float disableDelay = 3f;
    private bool hasHitGround = false;

    PhysicsMaterial2D birdMaterial;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// Runs on every collison with this object
    /// Used to start DisableAfterDelay coroutine
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Ground") && !hasHitGround)
        {
            hasHitGround = true;
            StartCoroutine(DisableAfterDelay());
        }
    }

    /// <summary>
    /// Checks if cursor is currently over this physics object
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public bool IsCursorOverBird(Vector3 location)
    {
        if (circleCollider2D == Physics2D.OverlapPoint(location, birdLayer))
        {
            Debug.Log("TRUE");
            return true;
        }
        Debug.Log("FALSE");
        return false; 
    }

    /// <summary>
    /// This function enables physics and sets move velocity
    /// </summary>
    /// <param name="velocity"></param>
    public void OnThrow(Vector2 velocity)
    {
        rigidbody2D.isKinematic = false;

        rigidbody2D.velocity = velocity;
    }

    /// <summary>
    /// This routine slows down and destroys/disables 
    /// this gameobject after a delay
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableAfterDelay()
    {
        while (rigidbody2D.velocity.y != 0)
        {
            yield return null;
        }

        float timeToDisable = Time.time + 2;
        while(timeToDisable > Time.time)
        {
            yield return null;
        }

        Vector3 startVelocity = rigidbody2D.velocity;
        //wait until velocity is slow
        while (Mathf.Abs( rigidbody2D.velocity.x) > 0.5 )
        {
            Debug.Log("Waiting to slow down");
            rigidbody2D.velocity = rigidbody2D.velocity * 0.95f * Time.deltaTime;
            yield return null;
        }
        //wait few seconds
        timeToDisable = Time.time + disableDelay;
        Debug.Log("now wait " + timeToDisable + " seconds");
        while (timeToDisable > Time.time)
        {
            yield return null;
        }

        //TODO notify GameManager
        GameManager.Instance.DestroyBird();
        //disable
        gameObject.SetActive(false);
    }
}
