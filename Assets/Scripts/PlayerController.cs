using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rb;
  private bool hasAlreadyHitGrab = false;
  private bool isTouchingARock = false;

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
    GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
	}

  private void GameManagerOnGameStateChanged(GameState state)
  {
    if (state == GameState.Grabbing)
    {
      rb.gravityScale = 0;
      Physics2D.gravity = new Vector2(0f, 0f);
      rb.velocity = new Vector2(0, 0);
      hasAlreadyHitGrab = false;
    }
  }

  private void onDestroy()
	{
		GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
	}

	// Update is called once per frame
	void Update()
	{
		if (!(GameManager.instance.gameState == GameState.Grabbing) && Input.GetKeyDown("space") && !hasAlreadyHitGrab)
		{
			hasAlreadyHitGrab = true;
      if (isTouchingARock) 
      {
        GameManager.instance.UpdateGameState(GameState.Grabbing);
      }
		}
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
