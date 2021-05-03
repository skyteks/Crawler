using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ABarUI : MonoBehaviour
{
    public abstract void SetFill(float current, float max);
}

public class BarUI : ABarUI
{
    public Image image;

    public Range customFillRange = new Range(0f, 1f);

    public override void SetFill(float current, float max)
    {
        image.fillAmount =  current.LinearRemap(0f, max, customFillRange.min, customFillRange.max);
    }
}
