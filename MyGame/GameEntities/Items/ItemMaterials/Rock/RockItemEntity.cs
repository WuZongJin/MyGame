using FarseerPhysics.Dynamics;
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

namespace MyGame.GameEntities.Items
{
    public class RockItemEntity : Entity
    {
        int count;

        public RockItemEntity(int count = 1)
        {
            this.count = count;
        }

        #region override
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = GameResources.GameTextureResource.xObjectPacker.Packer.getSubtexture(390);
            addComponent(new Sprite(texture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setMass(2f);

            var shape = addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(7)
                 .setIsSensor(true);
            shape.setCollisionCategories(CollisionSetting.ItemsCategory);
            shape.setCollidesWith(CollisionSetting.playerCategory);

            shape.onAdded += () =>
            {
                shape.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var picker = ((Component)fixtureB.userData).entity as Player.Player;

                    if (picker != null)
                    {
                        picker.pickUp(new RockItem(), this.count);
                        this.destroy();
                    }
                    return true;
                };
            };
        }
        #endregion
    }
}
