using Microsoft.Xna.Framework;
using Nez;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.UtilityComponents
{
    public class AutoMoveComponent : Component, IUpdatable
    {
        Vector2 direction;
        float speed;


        public AutoMoveComponent(Vector2 direction,float speed)
        {
            this.direction = direction;
            this.speed = speed;
        }

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
           
        }

        public void update()
        {
            entity.position += direction * speed * Time.deltaTime;
        }
    }
}
