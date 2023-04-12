using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  
  public GameState gameState = GameState.Jumping;

  public static event Action<GameState> OnGameStateChanged;

  void Awake()
  {
    instance = this;
  }

  public void UpdateGameState(GameState newState)
  {
    if(this.gameState == newState) 
    {
      return;
    }
    this.gameState = newState;
    switch (newState)
    {
      case GameState.Jumping:
        onGameStateChangeToJumping();
        break;
      case GameState.Grabbing:
        onGameStateChangeToGrabbing();
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(newState), newState, "A new non subscribed event was triggered");
    }

    OnGameStateChanged?.Invoke(newState);
  }

  private void onGameStateChangeToJumping()
  {
    // Do nothing
  }
    
  private void onGameStateChangeToGrabbing()
  {
    // Do nothing
  }
}

public enum GameState {
  Jumping,
  Grabbing,
}
