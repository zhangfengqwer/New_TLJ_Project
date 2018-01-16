using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDiPokerScript : MonoBehaviour {

    public static GameObject ShowDiPokerObj = null;

    public static GameObject create(List<TLJCommon.PokerInfo> dipaiList)
    {
        if (ShowDiPokerObj == null)
        {
            GameObject prefab = Resources.Load("Prefabs/Game/ShowDiPoker") as GameObject;
            ShowDiPokerObj = MonoBehaviour.Instantiate(prefab, GameObject.Find("Canvas").transform);

            ShowDiPokerObj.GetComponent<ShowDiPokerScript>().setData(dipaiList);

            return ShowDiPokerObj;
        }

        return null;
    }

    // Use this for initialization
    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShowDiPokerScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShowDiPokerScript", "Start", null, null);
            return;
        }

        Invoke("showEnd", 2.0f);
    }

    public void showEnd()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShowDiPokerScript", "showEnd"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShowDiPokerScript", "showEnd", null, null);
            return;
        }

        //for (int i = gameObject.transform.childCount - 1; i >= 0 ; i--)
        //{
        //    Destroy(gameObject.transform.GetChild(i));
        //}

        Destroy(gameObject);
        ShowDiPokerObj = null;
    }

    private void OnDestroy()
    {
        ShowDiPokerObj = null;
    }

    public void setData(List<TLJCommon.PokerInfo> dipaiList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShowDiPokerScript", "setData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShowDiPokerScript", "setData", null, dipaiList);
            return;
        }

        List<GameObject> objList = new List<GameObject>();

        for (int i = 0; i < dipaiList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(gameObject.transform);
            poker.transform.localScale = new Vector3(1, 1, 1);

            poker.GetComponent<PokerScript>().initPoker(dipaiList[i].m_num, (int)dipaiList[i].m_pokerType);
            poker.GetComponent<PokerScript>().m_canTouch = false;

            objList.Add(poker);
        }

        initPokerPos(objList);
    }

    public void initPokerPos(List<GameObject> objList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShowDiPokerScript", "initPokerPos"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShowDiPokerScript", "initPokerPos", null, objList);
            return;
        }

        int jiange = 35;

        for (int i = 0; i < objList.Count; i++)
        {
            int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
            objList[i].transform.localPosition = new Vector3(x, 0,0);
            objList[i].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            // 设置最后渲染
            objList[i].transform.SetAsLastSibling();
        }
    }
}
