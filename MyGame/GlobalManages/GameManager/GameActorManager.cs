using MyGame.GameEntities.Player;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GlobalManages.GameManager
{
    public class GameActorManager : GlobalManager
    {
        #region Properties
        public Player player;
        #endregion
        #region Constructor
        public GameActorManager()
        {

        }
        #endregion

        #region Player Method
        public void initPlayer()
        {
            player = new Player();
        }
        #endregion

    }
}
