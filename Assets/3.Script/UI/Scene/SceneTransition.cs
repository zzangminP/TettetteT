using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // 페이드 효과를 줄 이미지
    //public Scene sceneName;

    private void Start()
    {
        // 시작할 때 페이드 인
        FadeIn();
    }

    // 페이드 인 함수
    public void FadeIn()
    {
        fadeImage.gameObject.SetActive(true); // 이미지를 활성화
        fadeImage.color = new Color(0, 0, 0, 1); // 알파 값을 1로 설정 (완전히 불투명)
        fadeImage.DOFade(0, 2f).OnComplete(() =>
        {
            fadeImage.gameObject.SetActive(false); // 페이드 인 완료 후 이미지를 비활성화
        });
    }

    // 페이드 아웃 함수
    public void FadeOut(string sceneName)
    {
        fadeImage.gameObject.SetActive(true); // 이미지를 활성화
        fadeImage.color = new Color(0, 0, 0, 0); // 알파 값을 0으로 설정 (투명)
        fadeImage.DOFade(1, 2f).OnComplete(() =>
        {
            // 페이드 아웃이 완료되면 새로운 씬 로드
            SceneManager.LoadScene(sceneName);
        });
    }

    // 씬 전환 함수
    public void ChangeScene(string sceneName)
    {
        FadeOut(sceneName);
    }
}