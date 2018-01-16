using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DragUtil : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform _rectTransform;
    public Vector3 offset = new Vector3((float) 3.7, (float) 2.3, 0);

    private void Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }

    // begin dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
        //得到刚开始close所在的坐标
//        Vector3 globalMousePos;
//        RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position,
//            eventData.pressEventCamera, out globalMousePos);
//        print(globalMousePos);
    }

    // during dragging
    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    // end dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
        offset = new Vector3((float) 3.5, (float) 2.3, 0);
    }

    /// <summary>
    /// set position of the dragged game object
    /// </summary>
    /// <param name="eventData"></param>
    public void SetDraggedPosition(PointerEventData eventData)
    {
        // transform the screen point to world point int rectangle
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position,
            eventData.pressEventCamera, out globalMousePos))
        {
            _rectTransform.position = globalMousePos - offset;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position,
            eventData.pressEventCamera, out globalMousePos);
        offset = globalMousePos - _rectTransform.position;
    }
}