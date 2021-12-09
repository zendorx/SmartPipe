using Unity.Mathematics;
using UnityEngine;

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
            return new int2((int) (position.x + spacing/2 / spacing), (int) (- (position.y - spacing/2) / spacing));
        }
    }
}