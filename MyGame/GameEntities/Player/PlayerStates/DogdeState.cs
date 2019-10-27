using Nez.AI.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerStates
{
    public class DogdeState:State<Player>
    {
        #region Properties
        bool dodgeed = false;
        #endregion

        public override void onInitialized()
        {
            base.onInitialized();
        }

        public override void begin()
        {
            base.begin();
        }

        public override void reason()
        {
            base.reason();
        }

        public override void update(float deltaTime)
        {
            
        }

        public override void end()
        {
            base.end();
        }

    }
}
