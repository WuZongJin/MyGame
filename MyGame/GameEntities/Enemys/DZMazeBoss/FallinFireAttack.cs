using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
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
    public class FallinFireAttack:Entity,IPauseable
    {
        

        public enum FireAnimation
        {
            Init,
            Fire,
        }


        Sprite<FireAnimation> animations;
        SceneObjectTriggerComponent trigger;
        float initTime = 1f;
        float initTimer = 0f;
        bool fired = false;


        public bool couldPause { get; set; }

        public FallinFireAttack()
        {
            couldPause = true;
        }


        #region Override
        public override void update()
        {
            if (couldPause && GameSetting.isGamePause)
                return;
            base.update();
            if (!fired)
            {
                initTimer += Time.deltaTime;
                if (initTimer > initTime)
                {
                    fired = true;
                    animations.currentAnimation = FireAnimation.Fire;
                    trigger.setCollidesWith(CollisionSetting.playerCategory);
                }
            }
            
        }

        public override void onAddedToScene()
        {
            var texture = scene.content.Load<Texture2D>("TileMaps/MapTextures/objects");
            var subtexture1 = new Subtexture(texture, new Rectangle(64, 48, 16, 16));
            var subtexture2 = new Subtexture(texture, new Rectangle(64 + 16, 48, 16, 16));
            var subtexture3 = new Subtexture(texture, new Rectangle(64 + 16*2, 48, 16, 16));
            var subtexture4 = new Subtexture(texture, new Rectangle(64 + 16*3, 48, 16, 16));
            var subtexture5 = new Subtexture(texture, new Rectangle(64 + 16*4, 48, 16, 16));
            var subtexture6 = new Subtexture(texture, new Rectangle(64 + 16*5, 48, 16, 16));
            var subtexture7 = new Subtexture(texture, new Rectangle(64 + 16*6, 48, 16, 16));
            var subtexture8 = new Subtexture(texture, new Rectangle(64 + 16*7, 48, 16, 16));
            var subtexture9 = new Subtexture(texture, new Rectangle(64 + 16*8, 48, 16, 16));
            var subtexture10 = new Subtexture(texture, new Rectangle(64 + 16*9, 48, 16, 16));
            var subtexture11 = new Subtexture(texture, new Rectangle(64 + 16*10, 48, 16, 16));

            animations = addComponent(new Sprite<FireAnimation>(subtexture11));
            animations.addAnimation(FireAnimation.Init, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture11,
                subtexture10,
                subtexture9,
                subtexture8,
            })
            {
                
            }) ;

            animations.addAnimation(FireAnimation.Fire, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture7,
                subtexture6,
                subtexture5,
                subtexture4,
                subtexture3,
                subtexture2,
                subtexture1,

            })
            {
                loop = false
            }) ;

            animations.onAnimationCompletedEvent += Animations_onAnimationCompletedEvent;

            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            trigger = addComponent<SceneObjectTriggerComponent>();
            trigger.setRadius(5).setIsSensor(true);

            trigger.setCollisionCategories(CollisionSetting.enemyAttackCategory);
            trigger.setCollidesWith(CollisionSetting.unableColliderCategory);

            trigger.onAdded += () =>
            {
                trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var component = fixtureB.userData as Component;

                    var actorProperty = component.getComponent<ActorPropertyComponent>();
                    if (actorProperty != null)
                    {
                        actorProperty.executeDamage?.Invoke(AttackTypes.Physics, 8);
                        //var adir = component.entity.position - this.position;
                        //adir.Normalize();
                        //actorProperty.entity.getComponent<FSRigidBody>().body.applyLinearImpulse(adir * FSConvert.displayToSim);

                    }

                    return true;
                };
            };
        }

        private void Animations_onAnimationCompletedEvent(FireAnimation obj)
        {
           
            if(obj == FireAnimation.Fire)
            {
                this.destroy();
            }
        }


        #endregion



    }
}
