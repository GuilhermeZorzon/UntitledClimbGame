using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJumpManager : MonoBehaviour
{
  public static PlayerJumpManager instance;
  public Image PowerBarMask;
  public Image DirectionArrow;
  public float jumpXPosition = 1f;
  private Rigidbody2D rb;
  float maxJumpForce = 9f;
  float currentJumpForce = 0f;
  float jumpForceChangeSpeed = 0.5f;
  bool isIncreasingJumpForce = true;
  public float maxJumpDirection = 1f;
  float currentJumpDirection = 0f;
  float jumpDirectionChangeSpeed = 0.1f;
  bool isIncreasingJumpDirection = true;
  bool isChoosingDirection = true;
  bool canJump = false;
  bool canChooseDirection = false;

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
      isChoosingDirection = true;
      canChooseDirection = false;
      canJump = false;
      PrepareJump();
    }
  }

  private void onDestroy()
	{
		GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
	}

  void Update()
  {
    if (GameManager.instance.gameState == GameState.Grabbing && Input.GetKeyDown("space") && canChooseDirection && isChoosingDirection)
    {
      Debug.Log("currentJumpDirection " + currentJumpDirection);
      isChoosingDirection = false;
      canChooseDirection = false;
      StopCoroutine(PrepareJumpDirection());
      StartCoroutine(PrepareJumpForce());
    }
    else if (GameManager.instance.gameState == GameState.Grabbing && Input.GetKeyDown("space") && canJump && !isChoosingDirection)
    {
      Debug.Log("currentJumpForce " + currentJumpForce);
      rb.gravityScale = 1;
      Physics2D.gravity = new Vector2(0f, -9.81f);
      rb.AddForce(new Vector2 (currentJumpDirection, 1f) * currentJumpForce, ForceMode2D.Impulse);
      GameManager.instance.UpdateGameState(GameState.Jumping);
      StopCoroutine(PrepareJumpForce());
      canJump = false;
    }
  }

  public void PrepareJump()
  {
    StartCoroutine(PrepareJumpDirection());
  }

  IEnumerator PrepareJumpForce()
  {
    while(GameManager.instance.gameState == GameState.Grabbing && !isChoosingDirection)
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
      yield return new WaitForSeconds(0.03f);
      canJump = true;
    }
    
    yield return null;
  }

  IEnumerator PrepareJumpDirection()
  {
    while(GameManager.instance.gameState == GameState.Grabbing && isChoosingDirection)
    {
      if (isIncreasingJumpDirection)
      {
        currentJumpDirection += jumpDirectionChangeSpeed;
        if (currentJumpDirection >= maxJumpDirection)
        {
          isIncreasingJumpDirection = false;
        }
      }
      else
      {
        currentJumpDirection -= jumpDirectionChangeSpeed;
        if (currentJumpDirection <= -1)
        {
          isIncreasingJumpDirection = true;
        }
      }
      
      float rotation = currentJumpDirection * -90f;
      Debug.Log("rotation " + rotation);
      DirectionArrow.transform.rotation = (Quaternion.Euler(0, 0, rotation));
      yield return new WaitForSeconds(0.03f);
      canChooseDirection = true;
    }
    
    yield return null;
  }
}
