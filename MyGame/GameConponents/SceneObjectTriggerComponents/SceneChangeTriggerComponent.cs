using Microsoft.Xna.Framework;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.SceneObjectTriggerComponents
{
    public class SceneChangeTriggerComponent:FSCollisionBox
    {
        #region Properties
        public Action onAdded;
        #endregion

        #region Constructor
        public SceneChangeTriggerComponent()
        {

        }
        #endregion

        #region override Method
        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            onAdded?.Invoke();
        }
        #endregion
    }
}
