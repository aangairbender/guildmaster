using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameplay.Building
{
    public class Layout
    {
        public const int Width = 100;
        public const int Height = 100;

        private readonly Tile[] tiles = new Tile[Width * Height];

        private readonly HashSet<Wall> walls = new();

        public IReadOnlyCollection<Wall> Walls => walls;

        public void AddWall(Wall wall) => walls.Add(wall);

        public void RemoveWall(Wall wall) => walls.Remove(wall);
    }
}
