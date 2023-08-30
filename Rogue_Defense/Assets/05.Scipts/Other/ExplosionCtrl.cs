using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCtrl : MonoBehaviour
{
    Animator m_Animator = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();

        if(m_Animator !=null)
        {
            AnimatorStateInfo a_AnimInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            float a_LifeTime = a_AnimInfo.length; //폭발 애니메이션 재생시간
            Destroy(gameObject, a_LifeTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
