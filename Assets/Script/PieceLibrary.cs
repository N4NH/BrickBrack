using UnityEngine;

public static class PieceLibrary
{
    // Mỗi shape là các offset (dx,dy) tính từ anchor (ax,ay)
    public static readonly Vector2Int[][] Shapes = new Vector2Int[][]
    {
        // 1x1
        new Vector2Int[] { new(0,0) },

        // 1x2 ngang
        new Vector2Int[] { new(0,0), new(1,0) },

        // 1x3 ngang
        new Vector2Int[] { new(0,0), new(1,0), new(2,0) },

        // 2x2
        new Vector2Int[] { new(0,0), new(1,0), new(0,1), new(1,1) },

        // L 3 ô
        new Vector2Int[] { new(0,0), new(0,1), new(1,0) },

        // L 4 ô
        new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(1,0) },

        // L 5 ô
        new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(1,0), new(2,0) },

        // 3x3 ô
        new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(1,0), new(1,1), new(1,2), new(2,0), new(2,1), new(2,2) },

        // L 4 ô xoay ngang
        new Vector2Int[] { new(0,0), new(0,1), new(1,0), new(2,0) },

        // L 5 ô xoay ngang
        new Vector2Int[] { new(0,0), new(0,1), new(1,0), new(2,0), new(3,0) },

        // L 3 ô đảo
        new Vector2Int[] { new(0,0), new(0,-1), new(-1,0) },

        // L 4 ô đảo
        new Vector2Int[] { new(0,0), new(0,-1), new(0,-2), new(-1,0) },

        // L 5 ô đảo
        new Vector2Int[] { new(0,0), new(0,-1), new(0,-2), new(-1,0), new(-2,0) },

        // L 4 ô xoay ngang đảo
        new Vector2Int[] { new(0,0), new(0,-1), new(-1,0), new(-2,0) },

        // L 5 ô xoay ngang đảo
        new Vector2Int[] { new(0,0), new(0,-1), new(-1,0), new(-2,0), new(-3,0) },

        // L 3 ô xoay đảo
        new Vector2Int[] { new(0,0), new(0,1), new(-1,0) },

        // L 4 ô xoay đảo
        new Vector2Int[] { new(0,0), new(0,-1), new(0,-2), new(1,0) },

        // L 5 ô xoay đảo
        new Vector2Int[] { new(0,0), new(0,-1), new(0,-2), new(1,0), new(2,0) },

        // L 3 ô đảo xoay
        new Vector2Int[] { new(0,0), new(0,-1), new(1,0) },

        // L 4 ô đảo xoay
        new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(-1,0) },

        // L 5 ô đảo xoay
        new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(-1,0), new(-2,0) },
    };

    public static Vector2Int[] RandomShape()
    {
        return Shapes[Random.Range(0, Shapes.Length)];
    }
}