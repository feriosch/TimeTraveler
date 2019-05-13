using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XPPotion", menuName = "Items/XPPotion", order = 1)]
public class XPPotion : Item, IUsable
{
    [SerializeField]
    private int xp = 70;

    private float xpDifference;

    public void Use()
    {
        if (Player.MyInstance.MyXP.MyCurrentValue + xp < Player.MyInstance.MyXP.MyMaxValue)
        {
            Remove();
            Player.MyInstance.MyXP.MyCurrentValue += xp;
        }
        else
        {
            Remove();
            xpDifference = Player.MyInstance.MyXP.MyMaxValue - xp;
            Debug.Log("xpdifference: " + xpDifference);
            Player.MyInstance.MyLevel++;
            Player.MyInstance.MyHealth.MyMaxValue += 10;
            Player.MyInstance.MyHealth.MyCurrentValue = Player.MyInstance.MyHealth.MyMaxValue;
            Player.MyInstance.MyXP.MyCurrentValue = xpDifference;

        }
    }
}
