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

    
    public static RectInt bounds
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
        //this.tetrominoes = GetComponent<TetrominoData>();
        //this.board = GetComponent<Board>();
        //��Ʈ�ι̳� ����
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    //�����Ҷ� ��Ʈ�ι̳� ����
    private void Start()
    {
        SpawnPiece();
    }

    //��Ʈ�ι̳� ������ �� �������� ����
    public void SpawnPiece()
    {
        //spawnPosition = new Vector3Int(0, 0, 0);
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        Set(activePiece);

    }


    //�����ϱ��� ��Ʈ�ι̳� ��ġ ����
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }


    private bool IsValidPosition(Piece piece, Vector2Int position)
    {
        RectInt bounds;
        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
        }
        //if()


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
