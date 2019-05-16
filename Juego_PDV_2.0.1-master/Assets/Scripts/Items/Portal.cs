using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portal1", menuName = "Items/Portal1", order = 1)]
public class Portal : Item, IUsable
{
    public void Use()
    {
        Remove();
        Player.MyInstance.transform.position = new Vector3(120, -50, 0);
        Player.MyInstance.MyTarget = null;
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.MyStage = 2;
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        musicManager.playTrack(2);
        //Debug.Log("Music scuccess");
    }
}
