using Unity.Mathematics;
using UnityEngine;

namespace MatchGamesExample.GamePlay.View
{
    public class JewelView: MonoBehaviour, IPipeListener
    {
        public SpriteRenderer spriteRenderer;
        public int2 position;
        public int type;

        public void Construct(int2 position, int type, Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            this.position = position;
            this.type = type;
        }

        public void SetPosition(int2 pos)
        {
            position = pos;
            transform.localPosition = Jewel.IndexToPosition(pos.x, pos.y);
        }
    }
}