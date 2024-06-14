using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    public int rotationIndex { get; private set; }
    public float lockDelay = 0.5f;
    public float stepDelay = 1f;
    //private float 


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
    // ������ : ����Ű
    // �����̽� : �ϵ���
    // zx : �����̼�
    private void Update()
    {
        this.board.Clear(this);

        if((Time.deltaTime/60) == 1)
        {
        Move(Vector2Int.down);

        }
        //����Ű
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            Move(Vector2Int.left);
            Debug.Log("�Է� : ����");
        }
        //������Ű
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            Move(Vector2Int.right);
            Debug.Log("�Է� : ������");
        }
        //�Ʒ�Ű
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            Move(Vector2Int.down);
            Debug.Log("�Է� : �Ʒ���");

        }

        //z ������
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Rotation(-1);
        }

        //x ������
        if (Input.GetKeyDown(KeyCode.X))
        {
            Rotation(1);
        }

        this.board.Set(this);
    }

    private void ApplyRotationMatrix(int direction)
    {

        //float[] matrix = Data.RotationMatrix;

        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];
            int x, y;

            
            switch (this.data.tetromino)
            {
                case Tetromino.O:
                    return;
                case Tetromino.I:
                    {
                        //cell.x -= 0.5f;
                        //cell.y -= 0.5f;
                        x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                        y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                        break;
                    }
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);


        }
    }


    private void Rotation(int direction)
    {
        // Store the current rotation in case the rotation fails
        // and we need to revert
        int originalRotation = rotationIndex;

        // Rotate all of the cells using a rotation matrix
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        // Revert the rotation if the wall kick tests fail
        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);
        //return true;
        //�����Ϳ� �ִ� ��ű �� ��������
        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];
            //��ű�����ϰ� ���������� ������
            if(Move(translation))
            {
                return true; 
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }


    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
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
