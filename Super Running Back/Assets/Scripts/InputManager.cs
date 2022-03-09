using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Touch> onPressEvents;
    public UnityEvent<float> onKeyboardEvents;
    private void Update()
    {
        horizontalMove();
    }

    private void horizontalMove()
    {
        if (LongPress())
        {
            onPressEvents.Invoke(Input.GetTouch(0));
        }
    }

    private bool LongPress()
    {
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].tapCount == 1)
            {
                return true;
            }
        }
        return false;
    }

    private void keyboardMove()
    {
        var h = Input.GetAxis("Horizontal");

        onKeyboardEvents.Invoke(h);
    }
}
