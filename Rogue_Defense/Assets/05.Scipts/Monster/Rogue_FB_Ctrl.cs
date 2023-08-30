using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue_FB_Ctrl : MonoBehaviour
{
    public int m_MaxHp; //최대 Hp
    public int m_CurHp; //현재 Hp

    public float m_MvSpeed;

    private void OnEnable()
    {
        m_MaxHp = 10000;
        m_CurHp = 10000;

        m_MvSpeed = 2;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
