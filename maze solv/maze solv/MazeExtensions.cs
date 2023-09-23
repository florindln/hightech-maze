using HightechICT.Amazeing.Client.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze_solv
{
    public static class MazeExtensions
    {
        public static Direction GetOpposite(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
