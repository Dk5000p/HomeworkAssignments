using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CommandPattern
{
    public class NPCBehaviorCommands : MonoBehaviour
    {
        //The box we control with keys
        public Transform enemy;
        public Rigidbody2D enemyRB;
        public Transform player;
        //The different keys we need
        private Command moveLeft, moveRight;
        //Stores all commands for replay and undo
        public static List<Command> patrolingBehaviorCommands = new List<Command>();
        //Box start position to know where replay begins
        private Vector3 boxStartPos;
        
        


        void Start()
        {
            //Bind keys with commands
            Command moveLeft = new moveLeft();
            Command moveRight = new moveRight();
            patrolingBehaviorCommands.Add(moveLeft);
            patrolingBehaviorCommands.Add(moveLeft);
            patrolingBehaviorCommands.Add(moveLeft);
            patrolingBehaviorCommands.Add(moveLeft);
            patrolingBehaviorCommands.Add(moveLeft);
            patrolingBehaviorCommands.Add(moveRight);
            patrolingBehaviorCommands.Add(moveRight);
            patrolingBehaviorCommands.Add(moveRight);
            patrolingBehaviorCommands.Add(moveRight);
            patrolingBehaviorCommands.Add(moveRight);


        }



        void Update()
        {
            foreach (Command i in patrolingBehaviorCommands)
            {

            }
        }


        //Check if we press a key, if so do what the key is binded to 
        public void HandleInput()
        {
            
        }


        //Checks if we should start the replay
       
    }
}

