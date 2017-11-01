using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign_Guang_Script : MonoBehaviour {

    public Image m_image1;
    public Image m_image2;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_image1.transform.Rotate(new Vector3(0, 0, 0.4f));
        m_image2.transform.Rotate(new Vector3(0, 0, -0.4f));
    }
}
