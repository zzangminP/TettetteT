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
        // 3, 2, 1 ī��Ʈ�ٿ��� ���� �ݺ���
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();

            // ū �۾����� ���� �۾� ũ��� �����ϴ� �ִϸ��̼�
            countdownText.transform.localScale = Vector3.one * 30f; // ���� ũ��
            countdownText.transform.DOScale(Vector3.one * 10f, 1f); // 1�� ���� ũ�⸦ ����

            yield return new WaitForSeconds(1f); // 1�� ���
        }

        // ī��Ʈ�ٿ��� ���� �� ������ �۾�
        countdownText.text = "Go!";
        countdownText.transform.localScale = Vector3.one * 30f;
        countdownText.transform.DOScale(Vector3.one*10f, 1f);
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false); // ī��Ʈ�ٿ� �ؽ�Ʈ ��Ȱ��ȭ
        stage.SetActive(true);
    }
}
