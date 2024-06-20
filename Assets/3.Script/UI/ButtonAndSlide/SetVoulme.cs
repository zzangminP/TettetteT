using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVoulme : MonoBehaviour
{

    public AudioMixer mixer;


    public void Setlevel(float sliderValue)
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

}
