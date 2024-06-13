using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }

    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    


    private float lockTime;
    private float stepTime;
    private float moveTime;
    //
    private void Start()
    {
        
    }

    //private void U
    public void Initialize(Board board, Vector3Int position, TetrominoData data )
    {
        this.board = board;
        this.position = position;
        this.data = data;

        //stepTime = Time.time;
        //delayTime = Time.time - Time.deltaTime;

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
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        
        // Only save the movement if the new position is valid
        if (valid)
        {
            position = newPosition;
            moveTime = Time.time + Time.deltaTime;
            lockTime = 0f; // reset
        }

        return valid;
    }


    private void Update()
    {
        //board.Clear(this);

        // We use a timer to allow the player to make adjustments to the piece
        // before it locks in place
        lockTime += Time.deltaTime;
        /*
        // Handle rotation
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        // Handle hard drop
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }*/

        // Allow the player to hold movement keys but only after a move delay
        // so it does not move too fast
        if (Time.time > moveTime)
        {
            HandleMoveInputs();
        }

        /*// Advance the piece to the next row every x seconds
        if (Time.time > stepTime)
        {
            Step();
        }

        board.Set(this);*/
    }

    private void HandleMoveInputs()
    {
        // Soft drop movement
        if (Input.GetKey(KeyCode.S))
        {
            if (Move(Vector2Int.down))
            {
                // Update the step time to prevent double movement
                stepTime = Time.time + Time.deltaTime;//stepDelay;
                //if()
            }
        }

        // Left/right movement
        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(Vector2Int.right);
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }*/






    /*
    private bool IsBorder()
    {
        if (this.position.x == board.tilemap.localBounds.max.x ||
            this.position.x == board.tilemap.localBounds.min.x ||
            )
        {
            return true;
        }
        else
            return false;
    }
    */
}
