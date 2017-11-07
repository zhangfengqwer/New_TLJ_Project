using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShiFangPokerScript : MonoBehaviour, IPointerDownHandler
{
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
        {
            if (GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getIsSelect())
            {
                GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
            }
        }
    }
}
