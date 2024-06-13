using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }


    //초기설정
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.data = data;
        this.board = board;
        this.position = position;



        if (cells == null)
        {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    // 키조작
    private void Update()
    {
        this.board.Clear(this);

        //왼쪽키
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
            Move(Vector2Int.left);
            Debug.Log("입력 : 왼쪽");
        }
        //오른쪽키
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
           
            Move(Vector2Int.right);
            Debug.Log("입력 : 오른쪽");
        }
        //아래키
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            
            Move(Vector2Int.down);
            Debug.Log("입력 : 아래쪽");
            
        }

        this.board.Set(this);
    }

    //테트로미노 움직임
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        // Only save the movement if the new position is valid
        if (valid)
        {
            this.position = newPosition;

        }

        return valid;
    }
    
}
