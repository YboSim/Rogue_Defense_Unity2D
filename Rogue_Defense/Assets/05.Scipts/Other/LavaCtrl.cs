using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCtrl : MonoBehaviour
{
    public static int m_Damage = 30;
    float m_Duration = 5.0f; //용암 지속 시간
    float m_CoolTime = 0.0f; //몬스터에게 데미지를 가하는 쿨타임

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, m_Duration);
    }

    // Update is called once per frame
    void Update()
    {
        m_CoolTime += Time.deltaTime;
        if(m_CoolTime >= 1.0f)
        {
            Damage();

            m_CoolTime = 0.0f;
        }
    }

    void Damage()//쿨타임마다 용암바닥 위 몬스터들을 찾아와 데미지 입힘
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 3.0f);

        Monster a_Monster;
        foreach (Collider2D coll in colls)
        {
            a_Monster = coll.GetComponent<Monster>();
            if (a_Monster == null)
                continue;

            a_Monster.TakeDamage(m_Damage);
        }
    }

}
