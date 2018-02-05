using LitJson;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class OldPlayerBindPanelScript : MonoBehaviour
{
    public InputField m_inputField_uid;
    public OldPlayerBindRequest m_oldPlayerBindRequest;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/OldPlayerBindPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_oldPlayerBindPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OldPlayerBindPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OldPlayerBindPanelScript_hotfix", "Start", null, null);
            return;
        }

        m_oldPlayerBindRequest = LogicEnginerScript.Instance.GetComponent<OldPlayerBindRequest>();
        m_oldPlayerBindRequest.m_callBack = onReceive_OldPlayerBind;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClickOK()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OldPlayerBindPanelScript_hotfix", "onClickOK"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OldPlayerBindPanelScript_hotfix", "onClickOK", null, null);
            return;
        }

        if (m_inputField_uid.text.CompareTo("") == 0)
        {
            ToastScript.createToast("请输入UID");

            return;
        }

        NetLoading.getInstance().Show();
        
        m_oldPlayerBindRequest.m_oldUid = m_inputField_uid.text;
        m_oldPlayerBindRequest.OnRequest();
    }

    public void onReceive_OldPlayerBind(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OldPlayerBindPanelScript_hotfix", "onReceive_OldPlayerBind"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OldPlayerBindPanelScript_hotfix", "onReceive_OldPlayerBind", null, data);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int)jsonData["code"];
        if (code == (int)Consts.Code.Code_OK)
        {
            ToastScript.createToast("绑定成功，请去邮箱领取奖励");
            // 小红点点亮
        }
        else if (code == (int)Consts.Code.Code_OldPlayerUidIsNotExist)
        {
            ToastScript.createToast("您输入的UID不存在");
        }
        else if (code == (int)Consts.Code.Code_TheOldUidIsUsed)
        {
            ToastScript.createToast("您输入的UID已经被绑定过，不可重复使用");
        }
        else if (code == (int)Consts.Code.Code_TheUidIsBind)
        {
            ToastScript.createToast("您已经绑定过，不可重复绑定");
        }
        else
        {
            ToastScript.createToast("绑定失败：" + code.ToString ());
        }

        NetLoading.getInstance().Close();
    }
}
