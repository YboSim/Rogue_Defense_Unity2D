using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_Mgr : MonoBehaviour
{
    public Button m_StartBtn;       //게임시작 버튼
    public Button m_HelpBtn;        //도움말 버튼
    public Button m_ConfigBtn;      //세팅설정 버튼
    public Button m_ExitBtn;        //게임종료 버튼

    public GameObject m_HelpBox;
    public GameObject m_ConfigBox;
    public GameObject m_LogInBox;
    public Transform m_Canvas;

    

    // Start is called before the first frame update
    void Start()
    {
        if (m_StartBtn != null)
            m_StartBtn.onClick.AddListener(() =>
            {
                GameObject a_LogInBox = Instantiate(m_LogInBox) as GameObject;
                a_LogInBox.transform.SetParent(m_Canvas, false);
                a_LogInBox.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_HelpBtn != null)
            m_HelpBtn.onClick.AddListener(() =>
            {
                GameObject a_HelpBox = Instantiate(m_HelpBox) as GameObject;
                a_HelpBox.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
                a_HelpBox.transform.SetParent(m_Canvas, false);

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_ConfigBtn != null)
            m_ConfigBtn.onClick.AddListener(() =>
            {
                GameObject a_ConfigBox = Instantiate(m_ConfigBox) as GameObject;
                a_ConfigBox.transform.SetParent(m_Canvas,false);

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(() =>
            {
                Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);

                Application.Quit();
            });

        Sound_Mgr.Instance.PlayBGM("Action4", 1.0f);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
