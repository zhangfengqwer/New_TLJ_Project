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

    private bool isPointerDown = false;
    private bool longPressTriggered = false;
    private float timePressStarted;

    private void FixedUpdate()
    {
//        if (isPointerDown && !longPressTriggered)
//        {
//            if (Time.time - timePressStarted > durationThreshold)
//            {
//                longPressTriggered = true;
//                print("changanshijian:"+ (Time.time - timePressStarted));
//            }
//        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("100");

        timePressStarted = Time.time;
        isPointerDown = true;
        longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("200");

        isPointerDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("200");
        isPointerDown = false;
    }
}
