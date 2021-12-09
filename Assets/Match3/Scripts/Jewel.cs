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

        public int x;
        public int y;

        private bool isFalling = false;
        
        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }

        public void Construct()
        {
            OLD_SmartPipe.RegisterListener<Match3_SelectJewel>(this, OnSelect);
            OLD_SmartPipe.RegisterListener<Match3_UnSelectJewel>(this, OnUnSelect);
            OLD_SmartPipe.RegisterListener<Match3_DestroyJewel>(this, OnDestroyJewel);
        }

        public void OnDestroy()
        {
            OLD_SmartPipe.Unregister(this);
        }

        public void Start()
        {
        }

        public bool IsFalling()
        {
            return isFalling;
        }

        private void OnDestroyJewel(Match3_DestroyJewel obj)
        {
            if (obj.jewel == this)
            {
                Kill();
            }
        }

        public void Update()
        {
            const float speed = 5.5f;
            if (isFalling)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - speed * Time.deltaTime);

                var targetPosition = Jewel.IndexToPosition(x, y);

                if (targetPosition.y > transform.localPosition.y)
                {
                    isFalling = false;
                    UpdateName();
                    transform.localPosition = targetPosition;
                    Match3_FallEnd.Emmit(this);
                }
            }
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        public void MoveForFalling()
        {
            var targetPosition = Jewel.IndexToPosition(x, y - 5);
            transform.localPosition = targetPosition;
            isFalling = true;
        }
        
        public void SetPosition(int x, int y, bool withAnimation = false)
        {
            this.x = x;
            this.y = y;
            
            if (withAnimation)
            {
                isFalling = true;
            }
            else
            {
                transform.position = IndexToPosition(x, y);
            }
            
            UpdateName();
        }

        private void UpdateName()
        {
            gameObject.name = $"{x}x{y}";

            if (isFalling)
            {
                gameObject.name += " FALLING";
            }
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
