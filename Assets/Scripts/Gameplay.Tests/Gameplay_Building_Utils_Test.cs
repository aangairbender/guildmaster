using System.Collections;
using System.Collections.Generic;
using Gameplay.Building;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Gameplay_Building_Utils_Test
{
    // A Test behaves as an ordinary method
    [Test]
    public void ExtractOuterWalls()
    {
        var walls = new List<Wall>
        {
            new Wall(new Vector2Int(0, 0), new Vector2Int(1, 0)),
            new Wall(new Vector2Int(1, 0), new Vector2Int(2, 1)),
            new Wall(new Vector2Int(2, 1), new Vector2Int(2, 0)),
            new Wall(new Vector2Int(2, 1), new Vector2Int(3, 1)),
            new Wall(new Vector2Int(3, 1), new Vector2Int(3, 0)),
            new Wall(new Vector2Int(3, 0), new Vector2Int(2, -1)),
            new Wall(new Vector2Int(2, -1), new Vector2Int(1, -2)),
            new Wall(new Vector2Int(1, -2), new Vector2Int(1, -1)),
            new Wall(new Vector2Int(1, -1), new Vector2Int(1, 0)),
        };

        var outerWalls = Utils.ExtractOuterWalls(walls);

        var expectedOuterWalls = new List<Wall>
        {
            new Wall(new Vector2Int(1, 0), new Vector2Int(2, 1)),
            new Wall(new Vector2Int(2, 1), new Vector2Int(3, 1)),
            new Wall(new Vector2Int(3, 1), new Vector2Int(3, 0)),
            new Wall(new Vector2Int(3, 0), new Vector2Int(2, -1)),
            new Wall(new Vector2Int(2, -1), new Vector2Int(1, -2)),
            new Wall(new Vector2Int(1, -2), new Vector2Int(1, -1)),
            new Wall(new Vector2Int(1, -1), new Vector2Int(1, 0)),
        };

        Assert.AreSame(expectedOuterWalls, outerWalls);
    }
}
