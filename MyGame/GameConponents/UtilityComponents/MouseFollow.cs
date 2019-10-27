using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.UtilityComponents
{
    public class MouseFollow : Component, IUpdatable
    {
        public void update()
        {
            entity.setPosition(Input.scaledMousePosition);
        }
    }
}
