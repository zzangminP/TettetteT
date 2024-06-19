using UnityEngine;

public class BlockSlotSetActive : MonoBehaviour
{
    private Transform[] BlockSlots;


    void Start()
    {
        switch (GameManager.instance.currentStage)
        {

            case 2:
                {
                    for (int i = 0; i < 2; i++)
                    {
                        BlockSlots[i] = transform.GetChild(i + 9);
                        //BlockSlots[1] = transform.GetChild(10);

                        BlockSlots[i].gameObject.SetActive(true);
                        //BlockSlots[1].gameObject.SetActive(true);
                    }
                }
                break;
            case 3:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        BlockSlots[i] = transform.GetChild(i + 9);
                        //BlockSlots[1] = transform.GetChild(10);

                        BlockSlots[i].gameObject.SetActive(true);
                        //BlockSlots[1].gameObject.SetActive(true);
                    }

                }break;
        }
    }


}
