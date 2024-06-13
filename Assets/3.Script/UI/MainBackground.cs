using UnityEngine;

public class MainBackground : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera camera;
    private Color bgcolor = new Color();
    // Update is called once per frame
    private void Awake()
    {
        camera = GetComponent<Camera>();

    }
    void Update()
    {
        bgcolor = new Color(Mathf.Sin(Time.time), Mathf.Cos(Time.time), Mathf.Sin(Time.time/2));
        camera.backgroundColor = bgcolor;
    }
}
