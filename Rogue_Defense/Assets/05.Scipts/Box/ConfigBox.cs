using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigBox : MonoBehaviour
{
    public Button m_ExitBtn;
    public Toggle m_SoundToggle;
    public Slider m_OverallVol;
    public Slider m_EffVol;

    // Start is called before the first frame update
    void Start()
    {
        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(ExitBtnClick);

        if (m_SoundToggle != null)
            m_SoundToggle.onValueChanged.AddListener(SoundOnOff);

        if (m_OverallVol != null)
            m_OverallVol.onValueChanged.AddListener(BGMSliderChanged);

        if (m_EffVol != null)
            m_EffVol.onValueChanged.AddListener(EffSliderChanged);

        //--- üũ ����, �����̵� ���� �ε� �� UI��Ʈ�ѿ� ����
        int a_SoundOnOff = PlayerPrefs.GetInt("SoundOnOff", 1);
        if (m_SoundToggle != null)
        {
            if (a_SoundOnOff == 1)
                m_SoundToggle.isOn = true;
            else
                m_SoundToggle.isOn = false;
        }

        if (m_EffVol != null)
            m_EffVol.value = PlayerPrefs.GetFloat("EffSoundVolume", 1.0f);

        if (m_OverallVol != null)
            m_OverallVol.value = PlayerPrefs.GetFloat("BGMSoundVolume", 1.0f);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    void ExitBtnClick()
    {
        //������ �������� �ʰ� �����ص� �ѹ��� �������ش�
        if(m_SoundToggle.isOn == true)
            PlayerPrefs.SetInt("SoundOnOff", 1);
        else
            PlayerPrefs.SetInt("SoundOnOff", 0);

        float a_EffValue = m_EffVol.value;
        PlayerPrefs.SetFloat("EffSoundVolume", a_EffValue);

        float a_BGMValue = m_OverallVol.value;
        PlayerPrefs.SetFloat("BGMSoundVolume", a_BGMValue);
        //������ �������� �ʰ� �����ص� �ѹ��� �������ش�

        if (Game_Mgr.Inst != null)
        {
            if (Game_Mgr.Inst.m_DoublespdOnOff == true)
                Time.timeScale = 2.0f;
            else
                Time.timeScale = 1.0f;
        }

        Game_Mgr.m_GameState = GameState.Playing;

        Destroy(this.gameObject);
    }

    void SoundOnOff(bool value) //üũ ���°� ���� �Ǿ��� �� ȣ��Ǵ� �Լ�
    {
        if (m_SoundToggle != null)
        {
            if (value == true)
                PlayerPrefs.SetInt("SoundOnOff", 1);
            else
                PlayerPrefs.SetInt("SoundOnOff", 0);
        }

        Sound_Mgr.Instance.SoundOnOff(value);
    }

    void EffSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("EffSoundVolume", value);
        Sound_Mgr.Instance.EffSoundVolume(value);
    }

    void BGMSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("BGMSoundVolume", value);
        Sound_Mgr.Instance.BGMSoundVolume(value);
    }
}
