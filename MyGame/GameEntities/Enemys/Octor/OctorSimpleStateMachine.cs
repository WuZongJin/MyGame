using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameConponents.UtilityComponents;
using Nez;
using Nez.AI.FSM;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Octor
{
    public enum OctorSompleStates
    {
        Attack,
        Appera,
        HideToAppera,
        Hide,
    }

    public class OctorSimpleStateMachine:SimpleStateMachine<OctorSompleStates>
    {
        #region Properties
        Octor octor;
        Vector2 direction;

        bool attacked = false;

        float hideTime = 2f;
        float hideTimer = 0f;

        float hideToapperaTime = 0.5f;
        float hideToapperaTimer = 0f;

        float apperaTime = 1f;
        float apperaTimer = 0f;
        float attackTime = 2f;
        float attackTimer = 0f;

        #endregion

        public OctorSimpleStateMachine(Octor octor)
        {
            this.octor = octor;
            initialState = OctorSompleStates.Hide;
        }

        #region  Hide
        void Hide_Enter()
        {
            octor.animations.currentAnimation = Octor.OctorAnimations.Hide;
        }

        void Hide_Tick()
        {
            hideTimer += Time.deltaTime;
            if (hideTimer > hideTime)
            {
                if (octor.isFindTarget)
                {
                    hideTimer = 0f;
                    currentState = OctorSompleStates.Appera;
                }
            }
            if (octor.inHideBeAttacked)
            {
                currentState = OctorSompleStates.HideToAppera;
            }
        }
        #endregion

        #region Hide To Appera
        void HideToAppera_Enter()
        {
            octor.animations.color = Color.Transparent;
        }

        void HideToAppera_Tick()
        {
            hideToapperaTimer += Time.deltaTime;
            if (hideToapperaTimer > hideToapperaTime)
            {
                currentState = OctorSompleStates.Appera;
            }
        }

        void HideToAppera_Exit()
        {
            octor.animations.color = Color.White;
        }

        #endregion

        #region Appera
        void Appera_Enter()
        {
            octor.animations.currentAnimation = Octor.OctorAnimations.Appera;
        }

        void Appera_Tick()
        {
            apperaTimer += Time.deltaTime;
            if (apperaTimer > apperaTime)
            {
                if (octor.isFindTarget)
                {
                    apperaTimer = 0f;
                    currentState = OctorSompleStates.Attack;
                    return;
                }
            }

            if (!octor.isFindTarget && !octor.inHideBeAttacked)
            {
                currentState = OctorSompleStates.Hide;
            }
            else if (!octor.isFindTarget)
            {
                apperaTimer = 0f;
            }

        }
        #endregion

        #region Attack
        void Attack_Enter()
        {
            octor.animations.currentAnimation = Octor.OctorAnimations.Attack;
            attacked = false;
        }

        void Attack_Tick()
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > 1f&&!attacked&& octor.isFindTarget)
            {
                attacked = true;
                var attackEntity = octor.scene.createEntity("attackEntity").setPosition(octor.position);
                attackEntity.addComponent(new Sprite(GameResources.GameTextureResource.xObjectPacker.Packer.getSubtexture(634)));
                attackEntity.addComponent(new AutoSetLayerDepthInUpdate());

                var dir = octor.target.position - octor.position;
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

                        if (component == octor.colliderShape)
                            return true;
                        var actorProperty = component.getComponent<ActorPropertyComponent>();
                        if (actorProperty != null)
                        {
                            actorProperty.executeDamage?.Invoke(AttackTypes.Physics, 5);
                        }
                        attackEntity.destroy();
                        return true;
                    };
                };




            }

            if(attackTimer> attackTime)
            {
                attackTimer = 0f;
                if (octor.inHideBeAttacked)
                {
                    currentState = OctorSompleStates.Appera;
                }
                else
                {
                    currentState = OctorSompleStates.Hide;
                }
            }
        }
        #endregion

    }
}
