using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointUtil : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IPointerExitHandler
{
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    public float durationThreshold = 0.2f;

    public UnityEvent onLongPress = new UnityEvent();

    public bool isPointerDown = false;
    public bool longPressTriggered = false;
    public float timePressStarted;

    public void FixedUpdate()
    {
        //        if (isPointerDown && !longPressTriggered)
        //        {
        //            if (Time.time - timePressStarted > durationThreshold)
        //            {
        //                longPressTriggered = true;
        //                LogUtil.Log("changanshijian:"+ (Time.time - timePressStarted));
        //            }
        //        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        timePressStarted = Time.time;
        isPointerDown = true;
        longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
    }
}
