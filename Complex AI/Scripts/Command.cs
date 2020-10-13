using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public abstract class Command
{
    //How far should the box move when we press a button
    protected float moveDistance = 1f;
    public abstract void Execute(Transform boxTrans, Command command);
    public abstract void Execute(Transform boxTrans, Rigidbody2D rb, Command command);

        //Undo an old command
        public virtual void Undo(Transform boxTrans) { }

    //Move the box
    public virtual void Move(Transform boxTrans) { }
}
public class moveLeft : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            boxTrans.Translate(new Vector2(-1, 0));
        }
        public override void Execute(Transform boxTrans, Rigidbody2D rb, Command command)
        {
            
        }
    }
public class moveRight : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            boxTrans.Translate(new Vector2(1, 0));
        }
        public override void Execute(Transform boxTrans, Rigidbody2D rb, Command command)
        {

        }
    }
    public class idle : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            //doNothing;
        }
        public override void Execute(Transform boxTrans, Rigidbody2D rb, Command command)
        {

        }

    }
    public class Jump : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            
        }
        public override void Execute(Transform boxTrans,Rigidbody2D rb, Command command)
        {
            rb.AddForce(new Vector2(0f, 100f));
        }
    }




}
