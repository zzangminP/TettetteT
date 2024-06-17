using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // ���̵� ȿ���� �� �̹���
    //public Scene sceneName;

    private void Start()
    {
        // ������ �� ���̵� ��
        FadeIn();
    }

    // ���̵� �� �Լ�
    public void FadeIn()
    {
        fadeImage.gameObject.SetActive(true); // �̹����� Ȱ��ȭ
        fadeImage.color = new Color(0, 0, 0, 1); // ���� ���� 1�� ���� (������ ������)
        fadeImage.DOFade(0, 2f).OnComplete(() =>
        {
            fadeImage.gameObject.SetActive(false); // ���̵� �� �Ϸ� �� �̹����� ��Ȱ��ȭ
        });
    }

    // ���̵� �ƿ� �Լ�
    public void FadeOut(string sceneName)
    {
        fadeImage.gameObject.SetActive(true); // �̹����� Ȱ��ȭ
        fadeImage.color = new Color(0, 0, 0, 0); // ���� ���� 0���� ���� (����)
        fadeImage.DOFade(1, 2f).OnComplete(() =>
        {
            // ���̵� �ƿ��� �Ϸ�Ǹ� ���ο� �� �ε�
            SceneManager.LoadScene(sceneName);
        });
    }

    // �� ��ȯ �Լ�
    public void ChangeScene(string sceneName)
    {
        FadeOut(sceneName);
    }
}