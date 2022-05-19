using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberUI : ABarUI
{
    public Text text;
    public int digids;

    public override void SetFill(float current, float max)
    {
        text.text = current.ToString(string.Concat("N", digids));
    }
}
