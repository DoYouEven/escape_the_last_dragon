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
        private PlayerInput myPlayerInput;

        private bool canMove;

        private Lanes curLane;
        private Lanes wantedLane;
        public WorldDirection lastDirection;
        public WorldDirection curDirection;


        private float laneDistance = 1.5f;

        private float Speed
        {
            get { return normalSpeed * speedModifier; }
        }

        private DebugControl debugControl;

        private Vector3 direction;
        private float curAngle;
        //private float rotationSpeed = 10f;   
        public float CenterLanePosition; 
        private float leftLanePosition;  
        private float rightLanePosition;

        private Vector3 curTurningPos;
        private void SetLinePositions()
        {
            switch (curDirection)
            {
                case WorldDirection.North:
                    leftLanePosition = CenterLanePosition - laneDistance;
                    rightLanePosition = CenterLanePosition + laneDistance;
                    break;
                case WorldDirection.South:
                    leftLanePosition = CenterLanePosition + laneDistance;
                    rightLanePosition = CenterLanePosition - laneDistance;
                    break;
                case WorldDirection.East:
                    leftLanePosition = CenterLanePosition + laneDistance;
                    rightLanePosition = CenterLanePosition - laneDistance;
                    break;
                case WorldDirection.West:
                    leftLanePosition = CenterLanePosition - laneDistance;
                    rightLanePosition = CenterLanePosition + laneDistance;
                    break;
            }
        }


        protected void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
            myTransform = transform;
            myAnimator = GetComponent<Animator>();
            curLane = Lanes.Center;
            wantedLane = Lanes.Center;
            curDirection = WorldDirection.North;
            CenterLanePosition = myTransform.position.x;
            SetLinePositions();
            debugControl = DebugControl.Instance;
            direction = new Vector3(Mathf.Sin(curAngle), 0, Mathf.Cos(curAngle));
            myController = GetComponent<CharacterController>();
            myPlayerInput = GetComponent<PlayerInput>();
        }

        protected void OnEnable()
        {
            myPlayerInput.OnSwipeRightDelegate += OnSwipeRight;
            myPlayerInput.OnSwipeLeftDelegate += OnSwipeLeft;
        }

        protected void OnDisable()
        {
            myPlayerInput.OnSwipeRightDelegate -= OnSwipeRight;
            myPlayerInput.OnSwipeLeftDelegate -= OnSwipeLeft;
        }

        private void OnSwipeRight()
        {
            if (canTurn(false))
            {
                Turn(TurnDirection.Right, curTurningPos);
                debugControl.ShowText("Can Turn Right");
                return;
            }
            SwitchLane(Lanes.Right);
        }
        private void OnSwipeLeft()
        {
            if (canTurn())
            {
                Turn(TurnDirection.Left, curTurningPos);
                debugControl.ShowText("Can Turn Left");
                
                return;
            }
            SwitchLane(Lanes.Left);
        }

        private bool canTurn(bool left = true)
        {
            RaycastHit hit;
            if (Physics.Raycast(myTransform.position, Vector3.down, out hit))
            {
                var turnText = hit.transform.GetComponent<TurningTest>();
                if (turnText != null)
                {
                    curTurningPos = hit.transform.position;
                    return left ? turnText.CanTurnLeft : turnText.CanTurnRight;
                }
            }
            return false;
        }


        protected void FixedUpdate()
        {
            if (!canMove)
            {
                return;
            }
            if (wantedLane != curLane)
            {
                myController.transform.forward = direction;
                myController.SimpleMove(direction + (curLane < wantedLane ? myTransform.right : -myTransform.right) * Speed);
                if (!IsOnWantedLine(wantedLane)) return;
                curLane = wantedLane;
                debugControl.ShowText(curLane.ToString());
            }
            else
            {
                myController.transform.forward = direction;
                myController.SimpleMove(direction * Speed);
            }
        }

        public void Turn(TurnDirection turnDirection, Vector3 pos)
        {
            lastDirection = curDirection;

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
            //East
            if (direction.x > 0)
            {
                curDirection = WorldDirection.East;
            }
            //west
            else if (direction.x < 0)
            {
                curDirection = WorldDirection.West;
            }
            //North
            if (direction.z > 0)
            {
                curDirection = WorldDirection.North;
            }
            //South
            else if (direction.z < 0)
            {
                curDirection = WorldDirection.South;
            }
            SetCenterLanePosition(pos);
            if (turnDirection == TurnDirection.Left)
            {
                ReturnToMyLaneL();
            }
            else
            {
                ReturnToMyLaneR();
            }
        }

        private void ReturnToMyLaneL()
        {
            curLane = wantedLane == Lanes.Left ? Lanes.Center : Lanes.Left;
        }
        private void ReturnToMyLaneR()
        {
            curLane = wantedLane == Lanes.Right ? Lanes.Center : Lanes.Right;
        }
        public void SetCenterLanePosition(Vector3 pos)
        {
            if (curDirection == WorldDirection.North || curDirection == WorldDirection.South)
            {
                CenterLanePosition = pos.x;
            }
            else
            {
                CenterLanePosition = pos.z;
            }
            SetLinePositions();
        }

        private bool IsOnWantedLine(Lanes lanes)
        {
            switch (lanes)
            {
                case Lanes.Left:
                    switch (curDirection)
                    {
                        case WorldDirection.North:
                            myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, leftLanePosition, CenterLanePosition), myTransform.position.y, myTransform.position.z);
                            if (Math.Abs(myTransform.position.x - leftLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                        case WorldDirection.South:
                            myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, CenterLanePosition, leftLanePosition), myTransform.position.y, myTransform.position.z);

                            if (Math.Abs(myTransform.position.x - leftLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                        case WorldDirection.East:
                            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, Mathf.Clamp(myTransform.position.z, CenterLanePosition, leftLanePosition));

                            if (Math.Abs(myTransform.position.z - leftLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                        case WorldDirection.West:
                            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, Mathf.Clamp(myTransform.position.z, leftLanePosition, CenterLanePosition));

                            if (Math.Abs(myTransform.position.z - leftLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                    }
                    break;
                case Lanes.Center:
                    switch (curDirection)
                    {
                        case WorldDirection.North:
                            if (curLane == Lanes.Left)
                            {
                                myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, leftLanePosition, CenterLanePosition), myTransform.position.y, myTransform.position.z);
                            }
                            else
                            {
                                myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, CenterLanePosition, rightLanePosition), myTransform.position.y, myTransform.position.z);
                            }

                            if (Math.Abs(myTransform.position.x - CenterLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                        case WorldDirection.South:
                            if (curLane == Lanes.Left)
                            {
                                myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, CenterLanePosition, leftLanePosition), myTransform.position.y, myTransform.position.z);
                            }
                            else
                            {
                                myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, rightLanePosition, CenterLanePosition), myTransform.position.y, myTransform.position.z);
                            }

                            if (Math.Abs(myTransform.position.x - CenterLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                        case WorldDirection.East:
                            if (curLane == Lanes.Left)
                            {
                                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, Mathf.Clamp(myTransform.position.z, CenterLanePosition, leftLanePosition));
                            }
                            else
                            {
                                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, Mathf.Clamp(myTransform.position.z, rightLanePosition, CenterLanePosition));
                            }

                            if (Math.Abs(myTransform.position.z - CenterLanePosition) < 0.1f)
                            {
                                return true;
                            }

                            break;
                        case WorldDirection.West:
                            if (curLane == Lanes.Left)
                            {
                                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, Mathf.Clamp(myTransform.position.z, leftLanePosition, CenterLanePosition));
                            }
                            else
                            {
                                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, Mathf.Clamp(myTransform.position.z, CenterLanePosition, rightLanePosition));
                            }

                            if (Math.Abs(myTransform.position.z - CenterLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                    }
                    break;
                case Lanes.Right:

                    switch (curDirection)
                    {
                        case WorldDirection.North:
                            myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, CenterLanePosition, rightLanePosition), myTransform.position.y, myTransform.position.z);
                            if (Math.Abs(myTransform.position.x - rightLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                        case WorldDirection.South:
                            myTransform.position = new Vector3(Mathf.Clamp(myTransform.position.x, rightLanePosition, CenterLanePosition), myTransform.position.y, myTransform.position.z);
                            if (Math.Abs(myTransform.position.x - rightLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                        case WorldDirection.East:
                            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, Mathf.Clamp(myTransform.position.z, rightLanePosition, CenterLanePosition));
                            if (Math.Abs(myTransform.position.z - rightLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
                        case WorldDirection.West:
                            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, Mathf.Clamp(myTransform.position.z, CenterLanePosition, rightLanePosition));
                            if (Math.Abs(myTransform.position.z - rightLanePosition) < 0.1f)
                            {
                                return true;
                            }
                            break;
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
        protected void Update()
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
            //if (Input.GetMouseButtonDown(1))
            //{
            //    Turn(TurnDirection.Left);
            //}
            //if (Input.GetMouseButtonDown(2))
            //{
            //    Turn(TurnDirection.Right);
            //}
        }
        private void PlayMoveAnimation(bool play)
        {
            myAnimator.SetBool("Move", play);
        }
    }
}

