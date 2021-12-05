using System;
using System.IO.Pipes;
using DG.Tweening;
using Match3.Scripts;
using Unity.Mathematics;
using UnityEngine;

    public class Jewel : MonoBehaviour, IPipeListener
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _sprites;

        public const float spacing = 1.1f;

        public void Start()
        {
            SmartPipe.RegisterListener<Match3_SelectJewel>(this, OnSelect);
            SmartPipe.RegisterListener<Match3_UnSelectJewel>(this, OnUnSelect);
        }

        public void SetPosition(int x, int y)
        {
            transform.position = IndexToPosition(x, y);
        }

        public void SetType(int type)
        {
            _spriteRenderer.sprite = _sprites[type];
        }

        public void OnSelect(Match3_SelectJewel action)
        {
            if (action.jewel == this)
            {
                Select();
            }
        }

        public void OnUnSelect(Match3_UnSelectJewel action)
        {
            if (action.jewel == this || action.jewel == null)
            {
                UnSelect();
            }
        }

        public void Select()
        {
            transform.DOScale(Vector3.one * 0.9f, 0.4f).SetLoops(-1);
        }

        public void UnSelect()
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
        }
        
        public static Vector3 IndexToPosition(int x, int y)
        {
            return new Vector3(x * spacing, - y * spacing, 0f);
        }

        public static int2 PositionToIndex(Vector3 position)
        {
            return new int2((int) (position.x + spacing/2 / spacing), (int) (- (position.y - spacing/2) / spacing));
        }
    }
