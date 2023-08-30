using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue_MB_Ctrl : MonoBehaviour
{
    public int m_MaxHp; //최대 Hp
    public int m_CurHp; //현재 Hp

    public float m_MvSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        HpSetting();
    }

    void HpSetting()
    {
        if (Game_Mgr.Inst.m_GameTime <= 125.0f)
        {
            m_MaxHp = 1000;
            m_CurHp = 1000;

            m_MvSpeed = 2.5f;
        }
        else if (125.0f < Game_Mgr.Inst.m_GameTime &&
                Game_Mgr.Inst.m_GameTime <= 245.0f)
        {
            m_MaxHp = 1200;
            m_CurHp = 1200;

            m_MvSpeed = 3f;
        }
        else if (245.0f < Game_Mgr.Inst.m_GameTime &&
        Game_Mgr.Inst.m_GameTime <= 365.0f)
        {
            m_MaxHp = 1500;
            m_CurHp = 1500;

            m_MvSpeed = 3.5f;
        }
        else if (365.0f < Game_Mgr.Inst.m_GameTime &&
                 Game_Mgr.Inst.m_GameTime <= 485.0f)
        {
            m_MaxHp = 1800;
            m_CurHp = 1800;

            m_MvSpeed = 4f;
        }
        else if (485.0f < Game_Mgr.Inst.m_GameTime &&
         Game_Mgr.Inst.m_GameTime <= 605.0f)
        {
            m_MaxHp = 2200;
            m_CurHp = 2200;

            m_MvSpeed = 4.5f;
        }
        else if (605.0f < Game_Mgr.Inst.m_GameTime)
        {
            m_MaxHp = 2500;
            m_CurHp = 2500;

            m_MvSpeed = 5f;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
