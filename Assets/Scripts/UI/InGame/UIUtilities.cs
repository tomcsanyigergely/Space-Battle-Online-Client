using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUtilities
{
    public static void Hide(Canvas canvas)
    {
        canvas.transform.localPosition = new Vector3(0, 0, -1000);
    }

    public static void Show(Canvas canvas)
    {
        canvas.transform.localPosition = new Vector3(0, 0, 0);
    }

    public static void Hide(GameObject gameObject)
    {
        Vector3 localPos = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector3(localPos.x, localPos.y, -1000);
    }

    public static void Show(GameObject gameObject)
    {
        Vector3 localPos = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
    }
}
