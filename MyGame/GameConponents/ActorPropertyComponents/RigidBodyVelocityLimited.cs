using Microsoft.Xna.Framework;
using Nez;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.ActorPropertyComponents
{
    class RigidBodyVelocityLimited:Component,IUpdatable
    {
        FSRigidBody rigidBody;

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            rigidBody = entity.getComponent<FSRigidBody>();
        }

        public void update()
        {
            if (rigidBody.body.linearVelocity.Length() < 0.5f)
            {
                rigidBody.body.linearVelocity = Vector2.Zero;
            }
        }
    }
}
