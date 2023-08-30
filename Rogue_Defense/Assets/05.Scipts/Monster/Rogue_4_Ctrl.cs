using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue_4_Ctrl : MonoBehaviour
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
                m_MaxHp = 70;
                m_CurHp = 70;

                m_MvSpeed = 3.0f;
            }
            else if (120.0f < Game_Mgr.Inst.m_GameTime &&
                    Game_Mgr.Inst.m_GameTime <= 240.0f)
            {
                m_MaxHp = 150;
                m_CurHp = 150;

                m_MvSpeed = 3.5f;
            }
            else if (240.0f < Game_Mgr.Inst.m_GameTime &&
            Game_Mgr.Inst.m_GameTime <= 360.0f)
            {
                m_MaxHp = 210;
                m_CurHp = 210;

                m_MvSpeed = 4.0f;
            }
            else if (360.0f < Game_Mgr.Inst.m_GameTime &&
                     Game_Mgr.Inst.m_GameTime <= 480.0f)
            {
                m_MaxHp = 300;
                m_CurHp = 300;

                m_MvSpeed = 4.5f;
            }
            else if (480.0f < Game_Mgr.Inst.m_GameTime &&
             Game_Mgr.Inst.m_GameTime <= 600.0f)
            {
                m_MaxHp = 400;
                m_CurHp = 400;

                m_MvSpeed = 5.0f;
            }
            else if (600.0f < Game_Mgr.Inst.m_GameTime &&
                              Game_Mgr.Inst.m_GameTime <= 720.0f)
            {
                m_MaxHp = 500;
                m_CurHp = 500;
                m_MvSpeed = 6.0f;
            }
            else if (720.0f < Game_Mgr.Inst.m_GameTime &&
                              Game_Mgr.Inst.m_GameTime <= 840.0f)
            {
                m_MaxHp = 600;
                m_CurHp = 600;
                m_MvSpeed = 6.0f;
            }
            else if (840.0f < Game_Mgr.Inst.m_GameTime &&
                              Game_Mgr.Inst.m_GameTime <= 960.0f)
            {
                m_MaxHp = 700;
                m_CurHp = 700;
                m_MvSpeed = 6.0f;
            }
            else if (960.0f < Game_Mgr.Inst.m_GameTime &&
                              Game_Mgr.Inst.m_GameTime <= 1080.0f)
            {
                m_MaxHp = 800;
                m_CurHp = 800;
                m_MvSpeed = 6.0f;
            }
            else if (1080.0f < Game_Mgr.Inst.m_GameTime)
            {
                m_MaxHp = 1000;
                m_CurHp = 1000;
                m_MvSpeed = 6.0f;
            }
        }
        #endregion

        #region//---Hard
        //하드 모드
        else if (Game_Mgr.m_GameMode == GameMode.Hard)
        {
            if (Game_Mgr.Inst.m_GameTime <= 120.0f)
            {
                m_MaxHp = 70;
                m_CurHp = 70;

                m_MvSpeed = 3.0f;
            }
            else if (120.0f < Game_Mgr.Inst.m_GameTime &&
                    Game_Mgr.Inst.m_GameTime <= 240.0f)
            {
                m_MaxHp = 150;
                m_CurHp = 150;

                m_MvSpeed = 3.5f;
            }
            else if (240.0f < Game_Mgr.Inst.m_GameTime &&
            Game_Mgr.Inst.m_GameTime <= 360.0f)
            {
                m_MaxHp = 210;
                m_CurHp = 210;

                m_MvSpeed = 4.0f;
            }
            else if (360.0f < Game_Mgr.Inst.m_GameTime &&
                     Game_Mgr.Inst.m_GameTime <= 480.0f)
            {
                m_MaxHp = 300;
                m_CurHp = 300;

                m_MvSpeed = 4.5f;
            }
            else if (480.0f < Game_Mgr.Inst.m_GameTime &&
             Game_Mgr.Inst.m_GameTime <= 600.0f)
            {
                m_MaxHp = 400;
                m_CurHp = 400;

                m_MvSpeed = 5.0f;
            }
            else if (600.0f < Game_Mgr.Inst.m_GameTime)
            {
                m_MaxHp = 500;
                m_CurHp = 500;

                m_MvSpeed = 6.0f;
            }
        }
        #endregion

        #region//--- Easy
        //이지 모드
        else if (Game_Mgr.m_GameMode == GameMode.Easy)
        {
            if (Game_Mgr.Inst.m_GameTime <= 120.0f)
            {
                m_MaxHp = 40;
                m_CurHp = 40;

                m_MvSpeed = 3.0f;
            }
            else if (120.0f < Game_Mgr.Inst.m_GameTime &&
                    Game_Mgr.Inst.m_GameTime <= 240.0f)
            {
                m_MaxHp = 100;
                m_CurHp = 100;

                m_MvSpeed = 3.5f;
            }
            else if (240.0f < Game_Mgr.Inst.m_GameTime &&
            Game_Mgr.Inst.m_GameTime <= 360.0f)
            {
                m_MaxHp = 160;
                m_CurHp = 160;

                m_MvSpeed = 4.0f;
            }
            else if (360.0f < Game_Mgr.Inst.m_GameTime &&
                     Game_Mgr.Inst.m_GameTime <= 480.0f)
            {
                m_MaxHp = 240;
                m_CurHp = 240;

                m_MvSpeed = 4.5f;
            }
            else if (480.0f < Game_Mgr.Inst.m_GameTime &&
             Game_Mgr.Inst.m_GameTime <= 600.0f)
            {
                m_MaxHp = 320;
                m_CurHp = 320;

                m_MvSpeed = 5.0f;
            }
            else if (600.0f < Game_Mgr.Inst.m_GameTime)
            {
                m_MaxHp = 400;
                m_CurHp = 400;

                m_MvSpeed = 6.0f;
            }
        }
        #endregion
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
