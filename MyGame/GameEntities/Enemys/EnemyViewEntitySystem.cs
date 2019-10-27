using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys
{
    public class EnemyViewEntitySystem : EntitySystem
    {
        public EnemyViewEntitySystem(Matcher matcher) : base(matcher)
        {
        }

        public override void onAdded(Entity entity)
        {
            base.onAdded(entity);
           
        }


    }
}
