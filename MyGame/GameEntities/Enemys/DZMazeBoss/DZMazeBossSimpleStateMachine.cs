using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameEntities.Utils;
using Nez;
using Nez.AI.FSM;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.DZMazeBoss
{
    public enum DZMazeBossStates
    {
        Idle,
        Move,
        FireBallAttack,
        FallFireBallAttack,
        FSuperFireBallAttack,
        Recover,
        SecondLeve,
    }

    public class DZMazeBossSimpleStateMachine:SimpleStateMachine<DZMazeBossStates>
    {
        #region Properties
        DZMazeBoss boss;
        Vector2[] attackPosition;
        Vector2 fallAttackPosition;
        Vector2 dirPosition;

        float fireBallSpeed = 120f;

        bool startFallinAttack = false;

        float moveTimer = 0f;
        float moveTime = 3f;

        float fireAttackTime = 3f;
        float fireAttackTimer = 0f;

        float attackFrequence = 0.3f;


        float superAttackTime = 3f;
        float superAttackTimer = 0f;

        float fallAttackTime = 5f;
        float fallAttackTimer = 0f;

        float recoverTime = 2f;
        float recoverTimer = 0f;
        #endregion

        public DZMazeBossSimpleStateMachine(DZMazeBoss boss)
        {
            this.boss = boss;
            initialState = DZMazeBossStates.Idle;
            attackPosition = new Vector2[]
            {
                new Vector2(42,83),
                new Vector2(257,83),
                new Vector2(42,254),
                new Vector2(275,257)
            };

            fallAttackPosition = new Vector2(160, 160);
        }

        #region Idle
        void Idle_Enter()
        {

        }

        void Idle_Tick()
        {
            if (boss.startMove)
            {
                currentState = DZMazeBossStates.Move;
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
            if(moveTimer> moveTime)
            {
                moveTimer = 0f;
                var rand = Nez.Random.nextFloat();
                if (rand < 0.2)
                {
                    currentState = DZMazeBossStates.FSuperFireBallAttack;
                }
                else if(rand>=0.2 && rand < 0.6)
                {
                    currentState = DZMazeBossStates.FireBallAttack;
                }
                else if (rand >= 0.8)
                {
                    currentState = DZMazeBossStates.FallFireBallAttack;
                }
            }

            int distance = (int)Vector2.Distance(boss.position, boss.target.position);
            if (distance <70 || distance > 120)
            {
                Vector2 direction = Vector2.Zero;
                if (distance < 70)
                {
                    direction = boss.position - boss.target.position;
                }
                else
                {
                    direction = boss.target.position - boss.position;
                }

                direction = new Vector2(direction.X-direction.Y, direction.Y+direction.X);
                if (direction.Length() > 1)
                {
                    direction.Normalize();
                }
                   
                var movement = direction * boss.actorProperty.moveSpeed * Time.deltaTime;
                boss.position += movement;
            }
            else
            {
                var direction = boss.target.position - boss.position;
                if (direction.Length() > 1)
                {
                    direction.Normalize();
                }
                direction = new Vector2(-direction.Y, direction.X);
                var movement = direction * boss.actorProperty.moveSpeed * Time.deltaTime;
                boss.position += movement;
            }
        }

        void Move_Exit()
        {

        }
        #endregion

        #region FireBallAttack
        void FireBallAttack_Enter()
        {
            var rand = Nez.Random.nextInt(4);
            dirPosition = attackPosition[rand];
        }
        void FireBallAttack_Tick()
        {
            fireAttackTimer += Time.deltaTime;
            if(fireAttackTimer > fireAttackTime&& (int)boss.position.X == (int)dirPosition.X && (int)boss.position.Y == (int)dirPosition.Y)
            {

                var dir = boss.target.position - boss.position;
                dir.Normalize();
                var degress = (float)Math.Atan2(dir.Y, dir.X) + (float)Math.Atan2(1, 0);
                createfireBall(boss.position).setRotation(degress).addComponent(new AutoMoveComponent(degress, fireBallSpeed));

                currentState = DZMazeBossStates.Move;
            }

            Vector2 moveDir = Vector2.Zero;
            if((int)boss.position.X> (int)dirPosition.X)
            {
                moveDir -= new Vector2(1, 0);
            }
            else if((int)boss.position.X < (int)dirPosition.X)
            {
                moveDir += new Vector2(1, 0);
            }
            if((int)boss.position.Y > (int)dirPosition.Y)
            {
                moveDir -= new Vector2(0, 1);
            }
            else if ((int)boss.position.Y < (int)dirPosition.Y)
            {
                moveDir += new Vector2(0, 1);
            }

            boss.position += moveDir * boss.moveSpeed * 1f * Time.deltaTime;


        }

        void FireBallAttack_Exit()
        {

        }

        #endregion

        #region  FallFireBallAttack
        void FallFireBallAttack_Enter()
        {
            startFallinAttack = false;
            attackFrequence = 0.5f;
        }
        void FallFireBallAttack_Tick()
        {
            if(startFallinAttack)
            {
                fallAttackTimer += Time.deltaTime;
                if(fallAttackTimer> attackFrequence)
                {
                    attackFrequence += attackFrequence;
                    Core.scene.addEntity(new FallinFireAttack()).setPosition(boss.target.position);
                }
            }



            if(fallAttackTimer > fallAttackTime)
            {
                fallAttackTimer = 0;
                currentState = DZMazeBossStates.Move;
            }

            if (!startFallinAttack)
            {
                Vector2 moveDir = Vector2.Zero;
                if ((int)boss.position.X > (int)fallAttackPosition.X)
                {
                    moveDir -= new Vector2(1, 0);
                }
                else if ((int)boss.position.X < (int)fallAttackPosition.X)
                {
                    moveDir += new Vector2(1, 0);
                }
                if ((int)boss.position.Y > (int)fallAttackPosition.Y)
                {
                    moveDir -= new Vector2(0, 1);
                }
                else if ((int)boss.position.Y < (int)fallAttackPosition.Y)
                {
                    moveDir += new Vector2(0, 1);
                }

                boss.position += moveDir * boss.moveSpeed * 2f * Time.deltaTime;
                if ((int)boss.position.X == (int)fallAttackPosition.X && (int)boss.position.Y == (int)fallAttackPosition.Y)
                {
                    startFallinAttack = true;
                }
            }
         }

        void FallFireBallAttack_Exit()
        {

        }
        #endregion

        #region  FSuperFireBallAttack
        void FSuperFireBallAttack_Enter()
        {
            var rand = Nez.Random.nextInt(4);
            dirPosition = attackPosition[rand];
        }

        void FSuperFireBallAttack_Tick()
        {
            superAttackTimer += Time.deltaTime;
            if(superAttackTimer > superAttackTime&& (int)boss.position.X == (int)dirPosition.X && (int)boss.position.Y == (int)dirPosition.Y)
            {
                

                var dir = boss.target.position - boss.position;
                dir.Normalize();
                var degress = (float)Math.Atan2(dir.Y, dir.X) + (float)Math.Atan2(1, 0);
                createfireBall(boss.position).setRotation(degress).addComponent(new AutoMoveComponent(degress, fireBallSpeed));
                createfireBall(boss.position).setRotation(degress + 0.25f).addComponent(new AutoMoveComponent(degress + 0.25f, fireBallSpeed));
                createfireBall(boss.position).setRotation(degress - 0.25f).addComponent(new AutoMoveComponent(degress - 0.25f, fireBallSpeed));
                createfireBall(boss.position).setRotation(degress + 0.5f).addComponent(new AutoMoveComponent(degress + 0.5f, fireBallSpeed));
                createfireBall(boss.position).setRotation(degress - 0.5f).addComponent(new AutoMoveComponent(degress - 0.5f, fireBallSpeed));
                currentState = DZMazeBossStates.Move;
            }

            Vector2 moveDir = Vector2.Zero;
            if ((int)boss.position.X > (int)dirPosition.X)
            {
                moveDir -= new Vector2(1, 0);
            }
            else if ((int)boss.position.X < (int)dirPosition.X)
            {
                moveDir += new Vector2(1, 0);
            }
            if ((int)boss.position.Y > (int)dirPosition.Y)
            {
                moveDir -= new Vector2(0, 1);
            }
            else if ((int)boss.position.Y < (int)dirPosition.Y)
            {
                moveDir += new Vector2(0, 1);
            }

            boss.position += moveDir * boss.moveSpeed * 2f * Time.deltaTime;
        }

        void FSuperFireBallAttack_Exit()
        {
            
        }
        #endregion

        private Entity createfireBall(Vector2 position)
        {
            var entity = Core.scene.addEntity(new MoveAbleEntity()).setPosition(boss.position);
            var texture = Core.scene.content.Load<Texture2D>("Images/ItemsObjects/fireBall");
            var subtexture = new Subtexture(texture, new Rectangle(0, 0, 32, 20));


            entity.addComponent(new Sprite(subtexture));
            entity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setIsSleepingAllowed(false)
                .setIsBullet(true);



            var shape = entity.addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(5)
                .setIsSensor(true);

            shape.setCollisionCategories(CollisionSetting.enemyAttackCategory);
            shape.setCollidesWith(CollisionSetting.wallCategory
                | CollisionSetting.playerCategory
                | CollisionSetting.tiledObjectCategory);

            shape.onAdded += () =>
            {
                shape.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var component = fixtureB.userData as Component;
                    var actorProperty = component.getComponent<ActorPropertyComponent>();
                    if (actorProperty != null)
                    {
                        actorProperty.executeDamage?.Invoke(AttackTypes.Physics, 5);
                        var adir = component.entity.position - boss.position;
                        adir.Normalize();
                        actorProperty.entity.getComponent<FSRigidBody>().body.applyLinearImpulse(adir * FSConvert.displayToSim);
                    }
                    entity.destroy();
                    return true;
                };
            };

            return entity;

        }

    }

}
