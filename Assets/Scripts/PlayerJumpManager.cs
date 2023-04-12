using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJumpManager : MonoBehaviour
{
  public static PlayerJumpManager instance;
  public Image PowerBarMask;
  public float jumpXPosition = 1f;
  private Rigidbody2D rb;
  float maxJumpForce = 9f;
  float currentJumpForce = 0f;
  float jumpForceChangeSpeed = 0.5f;
  bool isIncreasingJumpForce = true;

  void Awake()
  {
    instance = this;
    GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    rb = gameObject.GetComponent<Rigidbody2D>();
  }

  private void GameManagerOnGameStateChanged(GameState state)
  {
    if (state == GameState.Grabbing)
    {
      StartCoroutine(PrepareJump());
    }
  }

  private void onDestroy()
	{
		GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
	}

  void Update()
  {
    if (GameManager.instance.gameState == GameState.Grabbing && Input.GetKeyDown("r"))
    {
      Debug.Log("currentJumpForce" + currentJumpForce);
      rb.gravityScale = 1;
      Physics2D.gravity = new Vector2(0f, -9.81f);
      rb.AddForce(new Vector2 (jumpXPosition, 1f) * currentJumpForce, ForceMode2D.Impulse);
      GameManager.instance.UpdateGameState(GameState.Jumping);
    }
  }

  public void Jump(Rigidbody2D rb)
  {
    StartCoroutine(PrepareJump());
  }

  IEnumerator PrepareJump()
  {
    while(GameManager.instance.gameState == GameState.Grabbing)
    {
      if (isIncreasingJumpForce)
      {
        currentJumpForce += jumpForceChangeSpeed;
        if (currentJumpForce >= maxJumpForce)
        {
          isIncreasingJumpForce = false;
        }
      }
      else
      {
        currentJumpForce -= jumpForceChangeSpeed;
        if (currentJumpForce <= 0)
        {
          isIncreasingJumpForce = true;
        }
      }
      
      float fill = currentJumpForce / maxJumpForce;
      PowerBarMask.fillAmount = fill;
      yield return new WaitForSeconds(0.02f);
    }
    
    yield return null;
  }
}
