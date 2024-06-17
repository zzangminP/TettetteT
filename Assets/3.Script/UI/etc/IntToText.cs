using UnityEngine;
using UnityEngine.UI;

public class IntToText : MonoBehaviour
{
    public Text deletedLineText;
    public Stage currentStage;
    // Start is called before the first frame update
    void Start()
    {
        //currentStage = GetComponent<Stage>();
    }

    // Update is called once per frame
    void Update()
    {
        deletedLineText.text = currentStage.deletedLineCount.ToString();
    }
}
