using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntToText : MonoBehaviour
{
    public Text deletedLineText;
    public Stage currentStage;

    private void Start()
    {
        deletedLineText.text = currentStage.deletedLineCount.ToString() + " / " + currentStage.goalLine.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        deletedLineText.text = currentStage.deletedLineCount.ToString() + " / " + currentStage.goalLine.ToString();
    }
}
