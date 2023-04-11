using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rb;
	private bool isGrabbing = false;
  private bool hasAlreadyHitGrab = false;
  private bool isTouchingARock = false;
  private bool isJumping = true;
  public float jumpForce = 10f;
  public float jumpXPosition = 1f;

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!isGrabbing && Input.GetKeyDown("space") && !hasAlreadyHitGrab)
		{
			hasAlreadyHitGrab = true;
      if (isTouchingARock) 
      {
        Grab();
      }
		}
    else if (isGrabbing && Input.GetKeyDown("space") && !isJumping)
    {
      Jump();
    }
	}

	void Grab()
	{
		rb.gravityScale = 0;
    Physics2D.gravity = new Vector2(0f, 0f);
    rb.velocity = new Vector2(0, 0);
    isGrabbing = true;
    isJumping = false;
	}

	void Jump()
	{
    isJumping = true;
    isGrabbing = false;
    hasAlreadyHitGrab = false;
		rb.gravityScale = 1;
    Physics2D.gravity = new Vector2(0f, -9.81f);
    rb.AddForce(new Vector2 (jumpXPosition, 1f) * jumpForce, ForceMode2D.Impulse);
	}

  void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log("Entered trigger");
    if (other.tag == "Rock")
    {
      Debug.Log("isTouchingARock");
      isTouchingARock = true;
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    Debug.Log("Exited trigger");
    if (other.tag == "Rock")
    {
      Debug.Log("stoped TouchingARock");
      isTouchingARock = false;
    }
  }
}
