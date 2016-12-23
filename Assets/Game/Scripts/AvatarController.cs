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



    public class AvatarController : Singelton<AvatarController>
    {

        private float normalSpeed = 10f;
        private float speedModifier = 1f;

        private Rigidbody myRigidbody;
        private Transform myTransform;
        private Animator myAnimator;

        private bool canMove;

        private Lanes curLane;
        private Lanes wantedLane;

        private Vector3 lanesPositions;

        private float laneDistance = 1.5f;

        private float Speed
        {
            get { return normalSpeed * speedModifier; }
        }

        private DebugControl debugControl;

        void Start()
        {
            myRigidbody = GetComponent<Rigidbody>();
            myTransform = transform;
            myAnimator = GetComponent<Animator>();
            curLane = Lanes.Center;
            wantedLane = Lanes.Center;

            lanesPositions = new Vector3(myTransform.position.x - laneDistance, myTransform.position.x, myTransform.position.x + laneDistance);
            debugControl = DebugControl.Instance;
        }

        private void FixedUpdate()
        {
            if (!canMove)
            {
                return;
            }
            myRigidbody.velocity = myTransform.forward * Speed;

            if (wantedLane != curLane)
            {
                myRigidbody.velocity += (curLane < wantedLane ? myTransform.right : -myTransform.right) * Speed;
                if (IsOnWantedLine(wantedLane))
                {
                    curLane = wantedLane;
                    debugControl.ShowText(curLane.ToString());
                }
            }
        }

        private void Turn(float angle)
        {
            
        }

        private bool IsOnWantedLine(Lanes wantedLane)
        {
            //switch ((int)myTransform.rotation.y)  
            //{
            //    case 0:

             //North
            switch (wantedLane)
            {
                case Lanes.Left:  

                    myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, lanesPositions.x, lanesPositions.y), myTransform.position.y, myTransform.position.z);   
                    
                    if (myTransform.position.x == lanesPositions.x)
                    {
                        return true;
                    }
                    break;
                case Lanes.Center:
                    if (curLane == Lanes.Left)
                    {
                        myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, lanesPositions.x, lanesPositions.y), myTransform.position.y, myTransform.position.z);  
                    }
                    else
                    {
                        myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, lanesPositions.y, lanesPositions.z), myTransform.position.y, myTransform.position.z);
                    }

                    if (myTransform.position.x == lanesPositions.y)
                    {
                        return true;
                    }
                    break;
                case Lanes.Right:  
                    myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, lanesPositions.y, lanesPositions.z), myTransform.position.y, myTransform.position.z); 
                    if (myTransform.position.x == lanesPositions.z)
                    {
                        return true;
                    }
                    break;
            }


            //        break;
            //}
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
            if (Input.GetMouseButtonDown(0)) 
            {
                  Turn(90);
            }
        }


        private void PlayMoveAnimation(bool play)
        {
            myAnimator.SetBool("Move", play);
        }





    }
}

