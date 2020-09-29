using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StatePattern
{
    public class GameController : MonoBehaviour
    {
        public GameObject playerObj;
        public GameObject slimeObj;
        public GameObject skeletonObj;

        //A list that will hold all enemies
        List<Enemy> enemies = new List<Enemy>();


        void Start()
        {
            //Add the enemies we have
            enemies.Add(new Slime(slimeObj.transform));
            enemies.Add(new Skeleton(skeletonObj.transform));
        }


        void Update()
        {
            //Update all enemies to see if they should change state and move/attack player
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].UpdateEnemy(playerObj.transform);
            }
        }
    }
}
