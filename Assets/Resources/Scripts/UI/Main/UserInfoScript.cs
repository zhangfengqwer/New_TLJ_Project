using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoScript : MonoBehaviour {
    public Text nickName;
    public Text account;
    public Text coinCount;
    public Text yuanBaoCount;
    public Text shengLv;
    public Image userImage;

    private void Start()
    {
        ScaleParticleSystem(this.gameObject, 0.8f);
        nickName.text = "zfffff";
        account.text = "zhangfengqer";
        coinCount.text = 5000.ToString();
        yuanBaoCount.text = 20.ToString();
        shengLv.text = "23.21%";
        userImage.sprite = Resources.Load<Sprite>("Sprites/Game/Poker/icon_xiaowang");
    }

    private void ScaleObject(float begin)
    {
        Vector3 vector3 = new Vector3() {x = begin, y = begin, z = begin};

        transform.localScale = vector3;


    }

    /// <summary>
    /// 缩放粒子
    /// </summary>
    /// <param name="gameObj">粒子节点</param>
    /// <param name="scale">绽放系数</param>
    public static void ScaleParticleSystem(GameObject gameObj, float scale)
    {
        var hasParticleObj = false;
        var particles = gameObj.GetComponentsInChildren<ParticleSystem>(true);
        var max = particles.Length;
        for (int idx = 0; idx < max; idx++)
        {
            var particle = particles[idx];
            if (particle == null) continue;
            hasParticleObj = true;
            particle.startSize *= scale;
            particle.startSpeed *= scale;
            particle.startRotation *= scale;
            particle.transform.localScale *= scale;
        }
        if (hasParticleObj)
        {
            gameObj.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void OnCloseClick()
    {
        Destroy(this.gameObject);
    }

    public void OnBindPhoneClick()
    {
        print("绑定手机");
    }

    public void OnRealNameClick()
    {
        print("实名认证");
    }
}
