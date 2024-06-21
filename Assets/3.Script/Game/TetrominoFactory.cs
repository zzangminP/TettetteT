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

            case 7: // 십자모양 : 흰색
                color = new Color32(255, 255, 255, 255);
                CreateTile(tetromino, new Vector2(1f, 0f), color);
                CreateTile(tetromino, new Vector2(-1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 1f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, -1f), color);
                break;

            case 8: // 5칸 계단모양 : 핑크
                color = new Color32(255, 0, 174, 255);
                CreateTile(tetromino, new Vector2(-1f, 1f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 1f), color);
                CreateTile(tetromino, new Vector2(1f, -1f), color);
                CreateTile(tetromino, new Vector2(1f, 0f), color);
                break;

            case 9: // 5칸 ㄷ모양 : 검은색
                color = new Color32(0, 0, 0, 255);
                CreateTile(tetromino, new Vector2(-1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(1f, 0f), color);
                CreateTile(tetromino, new Vector2(-1f, 1f), color);
                CreateTile(tetromino, new Vector2(1f, 1f), color);
                break;

            case 10: // 5칸 따봉모양 : 뭔색인지모름
                color = new Color32(169, 4, 250, 255);
                CreateTile(tetromino, new Vector2(-1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(-1f, 1f), color);
                CreateTile(tetromino, new Vector2(-1f, -1f), color);
                CreateTile(tetromino, new Vector2(0f, -1f), color);
                break;

            case 11: // 5칸 따봉모양 좌우 반전 : 뭔색인지모름
                color = new Color32(4, 200, 169, 255);
                CreateTile(tetromino, new Vector2(-1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 1f), color);
                CreateTile(tetromino, new Vector2(-1f, -1f), color);
                CreateTile(tetromino, new Vector2(0f, -1f), color);
                break;

            case 12: // 1칸 : 색상 랜덤
                color = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                break;

                /*
            case 11: // T 큰거
                color = new Color32(250, 180, 51, 255);
                CreateTile(tetromino, new Vector2(-1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 0f), color);
                CreateTile(tetromino, new Vector2(1f, 0f), color);
                CreateTile(tetromino, new Vector2(0f, 1f), color);
                CreateTile(tetromino, new Vector2(0f, 2f), color);
                break;
            */
                /*
                case 10: // 5칸 무지개같은 모양 : 무지개
                   color = new Color(Mathf.Sin(Time.time), Mathf.Cos(Time.time), Mathf.Sin(Time.time / 2));
                    CreateTile(tetromino, new Vector2(-1f, -1f), color);
                    CreateTile(tetromino, new Vector2(0f, 0f), color);
                    CreateTile(tetromino, new Vector2(1f, -1f), color);
                    CreateTile(tetromino, new Vector2(-2f, -2f), color);
                    CreateTile(tetromino, new Vector2(2f, -2f), color);
                    break;*/
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

