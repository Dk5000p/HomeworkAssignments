using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StatePattern
{
    public class Slime : Enemy
{
        EnemyFSM slimeMode = EnemyFSM.Stroll;

        float health = 100f;


        public Slime(Transform slimeObj)
        {
            base.enemyObj = slimeObj;
        }


        //Update the slime's state
        public override void UpdateEnemy(Transform playerObj)
        {
            //The distance between the Slime and the player
            float distance = (base.enemyObj.position - playerObj.position).magnitude;

            switch (slimeMode)
            {
                case EnemyFSM.Attack:
                    if (health < 20f)
                    {
                        slimeMode = EnemyFSM.Flee;
                    }
                    else if (distance > 2f)
                    {
                        slimeMode = EnemyFSM.MoveTowardsPlayer;
                    }
                    break;
                case EnemyFSM.Flee:
                    if (health > 60f)
                    {
                        slimeMode = EnemyFSM.Stroll;
                    }
                    break;
                case EnemyFSM.Stroll:
                    if (distance < 10f)
                    {
                        slimeMode = EnemyFSM.MoveTowardsPlayer;
                    }
                    break;
                case EnemyFSM.MoveTowardsPlayer:
                    if (distance < 1f)
                    {
                        slimeMode = EnemyFSM.Attack;
                    }
                    else if (distance > 15f)
                    {
                        slimeMode = EnemyFSM.Stroll;
                    }
                    break;
            }

            //Move the enemy based on a state
            DoAction(playerObj, slimeMode);
        }
    }
}
