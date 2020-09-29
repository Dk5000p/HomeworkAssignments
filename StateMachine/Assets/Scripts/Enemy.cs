using UnityEngine;
using System.Collections;


namespace StatePattern
{
    //The enemy base class
    public class Enemy:MonoBehaviour
    {
        protected Transform enemyObj;
        public Animator anim;

        //The different states the enemy can be in
        protected enum EnemyFSM
        {
            Attack,
            Flee,
            Stroll,
            MoveTowardsPlayer
        }


        //Update the enemy by giving it a new state
        public virtual void UpdateEnemy(Transform playerObj)
        {

        }


        //Do something based on a state
        protected void DoAction(Transform playerObj, EnemyFSM enemyMode)
        {
            float fleeSpeed = 10f;
            float strollSpeed = 1f;
            float attackSpeed = 5f;
            float speed = 7f;

            switch (enemyMode)
            {
                case EnemyFSM.Attack:
                    //Attack player
                  
                    break;
                case EnemyFSM.Flee:
                    
                    
                    //Move
                    enemyObj.Translate(enemyObj.up * fleeSpeed * Time.deltaTime);
                    break;
                case EnemyFSM.Stroll:
                    Vector3 randomPos = new Vector3(Random.Range(0f, 100f),  Random.Range(0f, 100f), 0f);
                    enemyObj.position = Vector2.MoveTowards(enemyObj.position, randomPos, strollSpeed*Time.deltaTime);


                    break;
                case EnemyFSM.MoveTowardsPlayer:
                    float step = speed * Time.deltaTime;
                    enemyObj.position = Vector2.MoveTowards(enemyObj.position, playerObj.position, step);
                    break;
            }
        }
    }
}
