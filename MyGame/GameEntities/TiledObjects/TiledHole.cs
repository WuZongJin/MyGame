using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameConponents.UtilityComponents;
using Nez;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.TiledObjects
{
    public class TiledHole:Entity
    {

        Vector2 holeSize;
        public TiledHole(Vector2 position,Vector2 size) : base("fallinHole")
        {
            this.setPosition(position);
            this.holeSize = size;
        }

        public override void onAddedToScene()
        {
            base.onAddedToScene();

            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var box = addComponent(new SceneObjectTriggerComponentBox());
            box.setSize(holeSize.X, holeSize.Y).setIsSensor(true);

            box.setCollisionCategories(CollisionSetting.tiledHoleCategory);
            box.setCollidesWith(CollisionSetting.playerCategory | CollisionSetting.enemyCategory);

            box.onAdded += () =>
            {
                box.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var component = fixtureB.userData as Component;
                    var fallin = component.entity.getComponent<FallinAbleComponent>();
                    if (fallin != null)
                    {
                        var mposition = component.entity.position;
                       
                        fallin.fallinHole = box.GetFixture();
                        fallin.potentialFallin = true;
                        fallin.fallinReturnPosition = mposition;
                    }

                    return true;
                };
                box.GetFixture().onSeparation += (fixtureA, fixtureB) =>
                {
                    var component = fixtureB.userData as Component;
                    var fallin = component.entity.getComponent<FallinAbleComponent>();
                    if (fallin!=null)
                    {
                        var mposition = component.entity.position;
                        fallin.fallinHole = null;
                        fallin.potentialFallin = false;
                        fallin.fallinReturnPosition = Vector2.Zero;
                    }
                };

            };

        }


    }
}
