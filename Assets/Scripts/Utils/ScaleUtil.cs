using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUtil : MonoBehaviour
{
    public float endScale = 1;
    public float startScale = 0.9f;
    private float currentScale;
    public Transform target;
    public float speed = 1f;
    private bool scaleTag = false;

    public delegate void CallBack();
    public CallBack m_callBack = null;

    // Use this for initialization
    void Start()
    {
        AudioScript.getAudioScript().playSound_LayerShow();

        target.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        target.GetComponent<RectTransform>().DOScale(1f, 0.2f).OnComplete<Tween>(delegate ()
        {
            if (m_callBack != null)
            {
                m_callBack();
            }
        });
    }

    public void OnClickClose()
    {
        Destroy(gameObject);

        AudioScript.getAudioScript().playSound_LayerClose();
    }
}