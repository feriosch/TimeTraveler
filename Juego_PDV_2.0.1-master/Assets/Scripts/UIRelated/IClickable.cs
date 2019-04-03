using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IClickable
{
    Image MyIcon { get; set; }
    int MyCount { get; }
    TextMeshProUGUI MyStackText { get; }
}
