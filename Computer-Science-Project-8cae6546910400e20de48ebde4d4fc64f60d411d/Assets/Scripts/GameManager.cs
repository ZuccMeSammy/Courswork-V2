using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;

    public PlayerMovement player;
    public int money;

    public void SaveState()
    {
        Debug.Log("SaveState");
    }
    public void LoadState()
    {
        Debug.Log("LoadState");
    }
}
