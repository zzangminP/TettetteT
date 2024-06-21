using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverSceneTransition : MonoBehaviour
{
    public Text gameOver;
    public TextMeshProUGUI tmpText;
    public string fulltext = "Press \"R\" to Main Manu";
    public float typingSpeed = 0.1f;

    private void Start()
    {
        gameOver.color = new Color(gameOver.color.r, gameOver.color.g, gameOver.color.b, 0);
        

        StartCoroutine(ShowText());

    }

    IEnumerator ShowText()
    {
        gameOver.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        tmpText.text = "";

        foreach (char c in fulltext)
        {
            tmpText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            GameManager.instance.Init();
            SceneManager.LoadScene("Title");
        }
    }

}
