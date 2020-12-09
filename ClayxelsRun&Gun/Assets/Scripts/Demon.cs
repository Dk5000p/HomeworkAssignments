using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StatePattern
{
    public class Demon : Enemy
{
        EnemyFSM demonMode = EnemyFSM.Stroll;

        public float health = 100f;

        private void start()
        {
            InvokeRepeating("hurt", 2.0f, 10.0f);
        }
        void hurt()
        {
            health -= 10;
        }
        public Demon(Transform skeletonObj)
        {
            base.enemyObj = skeletonObj;
        }
        

        //Update the demon's state
        public override void UpdateEnemy(Transform playerObj)
        {
            //The distance between the demon and the player
            float distance = (base.enemyObj.position - playerObj.position).magnitude;

            switch (demonMode)
            {
                case EnemyFSM.Attack:
                    if (health < 20f)
                    {
                        demonMode = EnemyFSM.Flee;
                    }
                    else if (distance > 6f)
                    {
                        demonMode = EnemyFSM.MoveTowardsPlayer;
                    }
                    break;
                case EnemyFSM.Flee:
                    if (health > 60f)
                    {
                        demonMode = EnemyFSM.Stroll;
                    }
                    break;
                case EnemyFSM.Stroll:
                    if (distance < 10f)
                    {
                        demonMode = EnemyFSM.MoveTowardsPlayer;
                    }
                    break;
                case EnemyFSM.MoveTowardsPlayer:
                    //The demon has fiery rage so it can attack from distance
                    if (distance < 5f)
                    {
                        demonMode = EnemyFSM.Attack;
                    }
                    else if (distance > 15f)
                    {
                        demonMode = EnemyFSM.Stroll;
                    }
                    break;
            }

            //Move the enemy based on a state
            DoAction(playerObj, demonMode);
        }
    }

}
