using Microsoft.Xna.Framework;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.SceneObjectTriggerComponents
{
    public class SceneObjectTriggerComponent : FSCollisionCircle
    {
        public Action onAdded;

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            onAdded?.Invoke();
        }


    }

    public class SceneObjectTriggerComponentBox : FSCollisionBox
    {
        public Action onAdded;

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            onAdded?.Invoke();
        }


    }

    public class SceneObjectTriggerComponentPolygon : FSCollisionPolygon
    {
        public Action onAdded;

        public SceneObjectTriggerComponentPolygon(List<Vector2> vector2s) : base(vector2s)
        {

        }

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            onAdded?.Invoke();
        }
    }
}
