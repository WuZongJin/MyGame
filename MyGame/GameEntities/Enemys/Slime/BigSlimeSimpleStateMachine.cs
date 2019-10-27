using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.FSM;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Slime
{
    public enum BigSlimeSimpleStates
    {
        Idle,
        Move,
        Dush,
        Reproducetion,
    }

    public class BigSlimeSimpleStateMachine:SimpleStateMachine<BigSlimeSimpleStates>
    {
        #region Properties
        BigSlime bigSlime;

        Vector2 direction = Vector2.Zero;

        float moveRate = 0.1f;
        float dushRate = 0.5f;
        float reProduce = 0.6f;

        float dushTime = 2.0f;
        float dushTimer = 0f;
        float moveTime = 2f;
        float moveTimer = 0f;
        float reProduceTime = 2f;
        float reProduceTimer = 0f;
        #endregion

        public BigSlimeSimpleStateMachine(BigSlime bigSlime)
        {
            this.bigSlime = bigSlime;
            initialState = BigSlimeSimpleStates.Idle;
        }

        #region Idle
        void Idle_Enter()
        {
            bigSlime.animation.currentAnimation = Slime.SlimeAnimation.Idle;
        }
        void Idle_Tick()
        {
            currentState = BigSlimeSimpleStates.Move;
        }

        void Idle_Exit()
        {

        }
        #endregion

        #region Move
        void Move_Enter()
        {
            bigSlime.animation.currentAnimation = Slime.SlimeAnimation.Move;
        }

        void Move_Tick()
        {
            moveTimer += Time.deltaTime;
            if(moveTimer > moveTime)
            {
                moveTimer = 0f;
                float randnum = Nez.Random.nextFloat();
                if (randnum < moveRate)
                {
                    currentState = BigSlimeSimpleStates.Move;
                }
                else if (randnum >= moveRate && randnum< reProduce)
                {
                    currentState = BigSlimeSimpleStates.Reproducetion;
                }
                else
                {
                    currentState = BigSlimeSimpleStates.Dush;
                }


            }
            direction = bigSlime.target.position - bigSlime.position;
            direction.Normalize();
            var movement = direction * bigSlime.moveSpeed * Time.deltaTime * 0.5f;
            FSCollisionResult fSCollisionResult;
            FixtureExt.collidesWithAnyFixtures(bigSlime.collider.GetFixture(), ref movement, out fSCollisionResult);
            bigSlime.position += movement;
        }

        void Move_Exit()
        {

        }
        #endregion

        #region Dush
        void Dush_Enter()
        {
            bigSlime.animation.currentAnimation = Slime.SlimeAnimation.Move;
            direction = bigSlime.target.position - bigSlime.position;
            direction.Normalize();
        }

        void Dush_Tick()
        {
            dushTimer += Time.deltaTime;
            if (dushTimer > dushTime)
            {
                dushTimer = 0f;
                currentState = BigSlimeSimpleStates.Idle;
            }
            var movement = direction * bigSlime.moveSpeed * Time.deltaTime * 2f;
            FSCollisionResult fSCollisionResult;
            FixtureExt.collidesWithAnyFixtures(bigSlime.collider.GetFixture(), ref movement, out fSCollisionResult);
            bigSlime.position += movement;
        }

        void Dush_Exit()
        {

        }
        #endregion

        #region ReProduce
        void Reproducetion_Enter()
        {
            
        }

        void Reproducetion_Tick()
        {
            reProduceTimer += Time.deltaTime;
            if(reProduceTimer> reProduceTime)
            {
                reProduceTimer = 0f;
                bigSlime.scene.addEntity(new Slime(bigSlime.position));
                currentState = BigSlimeSimpleStates.Idle;
            }


        }

        void Reproducetion_Exit()
        {

        }
        #endregion

    }
}
