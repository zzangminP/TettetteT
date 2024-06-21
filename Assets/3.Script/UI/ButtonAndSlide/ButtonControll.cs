using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonControll : MonoBehaviour
{
    public SceneTransition sceneTransition;
    private Scene scene;


    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
    }
    public void Playbutton()
    {
        if (scene.name == "Title")
        {
            GameManager.instance.currentStage = 1;
            sceneTransition.ChangeScene("HowToPlay");
        }

        if (scene.name == "HowToPlay")
        {
            sceneTransition.ChangeScene("Stage1");
        }
    }
    public void Exitbutton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(); 
#endif



    }
}
