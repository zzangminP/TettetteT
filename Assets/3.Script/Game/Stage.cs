
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    //��ǥ ����
    [SerializeField]
    private int goalLine;
    public int deletedLineCount =0;

    private GameObject particleEffect;
    public GameObject particlePrefab;


    private bool isGameover = false;
    private bool isHardDrop = false;


    //private void OnDestroy()
    //{
    //    InstantiateParticleEffect();
    //}


    private CameraShake cmrShake;





    //��ƼŬ
    void InstantiateParticleEffect()
    {
        // ���� ����� ��ġ�� ��ƼŬ �ý����� �����մϴ�.
        GameObject particleEffect = Instantiate(particlePrefab, new Vector3(ghostTetromino.position.x, ghostTetromino.position.y - 1, 0), Quaternion.identity);

        //particleEffect.transform.SetParent(ghostTetromino);
        // ��ƼŬ �ý����� ����մϴ�.
        ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();

        }


        // ��ƼŬ �ý����� ����� ������ �ڵ����� ������Ʈ�� �ı��մϴ�.
        //Destroy(particleEffect, ps.main.duration);
    }




    void SpawnNextTetromino()
    {
        if (currentTetromino != null)
        {
            Destroy(currentTetromino.gameObject);
        }

        currentTetromino = tetrominoFactory.CreateTetromino(nextTetrominoIndex, tetrominoNode, new Vector3(0, halfHeight, 0));
        CreateGhostTetromino(); // ��Ʈ ��Ʈ�ι̳� ����
        nextTetrominoIndex = Random.Range(0, 13); // ��Ʈ�ι̳� �ε��� ���� Random.Range(0, 7);
        DisplayNextTetromino();
    }

    void DisplayNextTetromino()
    {
        foreach (Transform child in nextTetrominoNode)
        {
            Destroy(child.gameObject);
        }

        tetrominoFactory.CreateTetromino(nextTetrominoIndex, nextTetrominoNode, new Vector3(-12.8f, 7.7f, 0));
    }

    bool MoveTetromino(Vector3 moveDir, bool isRotate, bool isHardDrop)
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

                if (isHardDrop)
                {
                    InstantiateParticleEffect();
                }

                AddToBoard(currentTetromino);
                DestroyGhostTetromino();
                CheckBoardColumn();
                SpawnNextTetromino();

                if (!CanMoveTo(currentTetromino))
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
                c.a = 0.3f; // �������ϰ� ����
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
        ghostTetromino.position += Vector3.up; // �ùٸ� ��ġ�� �� ĭ ���� �̵�
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
                //���� �� ����� ī��Ʈ
                deletedLineCount++;
                //CheckGoal();
                //ī�޶� ��鸲
                var tmp = cmrShake.shakeAmount;
                cmrShake.shakeAmount = 1.5f;
                cmrShake.ShakeForTime(1);
                cmrShake.shakeAmount = tmp;

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

    
    //IEnumerator Wait_co()
    //{
    //    yield return new WaitForSeconds(5f);
    //}

    public void Start()
    {


        //CountdownController cnt = GetComponent<CountdownController>();
        //cnt.GetReady();

        cmrShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();

        if (tetrominoFactory == null)
        {
            Debug.LogError("TetrominoFactory is not assigned.");
            return;
        }

        tetrominoFactory.tilePrefab = tilePrefab; // ���丮�� Ÿ�� ������ �Ҵ�

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

        
        nextTetrominoIndex = Random.Range(0, 13); // ��Ʈ�ι̳� �ε��� ���� Range   Random.Range(0, 7);
        SpawnNextTetromino();
    }
    void HardDrop()
    {
        while (MoveTetromino(Vector3.down, false, true)) { }
        cmrShake.ShakeForTime(0.5f);
        //InstantiateParticleEffect();
    }

    void HandleInput()
    {
        Vector3 moveDir = Vector3.zero;
        bool isRotate = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("���� ����");
            moveDir.x = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("������ ����");
            moveDir.x = 1;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {

            Debug.Log("Z����");
            isRotate = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("�Ʒ� ����");
            moveDir.y = -1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space ����");
            HardDrop();

            //InstantiateParticleEffect();

        }

        if (Time.time > nextFallTime)
        {
            nextFallTime = Time.time + fallCycle;
            moveDir = Vector3.down;
            isRotate = false;
        }

        if (moveDir != Vector3.zero || isRotate)
        {
            MoveTetromino(moveDir, isRotate, false);
        }

        if (ghostTetromino != null)
        {
            UpdateGhostPosition();
        }
    }

    /*void CheckGoal()
    {
        if(deletedLineCount==goalLine)
        {
            //SceneManager.LoadScene("StageClear");

        }
        else
        {
            return;
        }
    }*/


    void Update()
    {
        HandleInput();
        
    }

}
