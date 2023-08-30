using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue_3_Ctrl : MonoBehaviour
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
        #region//--- Hell
        if (Game_Mgr.m_GameMode == GameMode.Hell)
        {
            if (Game_Mgr.Inst.m_GameTime <= 120.0f)
            {
                m_MaxHp = 120;
                m_CurHp = 120;

                m_MvSpeed = 3;
            }
            else if (120.0f < Game_Mgr.Inst.m_GameTime &&
                    Game_Mgr.Inst.m_GameTime <= 240.0f)
            {
                m_MaxHp = 210;
                m_CurHp = 210;

                m_MvSpeed = 3.5f;
            }
            else if (240.0f < Game_Mgr.Inst.m_GameTime &&
            Game_Mgr.Inst.m_GameTime <= 360.0f)
            {
                m_MaxHp = 290;
                m_CurHp = 290;

                m_MvSpeed = 4;
            }
            else if (360.0f < Game_Mgr.Inst.m_GameTime &&
                     Game_Mgr.Inst.m_GameTime <= 480.0f)
            {
                m_MaxHp = 380;
                m_CurHp = 380;

                m_MvSpeed = 4.5f;
            }
            else if (480.0f < Game_Mgr.Inst.m_GameTime &&
             Game_Mgr.Inst.m_GameTime <= 600.0f)
            {
                m_MaxHp = 460;
                m_CurHp = 460;

                m_MvSpeed = 5;
            }
            else if (600.0f < Game_Mgr.Inst.m_GameTime &&
                              Game_Mgr.Inst.m_GameTime <= 720.0f)
            {
                m_MaxHp = 540;
                m_CurHp = 540;
                m_MvSpeed = 6f;
            }
            else if (720.0f < Game_Mgr.Inst.m_GameTime &&
                              Game_Mgr.Inst.m_GameTime <= 840.0f)
            {
                m_MaxHp = 700;
                m_CurHp = 700;
                m_MvSpeed = 6f;
            }
            else if (840.0f < Game_Mgr.Inst.m_GameTime &&
                  Game_Mgr.Inst.m_GameTime <= 960.0f)
            {
                m_MaxHp = 850;
                m_CurHp = 850;
                m_MvSpeed = 6f;
            }
            else if (960.0f < Game_Mgr.Inst.m_GameTime &&
                              Game_Mgr.Inst.m_GameTime <= 1080.0f)
            {
                m_MaxHp = 1000;
                m_CurHp = 1000;
                m_MvSpeed = 6f;
            }
            else if (1080.0f < Game_Mgr.Inst.m_GameTime)
            {
                m_MaxHp = 1200;
                m_CurHp = 1200;
                m_MvSpeed = 6f;
            }
        }
#endregion

        #region//--- Hard
        else if (Game_Mgr.m_GameMode == GameMode.Hard)
        {
            if (Game_Mgr.Inst.m_GameTime <= 120.0f)
            {
                m_MaxHp = 120;
                m_CurHp = 120;

                m_MvSpeed = 3;
            }
            else if (120.0f < Game_Mgr.Inst.m_GameTime &&
                    Game_Mgr.Inst.m_GameTime <= 240.0f)
            {
                m_MaxHp = 210;
                m_CurHp = 210;

                m_MvSpeed = 3.5f;
            }
            else if (240.0f < Game_Mgr.Inst.m_GameTime &&
            Game_Mgr.Inst.m_GameTime <= 360.0f)
            {
                m_MaxHp = 290;
                m_CurHp = 290;

                m_MvSpeed = 4;
            }
            else if (360.0f < Game_Mgr.Inst.m_GameTime &&
                     Game_Mgr.Inst.m_GameTime <= 480.0f)
            {
                m_MaxHp = 380;
                m_CurHp = 380;

                m_MvSpeed = 4.5f;
            }
            else if (480.0f < Game_Mgr.Inst.m_GameTime &&
             Game_Mgr.Inst.m_GameTime <= 600.0f)
            {
                m_MaxHp = 460;
                m_CurHp = 460;

                m_MvSpeed = 5;
            }
            else if (600.0f < Game_Mgr.Inst.m_GameTime)
            {
                m_MaxHp = 540;
                m_CurHp = 540;

                m_MvSpeed = 6;
            }
        }
#endregion

        #region//--- Easy
        else if (Game_Mgr.m_GameMode == GameMode.Easy)
        {
            if (Game_Mgr.Inst.m_GameTime <= 120.0f)
            {
                m_MaxHp = 80;
                m_CurHp = 80;

                m_MvSpeed = 3;
            }
            else if (120.0f < Game_Mgr.Inst.m_GameTime &&
                    Game_Mgr.Inst.m_GameTime <= 240.0f)
            {
                m_MaxHp = 140;
                m_CurHp = 140;

                m_MvSpeed = 3.5f;
            }
            else if (240.0f < Game_Mgr.Inst.m_GameTime &&
            Game_Mgr.Inst.m_GameTime <= 360.0f)
            {
                m_MaxHp = 210;
                m_CurHp = 210;

                m_MvSpeed = 4;
            }
            else if (360.0f < Game_Mgr.Inst.m_GameTime &&
                     Game_Mgr.Inst.m_GameTime <= 480.0f)
            {
                m_MaxHp = 290;
                m_CurHp = 290;

                m_MvSpeed = 4.5f;
            }
            else if (480.0f < Game_Mgr.Inst.m_GameTime &&
             Game_Mgr.Inst.m_GameTime <= 600.0f)
            {
                m_MaxHp = 360;
                m_CurHp = 360;

                m_MvSpeed = 5;
            }
            else if (600.0f < Game_Mgr.Inst.m_GameTime)
            {
                m_MaxHp = 440;
                m_CurHp = 440;

                m_MvSpeed = 6;
            }
        }

#endregion
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
