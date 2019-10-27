using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Common
{
    public interface IFallinable
    {
        Vector2 fallinReturnPosition { get; set; }
        Fixture fallinHole { get; set; }
        bool potentialFallin { get; set; }
        Action fallin { get; set; }
    }
}
