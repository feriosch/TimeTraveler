using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XP : Stat
{
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    [SerializeField]
    private SpellBook spellbook;

    [SerializeField]
    private TextMeshProUGUI levelText;

    new public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value >= MyMaxValue)
            {
                currentValue = 0;
                MyInstance.MyLevel++;
                MyInstance.MyHealth.MyMaxValue += 10;
                MyInstance.MyHealth.MyCurrentValue = MyInstance.MyHealth.MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;

            if (statValue != null)
            {
                statValue.text = currentValue + "";
            }

        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        levelText.text = Player.MyInstance.MyLevel.ToString();
    }
}
