using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadIconScript : MonoBehaviour {

    public Image m_icon;

	// Use this for initialization
	void Start ()
    {
		
	}

    public void setIcon(string path)
    {
        m_icon.sprite = Resources.Load("Sprites/Head/" + path, typeof(Sprite)) as Sprite;
    }
}
