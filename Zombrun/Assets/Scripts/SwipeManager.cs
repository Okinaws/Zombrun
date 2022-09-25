using System;
using UnityEngine;

public class SwipeManager : Singleton<SwipeManager>
{
    private bool touchMoved;
    [NonSerialized]
    public bool isPaused = false;
    private Vector2 swipeDelta;
    Vector2 TouchPosition() { return (Vector2)Input.mousePosition; }
    bool TouchBegan() { return Input.GetMouseButtonDown(0); }
    bool TouchEnded() { return Input.GetMouseButtonUp(0); }
    bool GetTouch() { return Input.GetMouseButton(0); }
    private Vector2 oldPos;


    void Update()
    {
        if (isPaused) return;
        if (TouchBegan())
        {
            oldPos = TouchPosition();
            touchMoved = true;
        }
        
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - oldPos;
            PlayerController.Instance.Move(swipeDelta.x / Screen.currentResolution.width);
            oldPos = TouchPosition();
        }

        if (TouchEnded())
        {
            oldPos = Vector2.zero;
            touchMoved = false;
            PlayerController.Instance.Move(0);
        }
    }
}
