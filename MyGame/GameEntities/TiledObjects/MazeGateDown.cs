using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Common;
using MyGame.Common.Game;
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

namespace MyGame.GameEntities.TiledObjects
{
    public class MazeGateDown : Entity,IPauseable
    {
        #region Properties
        public Action triggerEvent;
        Entity colliderEntity;
        bool collisioned = false;

        public bool couldPause { get; set; }
        #endregion

        #region Constractor
        public MazeGateDown()
        {
            couldPause = true;
        }

        #endregion

        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = scene.content.Load<Texture2D>("TileMaps/MapTextures/Overworld");
            var subtexture = new Subtexture(texture, new Rectangle(352, 0, 48, 16));
            addComponent(new Sprite(subtexture));

            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Kinematic);

            var shape = addComponent<SceneObjectTriggerComponentBox>();
            shape.setSize(48, 18);

            shape.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            shape.setCollidesWith(CollisionSetting.playerCategory);

            shape.onAdded += () =>
            {
                shape.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    collisioned = true;
                    return true;
                };

                shape.GetFixture().onSeparation += (fixtureA, fixtureB) =>
                {
                    collisioned = false;
                };
            };


            colliderEntity = scene.createEntity("collision").setPosition(this.position);
            colliderEntity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Static);

            colliderEntity.addComponent<FSCollisionBox>()
                .setSize(48, 16);


        }

        public override void update()
        {
            if (couldPause && GameSetting.isGamePause)
                return;

            base.update();
            if (collisioned)
            {
                if (Input.isKeyPressed(Keys.E))
                {
                    triggerEvent?.Invoke();
                }
            }
        }

        public void destoryedByKey()
        {
            colliderEntity.destroy();
            this.destroy();
        }

    }
}
