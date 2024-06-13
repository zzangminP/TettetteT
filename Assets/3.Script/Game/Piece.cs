using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }

    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }


    private void Start()
    {

    }
    public void Initialize(Board board, Vector3Int position, TetrominoData data )
    {
        this.board = board;
        this.position = position;
        this.data = data;

        if(this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    /*
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
            //transform.position += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
            //transform.position += Vector3.right;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(Vector2Int.down);
            //transform.position += Vector3.down;
        }
        
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    //transform.position += Vector3.up;
        //}
    }
*/
    private void Move(Vector2Int translation)
    {
        
        Vector3Int newPosition = new Vector3Int(0, 0, 0);
        newPosition.x += translation.x;
        newPosition.y += translation.y;



    }
    
}
