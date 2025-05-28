using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridInfo : MonoBehaviour
{
    public Vector2Int index;
    public Image image;


    public void StartColorChange(Color color)
    {
        image.DOColor(color, 3f);
    }

}
