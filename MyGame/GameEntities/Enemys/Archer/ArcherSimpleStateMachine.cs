using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Archer
{
    public enum ArcherStates
    {
        Idle,
        Move,
        Attack,
        SuperAttack,
    }

    public class ArcherSimpleStateMachine:SimpleStateMachine<ArcherStates>
    {
        #region PRoperties
        Archer archer;

        Vector2 direction = Vector2.Zero;

        float moveTime = 1f;
        float moveTimer = 0f;
        float attackTime = 2f;
        float attackTimer = 0f;
        float superAttackTime = 2f;
        float superAttackTimer = 0f;
        #endregion

        #region Contsrtuctor
        public ArcherSimpleStateMachine(Archer archer)
        {
            this.archer = archer;
            initialState = ArcherStates.Idle;
        }
        #endregion

        #region Idle
        void Idle_Enter()
        {

        }
        void Idle_Tick()
        {
            if (archer.isFindTarget)
            {
                currentState = ArcherStates.Attack;
            }
        }

        void Idle_Exit()
        {

        }
        #endregion


        #region Move
        void Move_Enter()
        {

        }

        void Move_Tick()
        {
            moveTimer += Time.deltaTime;
            if(moveTimer > moveTime)
            {
                
            }
        }

        void Move_Exit()
        {

        }
        #endregion

        #region Attack
        void Attack_Enter()
        {

        }

        void Attack_Tick()
        {

        }

        void Attack_Exit()
        {

        }

        #endregion

        #region SuperAttack
        void SuperAttack_Enter()
        {

        }

        void SuperAttack_Tick()
        {

        }

        void SuperAttack_Exit()
        {

        }
        #endregion
    }
}
