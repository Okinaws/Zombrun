using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : Singleton<SwipeManager>
{
    public enum Direction { Left, Right};
    private bool[] swipe = new bool[2];

    private Vector2 startTouch;
    private bool touchMoved;
    private Vector2 swipeDelta;

    const float SWIPE_THRESHOLD = 50;

    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;
    public delegate void ClickDelegate(Vector2 pos);
    public ClickDelegate ClickEvent;

    Vector2 TouchPosition() { return (Vector2)Input.mousePosition; }
    bool TouchBegan() { return Input.GetMouseButtonDown(0); }
    bool TouchEnded() { return Input.GetMouseButtonUp(0); }
    bool GetTouch() { return Input.GetMouseButton(0); }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // start and finish
        if (TouchBegan())
        {
            startTouch = TouchPosition();
            touchMoved = true;
        }
        else if (TouchEnded() && touchMoved == true)
        {
            SendSwipe();
            touchMoved = false;
        }

        // calc distance
        swipeDelta = Vector2.zero;
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - startTouch;
        }

        // check swipe
        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            swipe[(int)Direction.Left] = swipeDelta.x < 0;
            swipe[(int)Direction.Right] = swipeDelta.x > 0;

            SendSwipe();
        }
    }

    void SendSwipe()
    {
        if(swipe[0] || swipe[1])
        {
            MoveEvent?.Invoke(swipe);
        }
        else
        {
            ClickEvent?.Invoke(TouchPosition());
        }
        Reset();
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        touchMoved = false;
        for (int i = 0; i < swipe.Length; i++)
        {
            swipe[i] = false;
        }
    }
}
