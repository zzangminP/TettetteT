using UnityEngine.UI;
using UnityEngine;

public class ShowCurrentStage : MonoBehaviour
{
    public Text currentStageText;
    //public Stage currentStage;


    // Update is called once per frame
    void Update()
    {
        currentStageText.text = GameManager.instance.currentStage.ToString();
    }
}
