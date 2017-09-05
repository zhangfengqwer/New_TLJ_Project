using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickLogin()
    {
        GameObject.Find("Canvas").GetComponent<LoginScript>().reqLogin();
    }

    public void onClickQuickRegister()
    {
        GameObject.Find("Canvas").GetComponent<LoginScript>().reqQuickRegister();
    }
}
