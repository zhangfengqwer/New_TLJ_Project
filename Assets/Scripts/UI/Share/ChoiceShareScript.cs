using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceShareScript : MonoBehaviour {

    public Button ShareFriends;
    public Button ShareFriendsCirle;

    public static GameObject Create(string content,string data)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ChoiceSharePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

        obj.GetComponent<ChoiceShareScript>().SetClickListener(content,data);
        return obj;
    }

    public void OnClickShareClose()
    {
        Destroy(this.gameObject);
    }

    public void SetClickListener(string content, string data)
    {
        ShareFriends.onClick.AddListener(() =>
        {
            PlatformHelper.WXShareFriends("AndroidCallBack", "OnWxShareFriends", content);
            Destroy(this.gameObject);
        });

        ShareFriendsCirle.onClick.AddListener(() =>
        {
            PlatformHelper.WXShareFriendsCircle("AndroidCallBack", "OnWxShareFriends", new byte[0]);
            Destroy(this.gameObject);
        });
    }
}
