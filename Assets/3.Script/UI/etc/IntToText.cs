using UnityEngine;
using UnityEngine.UI;

public class IntToText : MonoBehaviour
{
    public Text deletedLineText;
    public Stage currentStage;


    // Update is called once per frame
    void Update()
    {
        deletedLineText.text = currentStage.deletedLineCount.ToString() + " / " + currentStage.goalLine.ToString();
    }
}
