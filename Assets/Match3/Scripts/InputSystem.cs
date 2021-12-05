using System;
using UnityEngine;

namespace Match3.Scripts
{
    public class InputSystem : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                var worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
                //Debug.Log("CLICK : "+ worldPosition);
                //Debug.Log("CLICK AT: "+ Jewel.PositionToIndex(worldPosition));
                
                Input_Click.Emmit(worldPosition);
            }
        }
    }
}