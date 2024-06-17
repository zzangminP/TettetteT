using UnityEngine;
using TMPro; // TextMeshPro
using DG.Tweening; // dotween
using System.Collections;

public class CountdownController : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject stage;

    
    public void Start()
    {
        StartCoroutine(CountdownRoutine());
        //stage = GetComponent<Stage>();
    }

    IEnumerator CountdownRoutine()
    {
        // 3, 2, 1 카운트다운을 위한 반복문
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();

            // 큰 글씨에서 보통 글씨 크기로 조절하는 애니메이션
            countdownText.transform.localScale = Vector3.one * 30f; // 시작 크기
            countdownText.transform.DOScale(Vector3.one * 10f, 1f); // 1초 동안 크기를 줄임

            yield return new WaitForSeconds(1f); // 1초 대기
        }

        // 카운트다운이 끝난 후 시작할 작업
        countdownText.text = "Go!";
        countdownText.transform.localScale = Vector3.one * 30f;
        countdownText.transform.DOScale(Vector3.one*10f, 1f);
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false); // 카운트다운 텍스트 비활성화
        stage.SetActive(true);
    }
}
