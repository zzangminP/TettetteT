using UnityEngine;
using UnityEngine.Tilemaps;
public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    //Ŀ���� ����
    public Vector2Int[] cells { get; private set;}


    public void Initialize()
    {
        cells = Data.Cells[tetromino];
    }



}