using UnityEngine;

public class TetrominoFactory : MonoBehaviour
{
    public GameObject tilePrefab;


    //public ParticleSystem effect;
    //public GameObject effectPrefab;

    public Transform CreateTetromino(int index, Transform parent, Vector3 position, bool isGhost = false)
    {
        Color32 color = Color.white;

        Transform tetromino = new GameObject(isGhost ? "GhostTetromino" : "Tetromino").transform;
        //effect = gameObject.AddComponent<ParticleSystem>();
        tetromino.SetParent(parent);
        tetromino.localPosition = position;
        tetromino.localRotation = Quaternion.identity;
        //tetromino.GetComponent<ParticleSystem>();

        switch (index)
        {
            case 0: // I : 하늘색
                color = new Color32(115, 251, 253, 255);
                CreateTile(tetromino, new Vector2(-2f, 0.0f), color);
                CreateTile(tetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(tetromino, new Vector2(0f, 0.0f), color);
                CreateTile(tetromino, new Vector2(1f, 0.0f), color);
                break;

            case 1: // J : 파란색
                color = new Color32(0, 33, 245, 255);
                CreateTile(tetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(tetromino, new Vector2(0f, 0.0f), color);
                CreateTile(tetromino, new Vector2(1f, 0.0f), color);
                CreateTile(tetromino, new Vector2(-1f, 1.0f), color);
                break;

            case 2: // L : 귤색
                color = new Color32(243, 168, 59, 255);
                CreateTile(tetromino, new Vector2(-1f, 0.0f), color);
                CreateTile(tetromino, new Vector2(0f, 0.0f), color);
                CreateTile(tetromino, new Vector2(1f, 0.0f), color);
                CreateTile(tetromino, new Vector2(1f, 1.0f), color);
                break;

            case 3: // O : 노란색
                color = new Color32(255, 253, 84, 255);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 1f), color);
                CreateTile(tetromino, new Vector2(1f, 1f), color);
                break;

            case 4: // S : 녹색
                color = new Color32(117, 250, 76, 255);
                CreateTile(tetromino, new Vector2(-1f, -1f), color);
                CreateTile(tetromino, new Vector2(0f, -1f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(1f, 0f), color);
                break;

            case 5: // T : 자주색
                color = new Color32(155, 47, 246, 255);
                CreateTile(tetromino, new Vector2(-1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 1f), color);
                break;

            case 6: // Z : 빨간색
                color = new Color32(235, 51, 35, 255);
                CreateTile(tetromino, new Vector2(-1f, 1f), color);
                CreateTile(tetromino, new Vector2(0f, 1f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(1f, 0f), color);
                break;
        }

        if (isGhost)
        {
            foreach (Transform child in tetromino)
            {
                var tile = child.GetComponent<Tile>();
                if (tile != null)
                {
                    Color c = tile.color;
                    c.a = 0.3f; // 반투명하게 변경
                    tile.color = c;
                }
            }
        }

        return tetromino;
    }

    private void CreateTile(Transform parent, Vector2 position, Color color, int order = 1)
    {
        //var effectGoObj = Instantiate(effectPrefab);
        //ParticleSystem effectGo = effectGoObj.GetComponent<ParticleSystem>();
        var go = Instantiate(tilePrefab);
        //GameObject particleEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        //ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();

        go.transform.parent = parent;
        go.transform.localPosition = position;
       

        var tile = go.GetComponent<Tile>();
        tile.color = color;
        tile.sortingOrder = order;
        
    }
}

