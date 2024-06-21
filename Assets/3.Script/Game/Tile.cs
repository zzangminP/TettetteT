using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Color color
    {
        set { spriteRenderer.color = value; }
        get { return spriteRenderer.color; }
    }

    public int sortingOrder
    {
        set { spriteRenderer.sortingOrder = value; }
        get { return spriteRenderer.sortingOrder; }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //if (spriteRenderer == null)
        //{
        //    Debug.LogError("SpriteRenderer is not attached to the Tile.");
        //}
    }
}
