using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Editor Objects")]
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tetrominoNode;
    public Transform ghostNode;
    public Transform nextTetrominoNode; // 다음 테트로미노를 표시할 부모 노드

    [Header("Game Settings")]
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

    private void Start()
    {
        //gameoverPanel.SetActive(false);

        halfWidth = Mathf.RoundToInt(boardWidth * 0.5f);
        halfHeight = Mathf.RoundToInt(boardHeight * 0.5f);

        nextFallTime = Time.time + fallCycle;

        CreateBackground();

        // 각 행의 노드에 이름붙이기
        // 행의 자식 노드 개수가 width랑 같다면 꽉차는걸 알아야함...
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

    // 테트로미노를 보드에 추가
    void AddToBoard(Transform root)
    {
        // 루트 노드의 자식 블록들이 모두 옮겨질때까지....
        while (root.childCount > 0)
        {
            var node = root.GetChild(0);

            // 블록의 좌표 -> 보드의 좌표로 변환
            int x = Mathf.RoundToInt(node.transform.position.x + halfWidth);
            int y = Mathf.RoundToInt(node.transform.position.y + halfHeight - 1);

            node.parent = boardNode.Find(y.ToString());
            node.name = x.ToString();
        }
    }

    // 보드에 완성된 행이 있으면 삭제
    void CheckBoardColumn()
    {
        bool isCleared = false;

        // 모든 행들을 순환하면서 검사
        // 완성된 행 == 행의 자식 갯수가 가로 크기
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

        // 비어 있는 행이 존재하면 아래로 당기기
        if (isCleared)
        {
            // 줄 검사
            for (int i = 1; i < boardNode.childCount; ++i)
            {
                // 현재의 열 찾기
                var column = boardNode.Find(i.ToString());

                // 이미 비어 있는 행은 무시
                if (column.childCount == 0)
                    continue;

                int emptyCol = 0;
                int j = i - 1;

                // 첫번째 행까지 반복
                while (j >= 0)
                {
                    // 위의 행이 비어있으면
                    if (boardNode.Find(j.ToString()).childCount == 0)
                    {
                        emptyCol++;
                    }
                    j--;
                }

                // 비어있는 행이 하나 이상 있으면 블록들을 아래로 당김
                if (emptyCol > 0)
                {
                    var targetColumn = boardNode.Find((i - emptyCol).ToString());

                    while (column.childCount > 0)
                    {
                        Transform tile = column.GetChild(0);
                        tile.parent = targetColumn;

                        // emptyCol만큼 아래로 쳐 내림
                        tile.transform.position += new Vector3(0, -emptyCol, 0);
                    }
                    column.DetachChildren();
                }
            }
        }
    }

    // 이동 가능한지 체크
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

    // 타일 생성
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

    // 배경 타일을 생성
    void CreateBackground()
    {
        Color color = Color.gray;

        // 타일 보드
        color.a = 0.5f;
        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int y = halfHeight; y > -halfHeight; y--)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        // 좌우 테두리
        color.a = 1.0f;
        for (int y = halfHeight; y > -halfHeight; y--)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        // 아래 테두리
        for (int x = -halfWidth - 1; x <= halfWidth; x++)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }

    // 테트로미노 생성
    void CreateTetromino(int index)
    {
        Color32 color = Color.white;

        currentTetromino = new GameObject("Tetromino").transform;
        currentTetromino.SetParent(tetrominoNode);
        currentTetromino.localPosition = new Vector3(0, halfHeight, 0); // 보드 상단에서 생성
        currentTetromino.localRotation = Quaternion.identity;

        switch (index)
        {
            // I : 하늘색
            case 0:
                color = new Color32(115, 251, 253, 255);
                CreateTile(currentTetromino, new Vector2(-2f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0.0f), color);
                break;

            // J : 파란색
            case 1:
                color = new Color32(0, 33, 245, 255);
                CreateTile(currentTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(-1f, 1.0f), color);
                break;

            // L : 귤색
            case 2:
                color = new Color32(243, 168, 59, 255);
                CreateTile(currentTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 1.0f), color);
                break;

            // O : 노란색
            case 3:
                color = new Color32(255, 253, 84, 255);
                CreateTile(currentTetromino, new Vector2(0f, 0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 1f), color);
                CreateTile(currentTetromino, new Vector2(1f, 1f), color);
                break;

            // S : 녹색
            case 4:
                color = new Color32(117, 250, 76, 255);
                CreateTile(currentTetromino, new Vector2(-1f, -1f), color);
                CreateTile(currentTetromino, new Vector2(0f, -1f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0f), color);
                break;

            // T : 자주색
            case 5:
                color = new Color32(155, 47, 246, 255);
                CreateTile(currentTetromino, new Vector2(-1f, 0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 1f), color);
                break;

            // Z : 빨간색
            case 6:
                color = new Color32(235, 51, 35, 255);
                CreateTile(currentTetromino, new Vector2(-1f, 1f), color);
                CreateTile(currentTetromino, new Vector2(0f, 1f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0f), color);
                break;
        }
        CreateGhostTetromino();
        UpdateGhostPosition();
    }


    //다음 테트로미노 미리보기
    void CreateTetrominoPreview(int index, Transform parent)
    {
        Color32 color = Color.white;

        Transform previewTetromino = new GameObject("PreviewTetromino").transform;
        previewTetromino.SetParent(parent);
        previewTetromino.localPosition = Vector3.zero;
        previewTetromino.localRotation = Quaternion.identity;

        switch (index)
        {
            // I : 하늘색
            case 0:
                color = new Color32(115, 251, 253, 255);
                CreateTile(previewTetromino, new Vector2(-2f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0.0f), color);
                break;

            // J : 파란색
            case 1:
                color = new Color32(0, 33, 245, 255);
                CreateTile(previewTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(-1f, 1.0f), color);
                break;

            // L : 귤색
            case 2:
                color = new Color32(243, 168, 59, 255);
                CreateTile(previewTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 1.0f), color);
                break;

            // O : 노란색
            case 3:
                color = new Color32(255, 253, 84, 255);
                CreateTile(previewTetromino, new Vector2(0f, 0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 1f), color);
                CreateTile(previewTetromino, new Vector2(1f, 1f), color);
                break;

            // S : 녹색
            case 4:
                color = new Color32(117, 250, 76, 255);
                CreateTile(previewTetromino, new Vector2(-1f, -1f), color);
                CreateTile(previewTetromino, new Vector2(0f, -1f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0f), color);
                break;

            // T : 자주색
            case 5:
                color = new Color32(155, 47, 246, 255);
                CreateTile(previewTetromino, new Vector2(-1f, 0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 1f), color);
                break;

            // Z : 빨간색
            case 6:
                color = new Color32(235, 51, 35, 255);
                CreateTile(previewTetromino, new Vector2(-1f, 1f), color);
                CreateTile(previewTetromino, new Vector2(0f, 1f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0f), color);
                break;
        }
    }

    void SpawnNextTetromino()
    {
        if (currentTetromino != null)
        {
            Destroy(currentTetromino.gameObject);
        }

        CreateTetromino(nextTetrominoIndex);
        nextTetrominoIndex = Random.Range(0, 7);
        DisplayNextTetromino();
    }

    void DisplayNextTetromino()
    {
        foreach (Transform child in nextTetrominoNode)
        {
            Destroy(child.gameObject);
        }

        CreateTetrominoPreview(nextTetrominoIndex, nextTetrominoNode);
    }

    bool MoveTetromino(Vector3 moveDir, bool isRotate)
    {
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

            if ((int)moveDir.y == -1 && (int)moveDir.x == 0 && isRotate == false)
            {
                AddToBoard(currentTetromino);
                DestroyGhostTetromino();
                CheckBoardColumn();
                SpawnNextTetromino();
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

        // 고스트 색 바꾸기
        foreach (Transform child in ghostTetromino)
        {
            var tile = child.GetComponent<Tile>();
            if (tile != null)
            {
                Color c = tile.color;
                c.a = 0.3f; // Make it semi-transparent
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
        if (ghostTetromino == null) return; // 안전 검사

        ghostTetromino.position = currentTetromino.position;
        ghostTetromino.rotation = currentTetromino.rotation;

        while (CanMoveTo(ghostTetromino))
        {
            ghostTetromino.position += Vector3.down;
        }
        ghostTetromino.position += Vector3.up; // Move back up one step to valid position
    }
}
