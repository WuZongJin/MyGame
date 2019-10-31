using MyGame.Common;
using MyGame.Common.Game;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Utils
{
    public class MoveAbleEntity : Entity, IPauseable
    {
        public bool couldPause { get; set; }

        public MoveAbleEntity()
        {
            couldPause = true;
        }


        #region Override Method
        public override void update()
        {
            if (couldPause && GameSetting.isGamePause)
                return;
            base.update();

        }
        #endregion
    }
}
