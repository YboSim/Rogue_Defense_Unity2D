using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange_Arrow : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name.Contains("Rogue") == true)
        {
            MonsterInRange(coll.GetComponent<Monster>());
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.name.Contains("Rogue") == true)
        {
            MonsterOutRange(coll.GetComponent<Monster>());
        }
    }

    void MonsterInRange(Monster a_Monster) //공격범위 안 몬스터리스트에 추가
    {
        Tower_Arrow.m_MonListInRange.Add(a_Monster);
    }

    void MonsterOutRange(Monster a_Monster)
    {
        Tower_Arrow.m_MonListInRange.Remove(a_Monster);
    }
}
