using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.TiledObjects
{
    public class Sign:Entity,IPauseable
    {
        public Action triggerEvent;
        public SceneObjectTriggerComponent trigger;
        bool collisioned;


        Sprite eButtonSprite;
        Sprite searchSprite;

        #region Constructor
        public Sign(string name,TiledObject tiledObject) : base(name)
        {
            couldPause = true;
            this.setPosition(new Vector2(tiledObject.x, tiledObject.y - tiledObject.height));
        }

        public bool couldPause { get; set; }
        #endregion

        #region Override 
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var eButtontexture = scene.content.Load<Texture2D>("Images/ItemsObjects/EButton");
            eButtonSprite = addComponent(new Sprite(eButtontexture));
            eButtonSprite.setLocalOffset(new Vector2(-14, -14 - eButtonSprite.height));
            eButtonSprite.enabled = false;

            var searchTexture = scene.content.Load<Texture2D>("Images/ItemsObjects/searchFont");
            searchSprite = addComponent(new Sprite(searchTexture));
            searchSprite.setLocalOffset(new Vector2(-14 + eButtonSprite.width * 2, -14 - eButtonSprite.height));
            searchSprite.enabled = false;

            var tiledMap = scene.content.Load<TiledMap>("TileMaps/MapObjects/Sign01");
            var tiledMapComponent = addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "object" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.actorMoverLayer)
                .setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y+8));
                

            var colliderObject = tiledMap.getObjectGroup("Collision");
            var collision = colliderObject.objectWithName("collision");

            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Kinematic)
                ;

            Vertices vertices = new Vertices(collision.polyPoints);
            var collider = addComponent(new FSCollisionPolygon(vertices));
            collider.setCollisionCategories(CollisionSetting.wallCategory);


            trigger = addComponent<SceneObjectTriggerComponent>();
            trigger.setRadius(10)
                    .setIsSensor(true);
            trigger.setCollisionCategories(CollisionSetting.ItemsCategory);
            trigger.setCollidesWith(CollisionSetting.playerCategory);

            trigger.onAdded += onAddedMethod;

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

        #region Collision Method
        private void onAddedMethod()
        {
            trigger.GetFixture().onCollision += onCollision;
            trigger.GetFixture().onSeparation += onSeparation; 
        }

        private bool onCollision(Fixture fixtureA,Fixture fixtureB,Contact contact)
        {

            collisioned = true;
            eButtonSprite.enabled = true;
            searchSprite.enabled = true;
            return true;
        }

        private void onSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            collisioned = false;
            eButtonSprite.enabled = false;
            searchSprite.enabled = false;
        }
        #endregion

    }
}
