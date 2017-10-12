using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelScript : MonoBehaviour {
    private UIWarpContent uiWarpContent;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BagPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);
        return obj;
    }
    // Use this for initialization
    void Start ()
	{
	    uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
	    uiWarpContent.onInitializeItem = onInitializeItem;

        uiWarpContent.Init(UserBagData.getInstance().getUserBagDataList().Count);
    }

    void onInitializeItem(GameObject go, int dataindex)
    {
        var find = go.transform.Find("Text");
        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
            // 显示道具详情
            PropDetailPanelScript.create(UserBagData.getInstance().getUserBagDataList()[dataindex].prop_id, this);
        });

        find.GetComponent<Text>().text = UserBagData.getInstance().getUserBagDataList()[dataindex].prop_id + "x"+ UserBagData.getInstance().getUserBagDataList()[dataindex].prop_num;
    }

    public void useProp(int prop_id)
    {
        UserBagData.getInstance().useProp(prop_id,1);

        // 刷新list
    }
}
