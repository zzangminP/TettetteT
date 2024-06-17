using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVoulme : MonoBehaviour
{

    public AudioMixer mixer;


    public void Setlevel(float sliderValue)
    {

        if(sliderValue <=0.003)
        {
            mixer.SetFloat("MusicVol", -80f);
        }
        else
        {
            mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);

        }
            

        
    }

}
