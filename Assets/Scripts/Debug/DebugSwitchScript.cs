using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSwitchScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (!OtherData.s_isTest)
        {
            //GameUtil.hideGameObject(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
