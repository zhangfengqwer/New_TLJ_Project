using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text text;
    public GameObject obj; 

    public void MyStart()
    {
        StartCoroutine("StartTimer", 10);
    }



    IEnumerator StartTimer(int time)
    {
        while (time >= 0)
        {

            text.text = string.Format("时间：{0}", time);
            time--;
            yield return new WaitForSeconds(1);
        }
    }
}