using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonControll : MonoBehaviour
{
    public SceneTransition sceneTransition;

    public void Playbutton()
    {
        sceneTransition.ChangeScene("Stage1");

    }
}
