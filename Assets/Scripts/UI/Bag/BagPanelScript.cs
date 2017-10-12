using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelScript : MonoBehaviour {
    private UIWarpContent uiWarpContent;
    public List<UserPropData> PropList;

    public static BagPanelScript Instance;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BagPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);
        return obj;
    }
    // Use this for initialization
    void Start ()
	{
	    if (Instance == null)
	    {
	        Instance = this;
	    }

	    PropList = GetUserBagRequest.Instance.GetPropList();
	    uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
	    uiWarpContent.onInitializeItem = onInitializeItem;
	    uiWarpContent.Init(PropList.Count);
    }

    public void deleteItem(int dataindex)
    {
        uiWarpContent.DelItem(dataindex);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < PropList.Count; i++)
        {
            deleteItem(i);
        }
        uiWarpContent.Init(PropList.Count);
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
        var find = go.transform.Find("Text");
        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
            // 显示道具详情
            PropDetailPanelScript.create(PropList[dataindex].prop_id, this);
        });

        find.GetComponent<Text>().text = PropList[dataindex].prop_id + "x"+ PropList[dataindex].prop_num;
    }
}
