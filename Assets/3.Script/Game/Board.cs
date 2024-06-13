using UnityEngine;
using UnityEngine.Tilemaps;
public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition = new Vector3Int(0, 0, 0);

    public Vector2Int boardSize = new Vector2Int(10, 20);
    //public Board board;

    
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }

    }
    

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
        Bounds bounds = new Bounds();
        //this.tetrominoes = GetComponent<TetrominoData>();
        //this.board = GetComponent<Board>();
        //테트로미노 생성
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    //시작할때 테트로미노 생성
    private void Start()
    {
        SpawnPiece();
    }

    //테트로미노 데이터 중 랜덤으로 스폰


    /*
    public void SpawnPiece()
    {
        //spawnPosition = new Vector3Int(0, 0, 0);
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        Set(activePiece);

    }*/


    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            //GameOver();
        }
    }


    //스폰하기전 테트로미노 위치 설정
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    //움직일 수 있는지 확인
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    private void Update()
    {
        //if()

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            activePiece.transform.position += Vector3Int.left;
            
            //transform.position += Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            activePiece.transform.position += Vector3Int.right;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            activePiece.transform.position += Vector3Int.down;
        }
        /*
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.position += Vector3.up;
        }*/

    }
}
