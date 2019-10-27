using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.TiledObjects
{
    public class Barrier_Tree_Exposion:Entity
    {
        public Action triggerEvent;

        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = scene.content.Load<Texture2D>("TileMaps/MapTextures/Overworld");
            var subtexture = new Subtexture(texture,new Rectangle(48, 80, 48, 16));
            var sprite = addComponent(new Sprite(subtexture));
            //sprite.setLocalOffset(new Vector2(24, 8));
            sprite.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Kinematic);

            
            var collider = addComponent(new SceneObjectTriggerComponentBox());
            collider.setSize(48, 16);
            collider.setCollisionCategories(CollisionSetting.ItemsCategory);
            collider.setCollidesWith(CollisionSetting.expositionCategory);

            collider.onAdded += onAddedMethod;
        }

        private void onAddedMethod()
        {
            var collider = getComponent<SceneObjectTriggerComponentBox>();
            collider.GetFixture().onCollision += onCollision;
            
        }
        private bool onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            var collider = getComponent<SceneObjectTriggerComponentBox>();
            triggerEvent?.Invoke();
            return true;
        }

        public void destoryByExposion()
        {
            this.destroy();
        }
    }
}
