using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVoulme : MonoBehaviour
{

    public AudioMixer mixer;
    public Slider bgmSlider;
    public Slider sfxSlider;
    private void Awake()
    {
        bgmSlider.onValueChanged.AddListener(BgmSetlevel);
        sfxSlider.onValueChanged.AddListener(SfxSetlevel);
    }

    public void BgmSetlevel(float sliderValue)
    {
        


        if (sliderValue <= 0.003)
        {
            mixer.SetFloat("MusicVol", -80f);
        }
        else
        {
            float temp;
            mixer.GetFloat("MusicVol", out temp);
            if (temp < Mathf.Log10((float)0.2) * 20)
            {
                mixer.SetFloat("MusicVol", (float)0.001);
            }
            else
            {
                mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);

            }

        }



    }
    public void SfxSetlevel(float sliderValue)
    {

        if (sliderValue <= 0.003)
        {
            mixer.SetFloat("SfxVol", -80f);
        }
        else
        {
            float temp;
            mixer.GetFloat("SfxVol", out temp);
            if (temp < Mathf.Log10((float)0.2) * 20)
            {
                mixer.SetFloat("SfxVol", (float)0.001);
            }
            else
            {
                mixer.SetFloat("SfxVol", Mathf.Log10(sliderValue) * 20);

            }

        }
    }

}
