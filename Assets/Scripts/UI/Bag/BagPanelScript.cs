using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelScript : MonoBehaviour {
    private UIWarpContent uiWarpContent;
    private List<UserPropData> PropList;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BagPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);
        return obj;
    }
    // Use this for initialization
    void Start ()
	{
	    PropList = GetUserBagRequest.GetPropList();
	    uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
	    uiWarpContent.onInitializeItem = onInitializeItem;
	    uiWarpContent.Init(PropList.Count);
	   
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
        var find = go.transform.Find("Text");
        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
//            ToastScript.createToast(PropList[dataindex].prop_id+"");
            print(PropList[dataindex]);

            // 显示道具详情
            PropDetailPanelScript.create(PropList[dataindex].prop_id, this);
        });

        find.GetComponent<Text>().text = PropList[dataindex].prop_id+"";
    }


   
}
