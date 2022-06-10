using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages game state between scenes
/// </summary>
public class GameManager
{
    public static GameManager Instance { get; private set;}
    public static void Init()
    {
        Instance = new GameManager();
    }
    public void GetDamaged()
    {

    }

    private void EndGame()
    {

    }
}
