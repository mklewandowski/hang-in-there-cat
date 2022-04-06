using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    bool IsRight;
    public bool MovingRight;
    public bool MovingLeft;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsRight)
        {
            MovingRight = true;
        }
        else
        {
            MovingLeft = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsRight)
        {
            MovingRight = false;
        }
        else
        {
            MovingLeft = false;
        }
    }

    public void Clear()
    {
        MovingRight = false;
        MovingLeft = false;
    }
}
