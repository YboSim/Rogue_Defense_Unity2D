using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_Mgr : MonoBehaviour
{
    float AlphaThreshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
