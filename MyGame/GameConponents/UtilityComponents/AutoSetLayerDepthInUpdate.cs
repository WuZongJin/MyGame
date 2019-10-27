using MyGame.Common;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.UtilityComponents
{
    public class AutoSetLayerDepthInUpdate:Component,IUpdatable
    {
        #region 
        Sprite sprite;
        #endregion


        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            sprite = entity.getComponent<Sprite>();

        }

        public void update()
        {
            sprite.setLayerDepth(LayerDepthExt.caluelateLayerDepth(entity.position.Y));
        }
    }
}
