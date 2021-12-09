using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace MatchGamesExample.GamePlay.View
{
    public class JewelView: MonoBehaviour, IPipeListener
    {
        private static int TotalCount = 0;
        
        public SpriteRenderer spriteRenderer;
        public int2 position;
        public int type;

        private bool isSelected = false;
        private bool isFalling = false;
        public bool isTouchable = true;
        
        private int index;
        
        public bool IsAnimating()
        {
            return isFalling || !isTouchable;
        }
        
        public void Construct()
        {
            SmartPipe2.RegisterListener<GameClickAction>(this, OnSomeJewelClick);
            SmartPipe2.RegisterListener<JewelSelectAction>(this, OnSomeJewelSelected);
            SmartPipe2.RegisterListener_AsProccessor<JewelDestroyAction>(this, OnSomeJewelDestroy);
            SmartPipe2.RegisterListener_AsProccessor<JewelStartFallAction>(this, OnSomeJewelStartedFall);
            SmartPipe2.RegisterListener_AsProccessor<JewelStartFallFromTopAction>(this, OnSomeJewelStartedFallFromTop);
            index = TotalCount++;
            
            UpdateName();
        }

        private void OnSomeJewelStartedFallFromTop(JewelStartFallFromTopAction obj)
        {
            if (position.Equals(obj.pos))
            {
                gameObject.SetActive(true);
                isFalling = true;
                var pos = Utils.IndexToPosition(new int2(position.x, position.y * 2 - 5));
                pos.y = pos.y * 2.5f;
                transform.localPosition = pos;
                isTouchable = true;
                obj.SetCompleted();
            }
        }

        private void OnSomeJewelStartedFall(JewelStartFallAction obj)
        {
            if (position.Equals(obj.from))
            {
                SetPosition(obj.to, true);
                obj.SetCompleted();
            }
        }

        private void OnSomeJewelDestroy(JewelDestroyAction obj)
        {
            if (position.Equals(obj.position))
            {
                SmartPipe2.Unregister(this);
                obj.SetCompleted();
                Destroy(gameObject);
                return;
            }
        }

        private void UpdateName()
        {
            gameObject.name = $"[{index}] {position.x}x{position.y}";

            if (isFalling)
            {
                gameObject.name += " FALLING";
            }

            if (isSelected)
            {
                gameObject.name += " SELECTED";
            }
        }
        
        private void OnSomeJewelSelected(JewelSelectAction obj)
        {
            if (!obj.position.Equals(position))
            {
                UnSelect();
            }
        }

        private void OnSomeJewelClick(GameClickAction obj)
        {
            if (!obj.position.Equals(position))
            {
                return;
            }

            if (IsAnimating())
            {
                return;
            }
        
            Select();
            JewelSelectAction.Process(position);
        }
        
        protected void SetPosition(int2 pos, bool withAnimation)
        {
            position = pos;
            
            if (withAnimation)
            {
                isFalling = true;
            }
            else
            {
                transform.localPosition = Utils.IndexToPosition(pos);
            }
        }
        
        public void Select()
        {
            isSelected = true;
            transform.DOScale(Vector3.one * 0.9f, 0.4f).SetLoops(-1);
            UpdateName();
        }
        
        public void UnSelect()
        {
            isSelected = false;
            transform.DOKill();
            transform.localScale = Vector3.one;
            UpdateName();
        }

        private void Update()
        {
            const float speed = 15.5f;
            
            if (isFalling)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - speed * Time.deltaTime);

                var targetPosition = Utils.IndexToPosition(position);

                if (targetPosition.y > transform.localPosition.y)
                {
                    isFalling = false;
                    UpdateName();
                    transform.localPosition = targetPosition;
                }
            }
        }
    }
}