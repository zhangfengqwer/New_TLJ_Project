using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelScript : MonoBehaviour {
    private UIWarpContent uiWarpContent;
    private List<string> _list;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BagPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
	{
	    _list = new List<string>();
	    for (int i = 0; i < 100; i++)
	    {
	        _list.Add("物品:" +  Random.Range(0,1000));
	    }
	    uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
	    uiWarpContent.onInitializeItem = onInitializeItem;
	    uiWarpContent.Init(_list.Count);
	   
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
        var find = go.transform.Find("Text");
        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
            ToastScript.createToast(_list[dataindex]);
            print(_list[dataindex]);

            // 显示道具详情
            PropDetailPanelScript.create(101,this);
        });

        find.GetComponent<Text>().text = _list[dataindex];
    }


   
}
