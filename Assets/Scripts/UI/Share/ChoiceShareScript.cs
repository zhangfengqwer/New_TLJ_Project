using UnityEngine;
using UnityEngine.UI;

public class ChoiceShareScript : MonoBehaviour
{
    public Button ShareFriends;
    public Button ShareFriendsCirle;

    public static GameObject Create(string content, string data)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ChoiceSharePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

        obj.GetComponent<ChoiceShareScript>().SetClickListener(content, data);
        return obj;
    }

    private void Start()
    {
        OtherData.s_choiceShareScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChoiceShareScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChoiceShareScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void OnClickShareClose()
    {
        Destroy(this.gameObject);
    }

    public void SetClickListener(string content, string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChoiceShareScript_hotfix", "SetClickListener"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChoiceShareScript_hotfix", "SetClickListener", null, content, data);
            return;
        }

        ShareFriends.onClick.AddListener(() =>
        {
            PlatformHelper.WXShareFriends("AndroidCallBack", "OnWxShareFriends", content);
            Destroy(this.gameObject);
        });

        ShareFriendsCirle.onClick.AddListener(() =>
        {

            PlatformHelper.WXShareFriendsCircle("AndroidCallBack", "OnWxShareFriends", data);
            Destroy(this.gameObject);
        });
    }
}