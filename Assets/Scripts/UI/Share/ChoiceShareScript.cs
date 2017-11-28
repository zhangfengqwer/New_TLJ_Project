using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceShareScript : MonoBehaviour {

    public Button ShareFriends;
    public Button ShareFriendsCirle;

    public static GameObject Create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ChoiceSharePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        return obj;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickShareFriends(string content)
    {
        PlatformHelper.WXShareFriends("AndroidCallBack", "OnWxShareFriends", content);
    }

    public void OnClickShareFriendsCircle(byte[] data)
    {
        PlatformHelper.WXShareFriendsCircle("AndroidCallBack", "OnWxShareFriends", data);
    }

    public void OnClickShareClose()
    {
        Destroy(this.gameObject);
    }
}
