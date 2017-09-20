using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailPanelScript : MonoBehaviour {
    private UIWarpContent uiWarpContent;
    private List<string> list = new List<string>();
    void Start () {
        for (int i = 0; i < 30; i++)
        {
            list.Add(i + "");
        }
        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
	    uiWarpContent.onInitializeItem = onInitializeItem;
	    uiWarpContent.Init(list.Count);
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
        var EmailName = go.transform.Find("EmailName");
        Text text = EmailName.GetComponent<Text>();
        text.text = list[dataindex];
    }
}
