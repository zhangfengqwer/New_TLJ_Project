using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticePanelScript : MonoBehaviour {
    private UIWarpContent[] uiWarpContent;
    private List<string> _list;
    public GameObject obj1;
    // Use this for initialization
    void Start ()
	{
        Init();
    }

    private void Init()
    {
        _list = new List<string>();
        for (int i = 0; i < 100; i++)
        {
            _list.Add("物品:" + Random.Range(0, 1000));
        }
        uiWarpContent = gameObject.GetComponentsInChildren<UIWarpContent>();
        foreach (var VARIABLE in uiWarpContent)
        {
            VARIABLE.onInitializeItem = onInitializeItem;
            VARIABLE.Init(_list.Count);
        }
        obj1.SetActive(false);
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
//        var find = go.transform.Find("Text");
//        Button button = go.GetComponent<Button>();
//        button.onClick.RemoveAllListeners();
//        button.onClick.AddListener(delegate()
//        {
//            ToastScript.createToast(_list[dataindex]);
//
//            print(_list[dataindex]);
//        });
//        find.GetComponent<Text>().text = _list[dataindex];
    }


   
}
