using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FinalPortal", menuName = "Items/FinalPortal", order = 1)]
public class FinalPortal : Item, IUsable
{
    public void Use()
    {
        Remove();
        Player.MyInstance.transform.position = new Vector3(-150, -50, 0);
        Player.MyInstance.MyTarget = null;
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.MyStage = 4;
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        musicManager.playTrack(4);
        Debug.Log("You won scuccess");
    }
}

