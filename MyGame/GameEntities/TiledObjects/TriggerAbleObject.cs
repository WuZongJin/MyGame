using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.TiledObjects
{
    public class TriggerAbleObject:Entity,IPauseable
    {
        public Action triggerEvent;
        string text;
        float radius;
        bool collisioned;
        public bool couldPause { get; set; }
        public TriggerAbleObject(float radius = 10,string text = null)
        {
            couldPause = true;
            this.radius = radius;
            this.text = text ?? "摁E：调查";
        }

        #region Override Method
        public override void onAddedToScene()
        {
            var textComponent = addComponent(new Text(Graphics.instance.bitmapFont, text, new Vector2(0, -10), Color.White));
            textComponent.enabled = false;

            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var shape = addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(radius).setIsSensor(true);

            shape.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            shape.setCollidesWith(CollisionSetting.playerCategory);

            shape.onAdded =()=>{
                shape.GetFixture().onCollision += onCollision;
                shape.GetFixture().onSeparation += onSeparation;
            };
        }

        public override void update()
        {
            if (couldPause && GameSetting.isGamePause) return;
            base.update();
            if (collisioned)
            {
                if (Input.isKeyDown(Keys.E))
                {
                    triggerEvent?.Invoke();
                }
            }
        }
        #endregion


        #region Collision Event
        private bool onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            getComponent<Text>().enabled = true;
            collisioned = true;
            
            return true;
        }

        private void onSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            collisioned = false;
            getComponent<Text>().enabled = false;
        }
        #endregion

    }
}
