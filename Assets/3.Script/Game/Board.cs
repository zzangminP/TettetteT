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
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    
    }
    
    
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
        //Bounds bounds = new Bounds();
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
    
 
    
    //��� ����
    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];
    
        activePiece.Initialize(this, spawnPosition, data);
    
        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        
    }
    
    
    //�׸���̳� ��ġ���� 
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    //�׸���̳� ��ġ���� �ϱ� �� Ŭ����
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    //������ �� �ִ��� Ȯ��
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
    
    }

}
