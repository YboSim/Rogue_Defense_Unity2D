using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackBox : MonoBehaviour
{
    public Button m_OkBtn;
    public Button m_CancleBtn;

    // Start is called before the first frame update
    void Start()
    {
        if (m_OkBtn != null)
            m_OkBtn.onClick.AddListener(() =>
            {
                Time.timeScale = 1.0f;

                if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                {
                    Fade_Mgr.Inst.SceneOutReserve("ChapterScene");
                }
                else
                    SceneManager.LoadScene("ChapterScene");

                Chapter_Mgr.m_MapIdx = 0; //인덱스 초기화

                Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);
            });

        if (m_CancleBtn != null)
            m_CancleBtn.onClick.AddListener(() =>
            {
                if (Game_Mgr.Inst.m_DoublespdOnOff == true)
                    Time.timeScale = 2.0f;
                else
                    Time.timeScale = 1.0f;

                Game_Mgr.m_GameState = GameState.Playing;

                Destroy(gameObject);
            });
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
