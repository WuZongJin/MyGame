using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using Nez;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.UtilityComponents
{
    public class FallinAbleComponent : Component, IFallinable,Nez.IUpdatable
    {

        #region Properties
        public Vector2 fallinReturnPosition { get; set; }
        public Fixture fallinHole { get; set; }
        public Fixture nextfallinHole { get; set; }
        public bool potentialFallin { get; set; }
        public Action fallin { get; set; }

        #endregion


        public void update()
        {
            if (potentialFallin)
            {
                var rigidBody = entity.getComponent<FSRigidBody>();

                var tposition = rigidBody.body.position + new Vector2(0f, 0.05f);
                if (fallinHole.testPoint(ref tposition))
                {
                    fallin?.Invoke();
                }
            }
        }
    }
}
