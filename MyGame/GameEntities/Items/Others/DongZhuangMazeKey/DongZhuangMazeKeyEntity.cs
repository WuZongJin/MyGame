using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.Others
{
    public class DongZhuangMazeKeyEntity:Entity
    {
        public DongZhuangMazeKeyEntity() : base("key")
        {
            this.setScale(0.5f);
        }

        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = scene.content.Load<Texture2D>("Images/ItemsIcon/I_Key01");

            addComponent(new Sprite(texture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var shape = addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(7)
                 .setIsSensor(true);
            shape.setCollisionCategories(CollisionSetting.ItemsCategory);
            shape.setCollidesWith(CollisionSetting.playerCategory);

            shape.onAdded = () =>
            {
                shape.GetFixture().onCollision += Body_onCollision;
            };
        }

        private bool Body_onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            var picker = ((Component)fixtureB.userData).entity as Player.Player;

            if (picker != null)
            {
                picker.pickUp(new DongZhuangMazeKey(), 1);
                this.destroy();
            }
            return true;
        }


    }
}
