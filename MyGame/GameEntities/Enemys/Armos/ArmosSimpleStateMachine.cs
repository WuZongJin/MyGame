using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameEntities.Utils;
using Nez;
using Nez.AI.FSM;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Armos
{
    public enum ArmosStates
    {
        Defend,
        Attack,
    }

    public class ArmosSimpleStateMachine:SimpleStateMachine<ArmosStates>
    {
        #region Properties
        Armos armos;

        float attackfrequency = 1f;
        float attackTimer = 0f;
        #endregion

        #region Constructor
        public ArmosSimpleStateMachine(Armos armos)
        {
            this.armos = armos;
            initialState = ArmosStates.Defend;
        }

        #endregion

        #region Defend
        void Defend_Enter()
        {
            armos.shiled.enabled = true;
            armos.sprite.color = Color.DarkGray;
            armos.collider.setCollisionCategories(CollisionSetting.unableColliderCategory);

        }

        void Defend_Tick()
        {
            if (!armos.isTargetToClose)
            {
                currentState = ArmosStates.Attack;
            }
        }

        void Defend_Exit()
        {
            armos.shiled.enabled = false;
            armos.rigidBody.setBodyType(BodyType.Dynamic);
            armos.collider.setCollisionCategories(CollisionSetting.enemyCategory);

        }
        #endregion

        #region Attack
        void Attack_Enter()
        {
            armos.shiled.enabled = false;
            armos.sprite.color = Color.White;
        }

        void Attack_Tick()
        {
            attackTimer += Time.deltaTime;
            if(attackTimer> attackfrequency)
            {
                attackTimer = 0f;
                var attackEntity = armos.scene.addEntity(new MoveAbleEntity()).setPosition(armos.position);
                attackEntity.addComponent(new Sprite(GameResources.GameTextureResource.xObjectPacker.Packer.getSubtexture(634)));
                attackEntity.addComponent(new AutoSetLayerDepthInUpdate());

                var dir = armos.target.position - armos.position;
                dir.Normalize();
                attackEntity.addComponent(new AutoMoveComponent(dir, 100));
                var rigidBody = attackEntity.addComponent<FSRigidBody>()
                    .setBodyType(BodyType.Dynamic);

                var trigger = attackEntity.addComponent<SceneObjectTriggerComponent>();
                trigger.setRadius(5).setIsSensor(true);

                trigger.setCollisionCategories(CollisionSetting.enemyAttackCategory);
                trigger.setCollidesWith(CollisionSetting.wallCategory
                    | CollisionSetting.playerCategory
                    | CollisionSetting.tiledObjectCategory);

                trigger.onAdded += () =>
                {


                    trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                    {
                        var component = fixtureB.userData as Component;

                        if (component == armos.collider|| component.entity == armos.colliderEntity)
                            return true;
                        var actorProperty = component.getComponent<ActorPropertyComponent>();
                        if (actorProperty != null)
                        {
                            actorProperty.executeDamage?.Invoke(AttackTypes.Physics, 5);
                            var adir = component.entity.position - armos.position;
                            adir.Normalize();
                            actorProperty.entity.getComponent<FSRigidBody>().body.applyLinearImpulse(adir * FSConvert.displayToSim);

                        }
                        attackEntity.destroy();
                        return true;
                    };
                };
            }

            if (armos.isTargetToClose)
            {
                currentState = ArmosStates.Defend;
            }

        }

        void Attack_Exit()
        {

        }
        #endregion

    }
}
