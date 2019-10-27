using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using MyGame.Common;
using MyGame.GameConponents.ActorPropertyComponents;
using Nez;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.AttackComponents
{
    public class AttackEntitySystem : EntityProcessingSystem
    {
        public AttackEntitySystem(Matcher matcher) : base(matcher)
        {
        }

        public override void onAdded(Entity entity)
        {
            base.onAdded(entity);
            var rigidBody = entity.getComponent<FSRigidBody>();
            var collider = entity.getComponent<FSCollisionShape>();
            //rigidBody.body.onCollision += Body_onCollision;
            var fsworld = entity.scene.getSceneComponent<FSWorld>();
            //fsworld.world.addController(Contr)
        }

        

        public override void process(Entity entity)
        {
            //var attack = entity.getComponent<AttackTrigerComponent>();
            //var rigidBody = entity.getComponent<FSRigidBody>();
            //var fsworld = entity.scene.getSceneComponent<FSWorld>();
           //fsworld.world.contactManager
        }

        private bool queryCallback(Fixture fixture)
        {
            return true;
        }
    }
}
