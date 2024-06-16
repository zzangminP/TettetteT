using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tetrominoNode;
    public Transform ghostNode;
    public Transform nextTetrominoNode;
    public TetrominoFactory tetrominoFactory;

    [Range(4, 40)]
    public int boardWidth = 10;
    [Range(5, 20)]
    public int boardHeight = 20;
    public float fallCycle = 1.0f;

    private int halfWidth;
    private int halfHeight;
    private float nextFallTime;

    private Transform ghostTetromino;
    private Transform currentTetromino;
    private int nextTetrominoIndex;

    private bool isGameover = false;


    void SpawnNextTetromino()
    {
        if (currentTetromino != null)
        {
            Destroy(currentTetromino.gameObject);
        }

        currentTetromino = tetrominoFactory.CreateTetromino(nextTetrominoIndex, tetrominoNode, new Vector3(0, halfHeight, 0));
        CreateGhostTetromino(); // 고스트 테트로미노 생성
        nextTetrominoIndex = Random.Range(0, 7);
        DisplayNextTetromino();
    }

    void DisplayNextTetromino()
    {
        foreach (Transform child in nextTetrominoNode)
        {
            Destroy(child.gameObject);
        }

        tetrominoFactory.CreateTetromino(nextTetrominoIndex, nextTetrominoNode, new Vector3(-12.8f,7.7f,0));
    }

    bool MoveTetromino(Vector3 moveDir, bool isRotate)
    {
        if (currentTetromino == null)
        {
            Debug.LogError("currentTetromino is not initialized.");
            return false;
        }

        Vector3 oldPos = currentTetromino.transform.position;
        Quaternion oldRot = currentTetromino.transform.rotation;

        currentTetromino.transform.position += moveDir;
        if (isRotate)
        {
            currentTetromino.transform.rotation *= Quaternion.Euler(0, 0, 90);
        }

        if (!CanMoveTo(currentTetromino))
        {
            currentTetromino.transform.position = oldPos;
            currentTetromino.transform.rotation = oldRot;

            if ((int)moveDir.y == -1 && (int)moveDir.x == 0 && !isRotate)
            {
                AddToBoard(currentTetromino);
                DestroyGhostTetromino();
                CheckBoardColumn();
                SpawnNextTetromino();

                if(!CanMoveTo(currentTetromino))
                {
                    isGameover = true;
                    SceneManager.LoadScene("GameOver");
                }
            }

            return false;
        }

        UpdateGhostPosition();
        return true;
    }

    void CreateGhostTetromino()
    {
        if (ghostTetromino != null)
        {
            Destroy(ghostTetromino.gameObject);
        }

        ghostTetromino = Instantiate(currentTetromino, ghostNode);
        ghostTetromino.name = "GhostTetromino";

        foreach (Transform child in ghostTetromino)
        {
            var tile = child.GetComponent<Tile>();
            if (tile != null)
            {
                Color c = tile.color;
                c.a = 0.3f; // 반투명하게 변경
                tile.color = c;
            }
        }
        UpdateGhostPosition();
    }

    void DestroyGhostTetromino()
    {
        if (ghostTetromino != null)
        {
            Destroy(ghostTetromino.gameObject);
            ghostTetromino = null;
        }
    }

    void UpdateGhostPosition()
    {
        if (ghostTetromino == null || currentTetromino == null) return;

        ghostTetromino.position = currentTetromino.position;
        ghostTetromino.rotation = currentTetromino.rotation;

        while (CanMoveTo(ghostTetromino))
        {
            ghostTetromino.position += Vector3.down;
        }
        ghostTetromino.position += Vector3.up; // 올바른 위치로 한 칸 위로 이동
    }

    void CreateBackground()
    {
        Color color = Color.gray;

        color.a = 0.5f;
        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int y = halfHeight; y > -halfHeight; y--)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        color.a = 1.0f;
        for (int y = halfHeight; y > -halfHeight; y--)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        for (int x = -halfWidth - 1; x <= halfWidth; x++)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }

    Tile CreateTile(Transform parent, Vector2 position, Color color, int order = 1)
    {
        var go = Instantiate(tilePrefab);
        go.transform.parent = parent;
        go.transform.localPosition = position;

        var tile = go.GetComponent<Tile>();
        tile.color = color;
        tile.sortingOrder = order;

        return tile;
    }

    void AddToBoard(Transform root)
    {
        while (root.childCount > 0)
        {
            var node = root.GetChild(0);

            int x = Mathf.RoundToInt(node.transform.position.x + halfWidth);
            int y = Mathf.RoundToInt(node.transform.position.y + halfHeight - 1);

            node.parent = boardNode.Find(y.ToString());
            node.name = x.ToString();
        }
    }

    void CheckBoardColumn()
    {
        bool isCleared = false;

        foreach (Transform column in boardNode)
        {
            if (column.childCount == boardWidth)
            {
                foreach (Transform tile in column)
                {
                    Destroy(tile.gameObject);
                }

                column.DetachChildren();
                isCleared = true;
            }
        }

        if (isCleared)
        {
            for (int i = 1; i < boardNode.childCount; ++i)
            {
                var column = boardNode.Find(i.ToString());

                if (column.childCount == 0)
                    continue;

                int emptyCol = 0;
                int j = i - 1;

                while (j >= 0)
                {
                    if (boardNode.Find(j.ToString()).childCount == 0)
                    {
                        emptyCol++;
                    }
                    j--;
                }

                if (emptyCol > 0)
                {
                    var targetColumn = boardNode.Find((i - emptyCol).ToString());

                    while (column.childCount > 0)
                    {
                        Transform tile = column.GetChild(0);
                        tile.parent = targetColumn;

                        tile.transform.position += new Vector3(0, -emptyCol, 0);
                    }
                    column.DetachChildren();
                }
            }
        }
    }

    bool CanMoveTo(Transform root)
    {
        for (int i = 0; i < root.childCount; ++i)
        {
            var node = root.GetChild(i);
            int x = Mathf.RoundToInt(node.transform.position.x + halfWidth);
            int y = Mathf.RoundToInt(node.transform.position.y + halfHeight - 1);

            if (x < 0 || x > boardWidth - 1)
                return false;

            if (y < 0)
                return false;

            var column = boardNode.Find(y.ToString());

            if (column != null && column.Find(x.ToString()) != null)
                return false;
        }

        return true;
    }


    void Start()
    {
        if (tetrominoFactory == null)
        {
            Debug.LogError("TetrominoFactory is not assigned.");
            return;
        }

        tetrominoFactory.tilePrefab = tilePrefab; // 팩토리에 타일 프리팹 할당

        halfWidth = Mathf.RoundToInt(boardWidth * 0.5f);
        halfHeight = Mathf.RoundToInt(boardHeight * 0.5f);

        nextFallTime = Time.time + fallCycle;

        CreateBackground();

        for (int i = 0; i < boardHeight; ++i)
        {
            var col = new GameObject((boardHeight - i - 1).ToString());
            col.transform.position = new Vector3(0, halfHeight - i, 0);
            col.transform.parent = boardNode;
        }

        nextTetrominoIndex = Random.Range(0, 7);
        SpawnNextTetromino();
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        bool isRotate = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir.x = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir.x = 1;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isRotate = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir.y = -1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (MoveTetromino(Vector3.down, false)) { }
        }

        if (Time.time > nextFallTime)
        {
            nextFallTime = Time.time + fallCycle;
            moveDir = Vector3.down;
            isRotate = false;
        }

        if (moveDir != Vector3.zero || isRotate)
        {
            MoveTetromino(moveDir, isRotate);
        }

        if (ghostTetromino != null)
        {
            UpdateGhostPosition();
        }
    }

}
