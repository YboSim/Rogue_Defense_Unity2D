using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnPressCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool m_IsButtonDowning = false;
    Image m_BtnImg = null;

    public Sprite m_DownImg = null;
    public Sprite m_UpImg = null;

    // Start is called before the first frame update
    void Start()
    {
        m_BtnImg = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsButtonDowning == true)
        {
            m_BtnImg.sprite = m_DownImg;
        }
        else
        {
            m_BtnImg.sprite = m_UpImg;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_IsButtonDowning = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_IsButtonDowning = false;
    }
}
