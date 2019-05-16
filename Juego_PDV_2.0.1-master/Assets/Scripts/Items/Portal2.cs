using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portal2", menuName = "Items/Portal2", order = 1)]
public class Portal2 : Item, IUsable
{
    public void Use()
    {
        Remove();
        Player.MyInstance.transform.position = new Vector3(280, -50, 0);
        Player.MyInstance.MyTarget = null;
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.MyStage = 3;
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        musicManager.playTrack(3);
        //Debug.Log("Final music scuccess");
    }
}

