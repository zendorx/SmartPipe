using System;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Match3.Scripts.UI
{
    public class GameWindow : MonoBehaviour, IPipeListener
    {
        [SerializeField] private Text _text;
        [SerializeField] private Button _buttonUnselect;

        [SerializeField] private Button _selectRandom;

        private int fieldWidth;
        private int fieldHeight;
        
        public void Start()
        {
            OLD_SmartPipe.RegisterListener<Match3_SelectJewel>(this, OnJewelSelected);
            OLD_SmartPipe.RegisterListener<Match3_UnSelectJewel>(this, OnUnSelectJewel);
            OLD_SmartPipe.RegisterListener<Match3_InitField>(this, OnInitField);
        }

        private void OnInitField(Match3_InitField obj)
        {
            fieldWidth = obj.x;
            fieldHeight = obj.y;
        }

        private void OnUnSelectJewel(Match3_UnSelectJewel obj)
        {
            _text.text = "NOT SELECTED";
            _buttonUnselect.interactable = false;
        }

        private void OnJewelSelected(Match3_SelectJewel obj)
        {
            _text.text = "SELECTED: " + obj.jewel.gameObject.name;
            _buttonUnselect.interactable = true;
        }

        public void UnSelectClicked()
        {
            Match3_UnSelectJewel.Emmit(null);
        }

        public void LagInput()
        {
            OLD_SmartPipe.actionsPerFrame = 1;
        }
    }
}