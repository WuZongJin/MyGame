using Nez;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys
{
    public class EnemyViewComponent:FSCollisionCircle
    {
        public Action onAdded;
        public EnemyViewComponent()
        {

        }
        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            onAdded?.Invoke();
        }



    }
}
