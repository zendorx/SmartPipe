using MatchGamesExample.GamePlay.View;
using Unity.Mathematics;
using UnityEngine;

namespace MatchGamesExample.GamePlay
{
    public class Match3Factory : MonoBehaviour, IPipeListener
    {
        [SerializeField] private Transform _root;
        [SerializeField] private JewelView _jewelViewPrefab;
        [SerializeField] private Sprite[] _sprites;
        
        public void Awake()
        {
            SmartPipe.RegisterListener_AsFactory<JewelCreateAction>(this, CreateJewel);
        }

        private void CreateJewel(JewelCreateAction obj)
        {
            JewelView view = Instantiate(_jewelViewPrefab, Vector3.zero, Quaternion.identity, _root);
            obj.result = view;
            view.position = obj.at;
            view.transform.localPosition = Utils.IndexToPosition(obj.at);
            view.type = obj.type;
            view.spriteRenderer.sprite = _sprites[view.type];
            view.Construct();
            obj.Resolve();
            
            if (obj.moveUpper)
            {
                view.isTouchable = false;
                view.gameObject.SetActive(false);
                JewelStartFallFromTopAction.Process(obj.at);
            }
        }
    }
}