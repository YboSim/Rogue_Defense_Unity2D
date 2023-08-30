using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chapter_Mgr : MonoBehaviour
{
    public Button m_BurialGroundBtn;
    public Button m_ArcticBtn;
    public Button m_ForestBtn;
    public Button m_DesertBtn;
    public Button m_BackBtn;

    public GameObject m_ModeBox;
    public Transform m_Canvas;

    //ModeBox를 켰을때 맵선택과 뒤로가기 버튼을 비활성화 하기위한 변수
    public static bool m_ModeBoxOn = false;

    //선택한 맵을 저장하기 위한 변수(씬 이동 및 점수저장을 위한 변수)
    public static int m_MapIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_ModeBoxOn = false;
        m_MapIdx = 0;

        if (m_BackBtn != null)
            m_BackBtn.onClick.AddListener(() =>
            {
                if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                    Fade_Mgr.Inst.SceneOutReserve("TitleScene");
                else
                    SceneManager.LoadScene("TitleScene");

                m_MapIdx = 0; //인덱스 초기화

                Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);
            });

        if (m_BurialGroundBtn != null)
            m_BurialGroundBtn.onClick.AddListener(() =>
            {
                GameObject a_ModeBox = Instantiate(m_ModeBox) as GameObject;
                a_ModeBox.transform.SetParent(m_Canvas, false);
                a_ModeBox.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

                m_ModeBoxOn = true;

                m_MapIdx = 1;

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_ArcticBtn != null)
            m_ArcticBtn.onClick.AddListener(() =>
            {
                GameObject a_ModeBox = Instantiate(m_ModeBox) as GameObject;
                a_ModeBox.transform.SetParent(m_Canvas, false);
                a_ModeBox.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

                m_ModeBoxOn = true;

                m_MapIdx = 2;

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_ForestBtn != null)
            m_ForestBtn.onClick.AddListener(() =>
            {
                GameObject a_ModeBox = Instantiate(m_ModeBox) as GameObject;
                a_ModeBox.transform.SetParent(m_Canvas, false);
                a_ModeBox.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

                m_ModeBoxOn = true;

                m_MapIdx = 3;

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_DesertBtn != null)
            m_DesertBtn.onClick.AddListener(() =>
            {
                GameObject a_ModeBox = Instantiate(m_ModeBox) as GameObject;
                a_ModeBox.transform.SetParent(m_Canvas, false);
                a_ModeBox.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

                m_ModeBoxOn = true;

                m_MapIdx = 4;

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        Sound_Mgr.Instance.PlayBGM("Action3", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_ModeBoxOn == true)
        {
            m_BurialGroundBtn.GetComponent<Button>().interactable = false;
            m_ArcticBtn.GetComponent<Button>().interactable = false;
            m_ForestBtn.GetComponent<Button>().interactable = false;
            m_DesertBtn.GetComponent<Button>().interactable = false;
            m_BackBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            m_BurialGroundBtn.GetComponent<Button>().interactable = true;
            m_ArcticBtn.GetComponent<Button>().interactable = true;
            m_ForestBtn.GetComponent<Button>().interactable = true;
            m_DesertBtn.GetComponent<Button>().interactable = true;
            m_BackBtn.GetComponent<Button>().interactable = true;
        }
    }
}
