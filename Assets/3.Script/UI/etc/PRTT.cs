using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class PRTT : MonoBehaviour
{

    public TextMeshProUGUI tmpText;
    public string fulltext = "Press \"R\" to Main Manu";
    public float typingSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {

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
        if (Input.GetKey(KeyCode.R))
        {
            GameManager.instance.Init();
            SceneManager.LoadScene("Title");
        }
    }
}
