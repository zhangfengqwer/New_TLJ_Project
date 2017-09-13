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

    // Use this for initialization
    void Start()
    {
        target.transform.localScale = new Vector3(startScale,startScale,1);
        currentScale = startScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (scaleTag)
        {
            if (currentScale > startScale)
            {
                currentScale -= Time.deltaTime * speed;
                target.transform.localScale = new Vector3(currentScale, currentScale, 1);
            }
            else
            {
                scaleTag = false;
                Destroy(this.gameObject);

            }

        }
        else
        {
            if (currentScale <= endScale)
            {
                currentScale += Time.deltaTime * speed;
                target.transform.localScale = new Vector3(currentScale, currentScale, 1);
            }
        }
    }

    public void OnClickClose()
    {
        scaleTag = true;

    }
}