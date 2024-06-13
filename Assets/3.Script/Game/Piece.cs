using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }


    //�ʱ⼳��
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

    // Ű����
    private void Update()
    {
        this.board.Clear(this);

        //����Ű
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
            Move(Vector2Int.left);
            Debug.Log("�Է� : ����");
        }
        //������Ű
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
           
            Move(Vector2Int.right);
            Debug.Log("�Է� : ������");
        }
        //�Ʒ�Ű
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            
            Move(Vector2Int.down);
            Debug.Log("�Է� : �Ʒ���");
            
        }

        this.board.Set(this);
    }

    //��Ʈ�ι̳� ������
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
