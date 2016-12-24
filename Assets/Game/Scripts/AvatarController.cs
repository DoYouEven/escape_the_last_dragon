using System;
using System.Collections;
using System.Collections.Generic;
using PK.InfiniteRunner.Debuging;
using UnityEngine;

namespace PK.InfiniteRunner.Game
{
    public enum Lanes
    {
        Left,
        Center,
        Right
    }

    public enum WorldDirection
    {
        North,
        South,
        East,
        West
    }

    public enum TurnDirection
    {
        Left,
        Right
    } 

    public class AvatarController : Singelton<AvatarController>
    {

        private float normalSpeed = 10f;
        private float speedModifier = 1f;

        private Rigidbody myRigidbody;
        private Transform myTransform;
        private Animator myAnimator;
        private CharacterController myController;

        private bool canMove;

        private Lanes curLane;
        private Lanes wantedLane;



        private float laneDistance = 1.5f;

        private float Speed
        {
            get { return normalSpeed * speedModifier; }
        }

        private DebugControl debugControl;

        private Vector3 direction;
        private float curAngle;
        private float rotationSpeed = 10f;

        public float CenterLanePosition;

        private float leftLanePosition
        {
            get { return CenterLanePosition - laneDistance; }
        }

        private float rightLanePosition
        {
            get { return CenterLanePosition + laneDistance; }
        }

        void Start()
        {
            myRigidbody = GetComponent<Rigidbody>();
            myTransform = transform;
            myAnimator = GetComponent<Animator>();
            curLane = Lanes.Center;
            wantedLane = Lanes.Center;


            CenterLanePosition = myTransform.position.x;

           
            debugControl = DebugControl.Instance;
            direction = new Vector3(Mathf.Sin(curAngle), 0, Mathf.Cos(curAngle));
            myController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            if (!canMove)
            {
                return;
            }
            
            
            if (wantedLane != curLane)
            {
                myController.SimpleMove(direction + (curLane < wantedLane ? myTransform.right : -myTransform.right) * Speed);
                myController.transform.forward = direction;
                if (IsOnWantedLine(wantedLane))
                {
                    curLane = wantedLane;
                    debugControl.ShowText(curLane.ToString());
                }
            }
            else
            {
                myController.transform.forward = direction;
                myController.SimpleMove(direction * Speed);
            }

        }

        public void Turn(TurnDirection turnDirection)
        {
           
            switch (turnDirection)
            {
                case TurnDirection.Left:
                    curAngle -= 90;
                    break;
                case TurnDirection.Right:
                    curAngle += 90;
                    break;
            }

            direction = new Vector3(Mathf.Round(Mathf.Sin(curAngle * Mathf.Deg2Rad)), 0, Mathf.Round(Mathf.Cos(curAngle * Mathf.Deg2Rad))); 
         
        }

        private bool IsOnWantedLine(Lanes lanes)
        {

            switch (lanes)
            {
                case Lanes.Left:

                    myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, leftLanePosition, CenterLanePosition), myTransform.position.y, myTransform.position.z);

                    if (Math.Abs(myTransform.position.x - leftLanePosition) < 0.1f)
                    {
                        return true;
                    }
                    break;
                case Lanes.Center:
                    if (curLane == Lanes.Left)
                    {
                        myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, leftLanePosition, CenterLanePosition), myTransform.position.y, myTransform.position.z);
                    }
                    else
                    {
                        myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, CenterLanePosition, rightLanePosition), myTransform.position.y, myTransform.position.z);
                    }

                    if (myTransform.position.x == CenterLanePosition)
                    {
                        return true;
                    }
                    break;
                case Lanes.Right:
                    myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, CenterLanePosition, rightLanePosition), myTransform.position.y, myTransform.position.z);
                    if (myTransform.position.x == rightLanePosition)
                    {
                        return true;
                    }
                    break;
            }
            return false;

        }
        public void SwitchLane(Lanes lanes)
        {
            switch (lanes)
            {
                case Lanes.Left:
                    if (curLane != Lanes.Left)
                    {
                        if (curLane == Lanes.Center)
                        {
                            wantedLane = Lanes.Left;
                        }
                        else
                        {
                            wantedLane = Lanes.Center;
                        }
                    }
                    break;

                case Lanes.Right:
                    if (curLane != Lanes.Right)
                    {
                        if (curLane == Lanes.Left)
                        {
                            wantedLane = Lanes.Center;
                        }
                        else
                        {
                            wantedLane = Lanes.Right;
                        }
                    }
                    break;
            }
        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                canMove = !canMove;
                if (!canMove)
                {
                    myRigidbody.velocity = Vector3.zero;
                }
                PlayMoveAnimation(canMove);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Turn(TurnDirection.Left);
            }
            if (Input.GetMouseButtonDown(2))
            {
                Turn(TurnDirection.Right);
            }
        }


        private void PlayMoveAnimation(bool play)
        {
            myAnimator.SetBool("Move", play);
        }





    }
}

