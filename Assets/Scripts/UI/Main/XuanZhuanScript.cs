using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XuanZhuanScript : MonoBehaviour {

    Animation m_animation;

	// Use this for initialization
	void Start ()
    {
        m_animation = gameObject.GetComponent<Animation>();
        InvokeRepeating("onInvoke", 0.1f, 5);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void onInvoke()
    {
        if (RandomUtil.getRandom(1, 2) == 1)
        {
            m_animation.Play("mainLayerBtnXuanZhuan");
        }
    }
}
