using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUtil : MonoBehaviour
{
    public float endScale = 1;
    public float startScale = 0.9f;
    public float currentScale;
    public Transform target;
    public float speed = 1f;
    public bool scaleTag = false;

    public delegate void CallBack();
    public CallBack m_callBack = null;

    public bool m_canClose = true;

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
        if (m_canClose)
        {
            Destroy(gameObject);

            AudioScript.getAudioScript().playSound_LayerClose();
        }
    }

    public void setCanClose(bool canClose)
    {
        m_canClose = canClose;
    }
}