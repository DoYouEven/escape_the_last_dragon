using System.Collections;
using System.Collections.Generic;
using InfiniteRunner.Player;
using UnityEngine;

namespace PK.InfiniteRunner.Game
{
    public class AutoTurnTriggrer : MonoBehaviour
    {
        public TurnDirection Direction;

        private string playerTag = "Player";

        private AvatarController controller;


        // Use this for initialization
        void Start()
        {
            controller = AvatarController.Instance;
        }


        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                controller.Turn(Direction);
            }
        }

        private void OnTriggerExit(Collider other)
        {

        }


    }
}

