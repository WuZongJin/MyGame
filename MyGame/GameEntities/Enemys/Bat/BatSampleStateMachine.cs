using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Bat
{
    public enum BatStates
    {
        Idle,
        FindEnemy,
        Attack,
        Patrol,
    }
    public class BatSampleStateMachine:SimpleStateMachine<BatStates>
    {
        Bat bat;
        Vector2 attackDirection = Vector2.Zero;

        public BatSampleStateMachine(Bat bat)
        {
            this.bat = bat;
        }

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            initialState = BatStates.Idle;
        }

        void Idle_Enter()
        {
            bat.animation.currentAnimation = Bat.BatAnimations.Idle;
        }
        void Idle_Tick()
        {
            if (bat.isFindTarget)
            {
                currentState = BatStates.FindEnemy;
            }
        }
        void Idle_Exit() { }

        #region FindEnemy
        void FindEnemy_Enter()
        {
            bat.animation.currentAnimation = Bat.BatAnimations.Fly;
        }
        void FindEnemy_Tick()
        {
            int distance = (int)Vector2.Distance(bat.position, bat.target.position);
            if (distance != 100)
            {
                Vector2 direction = Vector2.Zero;
                if (distance < 100)
                {
                    direction = bat.position - bat.target.position;
                }
                else
                {
                    direction = bat.target.position - bat.position;
                }
                
                direction.Normalize();
                var movement = direction * bat.actorProperty.moveSpeed * Time.deltaTime;
                bat.position += movement;
            }
            else
            {
                currentState = BatStates.Patrol;
            }
        }
        void FindEnemy_Exit() { }
        #endregion

        #region Attack
        void Attack_Enter()
        {
            bat.animation.currentAnimation = Bat.BatAnimations.Fly;
            attackDirection = bat.target.position - bat.position;
            attackDirection.Normalize();
        }

        void Attack_Tick()
        {
            bat.attackTimer += Time.deltaTime;
            if(bat.attackTimer > bat.attackTime)
            {
                bat.attackTimer = 0f;
                currentState = BatStates.FindEnemy;
            }
            
           
            var movement = attackDirection * bat.actorProperty.moveSpeed* 4f * Time.deltaTime;
            bat.position += movement;

        }

        void Attack_Exit()
        {
            
        }
        #endregion

        #region Patrol
        void Patrol_Enter()
        {
            bat.animation.currentAnimation = Bat.BatAnimations.Fly;
        }

        void Patrol_Tick()
        {
            bat.patrolTimer += Time.deltaTime;
            if(bat.patrolTimer > bat.patrolTime)
            {
                bat.patrolTimer = 0f;
                currentState = BatStates.Attack;
            }
            var direction = bat.target.position - bat.position;
            direction.Normalize();
            direction = new Vector2(-direction.Y, direction.X);
            var movement = direction * bat.actorProperty.moveSpeed * 0.7f * Time.deltaTime;
            bat.position += movement;

        }

        void Patrol_Exit()
        {

        }

        #endregion

    }
}
