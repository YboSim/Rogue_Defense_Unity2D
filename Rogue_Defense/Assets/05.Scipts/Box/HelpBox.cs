using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpBox : MonoBehaviour
{
    Button m_ExitBtn;

    // Start is called before the first frame update
    void Start()
    {
        m_ExitBtn = GetComponentInChildren<Button>();

        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.01f, 0.01f, 0.0f);
        if (transform.localScale.x >= 1.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
