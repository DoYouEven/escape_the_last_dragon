using UnityEngine;
using PK.InfiniteRunner.Debuging;

namespace PK.InfiniteRunner.Game
{
    public enum TiltDirection
    {
        None,
        Left,
        Right
    }
    public class PlayerInput : MonoBehaviour
    {
        //Minimal distance for swipes to be valid
        private float minSwipeDistanceY = 50f;
        private float minSwipeDistanceX = 50f;
        private float degree = 60f;
        //Swipe Variables
        private Touch touch;
        private Vector2 touchStart;
        private Vector2 swipeDistance;
        //Tilt Variables
        private TiltDirection curTiltDirection;
        private TiltDirection lastTiltDirection;
        private float tiltDir;
        private float minTilting = 0.2f;
        //Debuging
        private DebugControl uiDebug;

        void Start()
        {
            uiDebug = DebugControl.Instance;
            lastTiltDirection = TiltDirection.None;
        }
        void Update()
        {
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
            tiltDir = Input.acceleration.x;
            if (tiltDir > minTilting)
            {
                curTiltDirection = TiltDirection.Right;
            }
            else if (tiltDir < -minTilting)
            {
                curTiltDirection = TiltDirection.Left;
            }
            else
            {
                curTiltDirection = TiltDirection.None;
            }
            //For Debuging 
            if (lastTiltDirection != curTiltDirection)
            {
                lastTiltDirection = curTiltDirection;
                ShowCurTiltDirection();
            }
            //Only if there is one touch
            if (Input.touchCount == 1)
            {
                //Get that first Touch 
                touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        //Get Starting Position
                        touchStart = touch.position;
                        break;

                    case TouchPhase.Ended:
                        //Get Ending Position
                        swipeDistance = touchStart - touch.position;
                        //Check if swipe is vertical
                        if (Mathf.Abs(swipeDistance.y) > minSwipeDistanceY && Mathf.Abs(swipeDistance.y / swipeDistance.x) > Mathf.Tan(degree))
                        {
                            if (swipeDistance.y < 0)
                            {
                                OnSwipeUp();
                            }
                            else
                            {
                                OnSwipeDown();
                            }
                        }
                        //Check if swipe is horizontal
                        else if (Mathf.Abs(swipeDistance.x) > minSwipeDistanceX)
                        {
                            if (swipeDistance.x < 0)
                            {
                                OnSwipeRight();
                            }
                            else
                            {
                                OnSwipeLeft();
                            }
                        }
                        //If it wasn't vertical or horizontal then it was a single tap
                        else
                        {
                            OnSingleTap();
                        }
                        break;
                }
            }  
#else
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnSwipeUp();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnSwipeDown();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnSwipeLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnSwipeRight();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSingleTap();
            }
#endif
        }
        private void OnSwipeLeft()
        {
            uiDebug.ShowText("Swipe Left");
        }
        private void OnSwipeRight()
        {
            uiDebug.ShowText("Swipe Right");
        }
        private void OnSwipeUp()
        {
            uiDebug.ShowText("Swipe Up", DebugType.Warning);
        }
        private void OnSwipeDown()
        {
            uiDebug.ShowText("Swipe Down", DebugType.Error);
        }
        private void OnSingleTap()
        {
            uiDebug.ShowText("Single Tap");
        }
        private void ShowCurTiltDirection()
        {
            uiDebug.ShowText(curTiltDirection.ToString(), DebugType.Warning);
        }
    }
}

