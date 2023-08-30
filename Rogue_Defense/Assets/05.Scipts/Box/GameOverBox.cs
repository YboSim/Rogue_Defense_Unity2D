using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverBox : MonoBehaviour
{
    public Text m_GameOverText;
    public Text m_PlayTimeText;
    public Text m_MapText;
    public Text m_GameModeText;
    public Text m_ScoreText;
    public Text m_TotalGoldText;
    public Button m_LobbyBtn;
    public Button m_SelectMapBtn;

    float m_MvDownSpeed = 10.0f;
    int m_GameTime;
    float m_CacTime;
    int m_Minute;
    int m_Second;

    int m_TotalGold;

    // Start is called before the first frame update
    void Start()
    {
        if (m_LobbyBtn != null)
            m_LobbyBtn.onClick.AddListener(() =>
            {
                if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                    Fade_Mgr.Inst.SceneOutReserve("TitleScene");
                else
                    SceneManager.LoadScene("TitleScene");

                Chapter_Mgr.m_MapIdx = 0; //인덱스 초기화

                Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);
            });

        if (m_SelectMapBtn != null)
            m_SelectMapBtn.onClick.AddListener(() =>
            {
                if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                    Fade_Mgr.Inst.SceneOutReserve("ChapterScene");
                else
                    SceneManager.LoadScene("ChapterScene");

                Chapter_Mgr.m_MapIdx = 0; //인덱스 초기화

                Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);
            });

        m_GameTime = (int)Game_Mgr.Inst.m_GameTime;
        m_CacTime = m_GameTime / 60;
        m_Minute = (int)m_CacTime;
        m_Second = m_GameTime % 60;

        if (Game_Mgr.m_GameState == GameState.GameOver)
        {
            m_GameOverText.text = "Game Over";
        }
        else if(Game_Mgr.m_GameState == GameState.Victory)
        {
            m_GameOverText.text = "Victory";
        }

        m_PlayTimeText.text = "PlayTime - " + m_Minute + " : " + m_Second;

        if (Chapter_Mgr.m_MapIdx == 1)
            m_MapText.text = "Map - Burial Ground";
        else if (Chapter_Mgr.m_MapIdx == 2)
            m_MapText.text = "Map - Arctic";
        else if (Chapter_Mgr.m_MapIdx == 3)
            m_MapText.text = "Map - Forest";
        else if (Chapter_Mgr.m_MapIdx == 4)
            m_MapText.text = "Map - Desert";

        m_GameModeText.text = "Mode - " + Game_Mgr.m_GameMode.ToString();

        m_ScoreText.text = "Score - " + Game_Mgr.Inst.m_KillCount.ToString() + "Kill";


        m_TotalGold = Game_Mgr.Inst.m_Gold;
        Tower_Arrow a_ArTower = FindObjectOfType<Tower_Arrow>();
        if (a_ArTower != null)
            m_TotalGold += a_ArTower.m_TowerValue;
        Tower_Bomb a_BbTower = FindObjectOfType<Tower_Bomb>();
        if (a_BbTower != null)
            m_TotalGold += a_BbTower.m_TowerValue;
        Tower_Ice a_IceTower = FindObjectOfType<Tower_Ice>();
        if (a_IceTower != null)
            m_TotalGold += a_IceTower.m_TowerValue;
        Tower_Lava a_LvTower = FindObjectOfType<Tower_Lava>();
        if (a_LvTower != null)
            m_TotalGold += a_LvTower.m_TowerValue;
        Tower_Poison a_PsTower = FindObjectOfType<Tower_Poison>();
        if (a_PsTower != null)
            m_TotalGold += a_PsTower.m_TowerValue;

        m_TotalGoldText.text = "TotalGold - " + m_TotalGold + "Gold";
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y > 0.0f)
        {
            transform.localPosition -= new Vector3(0.0f, m_MvDownSpeed, 0.0f);
        }
    }
}
