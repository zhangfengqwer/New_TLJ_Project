using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activity_huafeisuipian_Script : MonoBehaviour {

    public GameObject m_listView;
    public ListViewScript m_ListViewScript;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Activity/Activity_huafeisuipian") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_huafeisuipian_Script_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_huafeisuipian_Script_hotfix", "Start", null, null);
            return;
        }

        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        NetLoading.getInstance().Show();

        LogicEnginerScript.Instance.GetComponent<HuaFeiSuiPianDuiHuanDataRequest>().CallBack = onReceive_HuaFeiSuiPianDuiHuanData;
        LogicEnginerScript.Instance.GetComponent<HuaFeiSuiPianDuiHuanDataRequest>().OnRequest();
    }

    public void onReceive_HuaFeiSuiPianDuiHuanData(string json)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_huafeisuipian_Script_hotfix", "onReceive_HuaFeiSuiPianDuiHuanData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_huafeisuipian_Script_hotfix", "onReceive_HuaFeiSuiPianDuiHuanData", null, json);
            return;
        }

        NetLoading.getInstance().Close();

        HuaFeiSuiPianDuiHuanData.getInstance().initJson(json);

        loadList();
    }

    public void loadList()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_huafeisuipian_Script_hotfix", "loadList"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_huafeisuipian_Script_hotfix", "loadList", null, null);
            return;
        }

        m_ListViewScript.clear();

        for (int i = 0; i < HuaFeiSuiPianDuiHuanData.getInstance().getDataList().Count; i++)
        {
            HuaFeiSuiPianDuiHuanDataContent temp = HuaFeiSuiPianDuiHuanData.getInstance().getDataList()[i];

            GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_huafeisuipian") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.transform.name = temp.duihuan_id.ToString();

            {
                CommonUtil.setImageSprite(obj.transform.Find("Image_icon_suipian").GetComponent<Image>(), GameUtil.getPropIconPath(temp.material_id));
                obj.transform.Find("Image_icon_suipian/Text").GetComponent<Text>().text = GameUtil.getMyPropNumById(temp.material_id).ToString() + "/" + temp.material_num;
                CommonUtil.setImageSprite(obj.transform.Find("Image_icon_huafei").GetComponent<Image>(), GameUtil.getPropIconPath(temp.Synthesis_id));

                obj.transform.Find("Button_duihuan").GetComponent<Button>().onClick.AddListener(() => onClickDuiHuan(obj));
            }

            m_ListViewScript.addItem(obj);
        }

        m_ListViewScript.addItemEnd();
    }

    public void onClickDuiHuan(GameObject obj)
    {
        int duihuan_id = int.Parse(obj.transform.name);

        NetLoading.getInstance().Show();

        LogicEnginerScript.Instance.GetComponent<HuaFeiSuiPianDuiHuanRequest>().CallBack = onReceive_HuaFeiSuiPianDuiHuan;
        LogicEnginerScript.Instance.GetComponent<HuaFeiSuiPianDuiHuanRequest>().duihuan_id = duihuan_id;
        LogicEnginerScript.Instance.GetComponent<HuaFeiSuiPianDuiHuanRequest>().OnRequest();
    }

    public void onReceive_HuaFeiSuiPianDuiHuan(string json)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_huafeisuipian_Script_hotfix", "onReceive_HuaFeiSuiPianDuiHuan"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_huafeisuipian_Script_hotfix", "onReceive_HuaFeiSuiPianDuiHuan", null, json);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(json);

        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            int duihuan_id = (int)jd["duihuan_id"];
            HuaFeiSuiPianDuiHuanDataContent temp = HuaFeiSuiPianDuiHuanData.getInstance().getDataById(duihuan_id);

            {
                GameUtil.changeData(temp.Synthesis_id, temp.Synthesis_num);
                GameUtil.changeData(temp.material_id, -temp.material_num);

                ShowRewardPanelScript.Show(temp.Synthesis_id.ToString() + ":" + temp.Synthesis_num.ToString(), false);
            }

            refreshMyMaterialNum();
        }
        else
        {
            string msg = (string)jd["msg"];

            ToastScript.createToast(msg);
        }
    }

    public void refreshMyMaterialNum()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_huafeisuipian_Script_hotfix", "refreshMyMaterialNum"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_huafeisuipian_Script_hotfix", "refreshMyMaterialNum", null, null);
            return;
        }

        for (int i = 0; i < HuaFeiSuiPianDuiHuanData.getInstance().getDataList().Count; i++)
        {
            HuaFeiSuiPianDuiHuanDataContent temp = HuaFeiSuiPianDuiHuanData.getInstance().getDataList()[i];

            GameObject obj = m_ListViewScript.getItemList()[i];
            obj.transform.Find("Image_icon_suipian/Text").GetComponent<Text>().text = GameUtil.getMyPropNumById(temp.material_id).ToString() + "/" + temp.material_num;
        }
    }
}
