using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    Easy,
    Hard,
    Hell
}

public enum GameState
{
    Playing,
    Pause,
    GameOver,
    Victory
}


public class Game_Mgr : MonoBehaviour
{
    [Header("------ UI ------")]
    public Button m_Back_Btn;
    public Button m_Config_Btn;
    public Button m_DoubleSpeed_Btn;
    public GameObject m_BackBox;
    public GameObject m_ConfigBox;
    public GameObject m_MessageBox;
    public GameObject m_GameOverBox;
    public Transform m_Canvas;
    public Text m_GoldText;
    public Text m_DeathCountText;
    public Image m_DoubleSpdState;
    public Text m_KillText;

    [HideInInspector] public int m_Gold = 0;  //�������� ���
    [HideInInspector] public int m_KillCount = 0; //ųī��Ʈ
    int m_DeathCount = 30; //����ī��Ʈ
    
    float m_MessBoxTime = 3.0f; //MessageBox ������� �ð� ����

    [HideInInspector] public bool m_DoublespdOnOff = false; //2��� �÷��� ���¸� ���� ����
    [HideInInspector] public float m_GameTime = 0; //���� �÷���Ÿ��
    //2���ֱ�� ���� ü��,�̼� ���׷��̵� ���� �˸� ����
    float m_MonStatUpTimer; 
    float m_MonStatUpTime = 120.0f;
    //2���ֱ�� ���� ü��,�̼� ���׷��̵� ���� �˸� ����

    public static GameMode m_GameMode = GameMode.Easy; //���� ���̵��� ���� ����
    public static GameState m_GameState = GameState.Playing; //���� ���¸� ���� ����

    public static Game_Mgr Inst; //�̱��� ������ ���� �ν��Ͻ�

    void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_GameTime = 0;

        m_MonStatUpTimer = 0.0f;

        m_GameState = GameState.Playing;

        //Button UI ����
        if (m_Back_Btn != null)
            m_Back_Btn.onClick.AddListener(() =>
            {
                if (m_GameState == GameState.Playing)
                {
                    if (m_BackBox != null)
                    {
                        GameObject a_BackBox = Instantiate(m_BackBox) as GameObject;
                        a_BackBox.transform.SetParent(m_Canvas, false);

                        Time.timeScale = 0.0f;

                        m_GameState = GameState.Pause;
                    }
                }
            });

        if (m_Config_Btn != null)
            m_Config_Btn.onClick.AddListener(() =>
            {
                if (m_GameState == GameState.Playing)
                {
                    GameObject a_ConfigBox = Instantiate(m_ConfigBox) as GameObject;
                    a_ConfigBox.transform.SetParent(m_Canvas, false);

                    Time.timeScale = 0.0f;

                    m_GameState = GameState.Pause;
                }
            });

        if (m_DoubleSpeed_Btn != null)
            m_DoubleSpeed_Btn.onClick.AddListener(DoubleSpdBtnClick);

        //���� �� 300���� ����
        m_Gold = 300; //Test
        if (m_GoldText != null)
            m_GoldText.text = "X " + m_Gold;

        //���۽� ����ī��Ʈ30���� ����
        m_DeathCount = 30;
        if (m_DeathCountText != null)
            m_DeathCountText.text = "X " + m_DeathCount;

        Sound_Mgr.Instance.PlayBGM("Action1", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GameState == GameState.GameOver || m_GameState == GameState.Victory)
            return;

        if (m_MessBoxTime > 0.0f)
        {
            m_MessBoxTime -= Time.deltaTime;
            if (m_MessBoxTime <= 0.0f)
                m_MessBoxTime = 0.0f;
        }

        // ���� �÷��� Ÿ��
        m_GameTime += Time.deltaTime;

        //���� ���Ⱦ� �˶�(2���ֱ�)
        m_MonStatUpTimer += Time.deltaTime;
        if (m_MonStatUpTimer > m_MonStatUpTime)
        {
            MonsterStatUpAlarm();

            m_MonStatUpTimer = 0.0f;
        }
        //���� ���Ⱦ� �˶�(2���ֱ�)
    }

    public void AddGold(int a_Value = 10)
    {
        m_Gold += a_Value;

        if(m_Gold > 999999)
        {
            m_GoldText.text = "X " + 999999;
        }
        else
        {
            m_GoldText.text = "X " + m_Gold;
        }
    }

    public void DeathCount(int a_Count = 1)
    {
        if (m_GameState == GameState.GameOver || m_GameState == GameState.Victory)
            return;

        if (m_DeathCount < 0)
            return;

        m_DeathCount -= a_Count;

        //���� ����
        if(m_DeathCount <= 0) 
        {
            m_DeathCount = 0;

            GameOverBox(GameState.GameOver);

            SaveBestScore(Chapter_Mgr.m_MapIdx); //���� ����
        }
        //���� ����

        if (9 < m_DeathCount && m_DeathCount <= 10)
        {
            ShowMessage("���� ����ī��Ʈ�� 10�����Դϴ�.");

            Sound_Mgr.Instance.PlayEffSound("warning", 0.7f);
        }

        m_DeathCountText.text = "X " + m_DeathCount;
    }

    public void AddKillCount(int a_Count = 1)
    {
        if (m_GameState == GameState.GameOver || m_GameState == GameState.Victory)
            return;

        if (m_KillCount < 0)
            return;

        m_KillCount += a_Count;

        m_KillText.text = m_KillCount.ToString() + " Kill";
    }

    public void ShowMessage(string a_Mess, float a_Time = 3.0f)
    {
        if (m_MessBoxTime > 0.0f)
            return;

        GameObject a_MessageBox = Instantiate(m_MessageBox) as GameObject;
        a_MessageBox.transform.SetParent(m_Canvas, false);

        Text a_MessText = a_MessageBox.GetComponentInChildren<Text>();
        if (a_MessText != null)
            a_MessText.text = a_Mess;

        Destroy(a_MessageBox,a_Time);

        m_MessBoxTime = 3.0f;
    }

    void DoubleSpdBtnClick()
    {
        if (m_GameState == GameState.GameOver || m_GameState == GameState.Victory || m_GameState == GameState.Pause)
            return;

        m_DoublespdOnOff = !m_DoublespdOnOff;

        if (m_DoublespdOnOff == true)
        {
            Time.timeScale = 2.0f;
            m_DoubleSpdState.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            m_DoubleSpdState.gameObject.SetActive(false);

        }
    }

    void SaveBestScore(int a_MapIdx)
    {
        if (GlobalValue.g_Unique_ID == "")
            return;

        if (m_GameMode == GameMode.Easy || m_GameMode == GameMode.Hard)
            return;

        int a_BestScore = 0;

        if (a_MapIdx == 1)
            a_BestScore = GlobalValue.g_BestScore_1;
        else if(a_MapIdx == 2)
            a_BestScore = GlobalValue.g_BestScore_2;
        else if (a_MapIdx == 3)
            a_BestScore = GlobalValue.g_BestScore_3;
        else if (a_MapIdx == 4)
            a_BestScore = GlobalValue.g_BestScore_4;

        if (m_KillCount > a_BestScore) //���� �����ھ BestScore���� ������
        {
            //PlayFab�� �� �ְ� ���� ����
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate
                        {
                            StatisticName = "BestScore_" + a_MapIdx.ToString(),
                            Value = m_KillCount
                        },
                    }
            };
            //PlayFab�� �� �ְ� ���� ����

            PlayFabClientAPI.UpdatePlayerStatistics(
                   request,

                   (result) =>
                   { //������Ʈ ������ ���� �Լ�
                           //Debug.Log("���� ���� ���� : " + m_KillCount + "Kill");
                   },

                   (error) =>
                   { //������Ʈ ���н� ���� �Լ�
                           Debug.Log(error.ErrorMessage);
                   }
             );

        }
    }

    void MonsterStatUpAlarm()
    {

        ShowMessage("Rogue���� ü�°� �ӵ��� ����մϴ�.");

        Sound_Mgr.Instance.PlayEffSound("warning", 0.7f);

    }

    public void GameOverBox(GameState a_GameState)
    {
        m_GameState = a_GameState;

        GameObject a_GameOverBox = Instantiate(m_GameOverBox) as GameObject;
        a_GameOverBox.transform.SetParent(m_Canvas.transform, false);
        a_GameOverBox.transform.localPosition = new Vector3(0.0f, 650.0f, 0.0f);

        Sound_Mgr.Instance.PlayEffSound("GameOver", 0.7f);
    }
}
