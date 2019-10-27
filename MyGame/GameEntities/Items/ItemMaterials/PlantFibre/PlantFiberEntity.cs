using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using MyGame.Common;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameResources;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public class PlantFiberEntity:Entity
    {
        int count;

        public PlantFiberEntity(int count = 1) : base("plantFieberEntity")
        {
            this.count = count;
        }

        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = GameTextureResource.xObjectPacker.Packer.getSubtexture(153);
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
                picker.pickUp(new PlantFibre(), this.count);
                this.destroy();
            }
            return true;
        }

    }
}
