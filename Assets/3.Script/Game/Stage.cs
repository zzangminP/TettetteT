using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Editor Objects")]
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tetrominoNode;
    public Transform ghostNode;
    public Transform nextTetrominoNode; // ���� ��Ʈ�ι̳븦 ǥ���� �θ� ���

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

        // �� ���� ��忡 �̸����̱�
        // ���� �ڽ� ��� ������ width�� ���ٸ� �����°� �˾ƾ���...
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

    // ��Ʈ�ι̳븦 ���忡 �߰�
    void AddToBoard(Transform root)
    {
        // ��Ʈ ����� �ڽ� ��ϵ��� ��� �Ű���������....
        while (root.childCount > 0)
        {
            var node = root.GetChild(0);

            // ����� ��ǥ -> ������ ��ǥ�� ��ȯ
            int x = Mathf.RoundToInt(node.transform.position.x + halfWidth);
            int y = Mathf.RoundToInt(node.transform.position.y + halfHeight - 1);

            node.parent = boardNode.Find(y.ToString());
            node.name = x.ToString();
        }
    }

    // ���忡 �ϼ��� ���� ������ ����
    void CheckBoardColumn()
    {
        bool isCleared = false;

        // ��� ����� ��ȯ�ϸ鼭 �˻�
        // �ϼ��� �� == ���� �ڽ� ������ ���� ũ��
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

        // ��� �ִ� ���� �����ϸ� �Ʒ��� ����
        if (isCleared)
        {
            // �� �˻�
            for (int i = 1; i < boardNode.childCount; ++i)
            {
                // ������ �� ã��
                var column = boardNode.Find(i.ToString());

                // �̹� ��� �ִ� ���� ����
                if (column.childCount == 0)
                    continue;

                int emptyCol = 0;
                int j = i - 1;

                // ù��° ����� �ݺ�
                while (j >= 0)
                {
                    // ���� ���� ���������
                    if (boardNode.Find(j.ToString()).childCount == 0)
                    {
                        emptyCol++;
                    }
                    j--;
                }

                // ����ִ� ���� �ϳ� �̻� ������ ��ϵ��� �Ʒ��� ���
                if (emptyCol > 0)
                {
                    var targetColumn = boardNode.Find((i - emptyCol).ToString());

                    while (column.childCount > 0)
                    {
                        Transform tile = column.GetChild(0);
                        tile.parent = targetColumn;

                        // emptyCol��ŭ �Ʒ��� �� ����
                        tile.transform.position += new Vector3(0, -emptyCol, 0);
                    }
                    column.DetachChildren();
                }
            }
        }
    }

    // �̵� �������� üũ
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

    // Ÿ�� ����
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

    // ��� Ÿ���� ����
    void CreateBackground()
    {
        Color color = Color.gray;

        // Ÿ�� ����
        color.a = 0.5f;
        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int y = halfHeight; y > -halfHeight; y--)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        // �¿� �׵θ�
        color.a = 1.0f;
        for (int y = halfHeight; y > -halfHeight; y--)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        // �Ʒ� �׵θ�
        for (int x = -halfWidth - 1; x <= halfWidth; x++)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }

    // ��Ʈ�ι̳� ����
    void CreateTetromino(int index)
    {
        Color32 color = Color.white;

        currentTetromino = new GameObject("Tetromino").transform;
        currentTetromino.SetParent(tetrominoNode);
        currentTetromino.localPosition = new Vector3(0, halfHeight, 0); // ���� ��ܿ��� ����
        currentTetromino.localRotation = Quaternion.identity;

        switch (index)
        {
            // I : �ϴû�
            case 0:
                color = new Color32(115, 251, 253, 255);
                CreateTile(currentTetromino, new Vector2(-2f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0.0f), color);
                break;

            // J : �Ķ���
            case 1:
                color = new Color32(0, 33, 245, 255);
                CreateTile(currentTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(-1f, 1.0f), color);
                break;

            // L : �ֻ�
            case 2:
                color = new Color32(243, 168, 59, 255);
                CreateTile(currentTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0.0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 1.0f), color);
                break;

            // O : �����
            case 3:
                color = new Color32(255, 253, 84, 255);
                CreateTile(currentTetromino, new Vector2(0f, 0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 1f), color);
                CreateTile(currentTetromino, new Vector2(1f, 1f), color);
                break;

            // S : ���
            case 4:
                color = new Color32(117, 250, 76, 255);
                CreateTile(currentTetromino, new Vector2(-1f, -1f), color);
                CreateTile(currentTetromino, new Vector2(0f, -1f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0f), color);
                break;

            // T : ���ֻ�
            case 5:
                color = new Color32(155, 47, 246, 255);
                CreateTile(currentTetromino, new Vector2(-1f, 0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 0f), color);
                CreateTile(currentTetromino, new Vector2(1f, 0f), color);
                CreateTile(currentTetromino, new Vector2(0f, 1f), color);
                break;

            // Z : ������
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


    //���� ��Ʈ�ι̳� �̸�����
    void CreateTetrominoPreview(int index, Transform parent)
    {
        Color32 color = Color.white;

        Transform previewTetromino = new GameObject("PreviewTetromino").transform;
        previewTetromino.SetParent(parent);
        previewTetromino.localPosition = Vector3.zero;
        previewTetromino.localRotation = Quaternion.identity;

        switch (index)
        {
            // I : �ϴû�
            case 0:
                color = new Color32(115, 251, 253, 255);
                CreateTile(previewTetromino, new Vector2(-2f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0.0f), color);
                break;

            // J : �Ķ���
            case 1:
                color = new Color32(0, 33, 245, 255);
                CreateTile(previewTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(-1f, 1.0f), color);
                break;

            // L : �ֻ�
            case 2:
                color = new Color32(243, 168, 59, 255);
                CreateTile(previewTetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0.0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 1.0f), color);
                break;

            // O : �����
            case 3:
                color = new Color32(255, 253, 84, 255);
                CreateTile(previewTetromino, new Vector2(0f, 0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 1f), color);
                CreateTile(previewTetromino, new Vector2(1f, 1f), color);
                break;

            // S : ���
            case 4:
                color = new Color32(117, 250, 76, 255);
                CreateTile(previewTetromino, new Vector2(-1f, -1f), color);
                CreateTile(previewTetromino, new Vector2(0f, -1f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0f), color);
                break;

            // T : ���ֻ�
            case 5:
                color = new Color32(155, 47, 246, 255);
                CreateTile(previewTetromino, new Vector2(-1f, 0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 0f), color);
                CreateTile(previewTetromino, new Vector2(1f, 0f), color);
                CreateTile(previewTetromino, new Vector2(0f, 1f), color);
                break;

            // Z : ������
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

        // ��Ʈ �� �ٲٱ�
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
        if (ghostTetromino == null) return; // ���� �˻�

        ghostTetromino.position = currentTetromino.position;
        ghostTetromino.rotation = currentTetromino.rotation;

        while (CanMoveTo(ghostTetromino))
        {
            ghostTetromino.position += Vector3.down;
        }
        ghostTetromino.position += Vector3.up; // Move back up one step to valid position
    }
}
