using UnityEngine;

namespace PK.InfinuteRunner.Game
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

        //Swipe Variables
        private Touch touch;
        private Vector2 touchStart;
        private Vector2 swipeDistance;

        //Tilt Variables
        private TiltDirection curTiltDirection;
        private TiltDirection lasTiltDirection;
        private float tiltDir;
        private float minTilting = 0.2f;

        //Debuging
        private InputUIDebug uiDebug;


        

        
        
        void Start()
        {
           uiDebug = InputUIDebug.Instance;
            lasTiltDirection = TiltDirection.None;
        }

        
        void Update()
        { 
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
            if (lasTiltDirection != curTiltDirection )
            {
                lasTiltDirection = curTiltDirection;
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
                        if (Mathf.Abs(swipeDistance.y) > minSwipeDistanceY)
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
            uiDebug.ShowText("Swipe Up");
        }
        private void OnSwipeDown()
        {
            uiDebug.ShowText("Swipe Down");
        }

        private void OnSingleTap()
        {
            uiDebug.ShowText("Single Tap");
        }
        private void ShowCurTiltDirection()
        {
             uiDebug.ShowTiltText(curTiltDirection.ToString());
        }

    }
}

