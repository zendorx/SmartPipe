using System.Numerics;
using Unity.Mathematics;

namespace MatchGamesExample.GamePlay
{
    public class Utils
    {
        public const float spacing = 1.1f;
        
        public static Vector3 IndexToPosition(int2 pos)
        {
            return new Vector3(pos.x * spacing, - pos.y * spacing, 0f);
        }

        public static int2 PositionToIndex(Vector3 position)
        {
            return new int2((int) (position.X + spacing/2 / spacing), (int) (- (position.Y - spacing/2) / spacing));
        }
    }
}