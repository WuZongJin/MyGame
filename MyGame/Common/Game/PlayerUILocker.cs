using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Common.Game
{
    public enum PlayerUIElementLocker
    {
        HPBar=1,
        MPBar=2,
        ItemWindow=4,
        EquitWindow=8,
        CreateItemWindow = 16,
        ExecuteablePropsWindow = 32,
    }
    public static class PlayerUILocker
    {
        public static PlayerUIElementLocker playerUILocker = PlayerUIElementLocker.HPBar|PlayerUIElementLocker.ItemWindow|PlayerUIElementLocker.CreateItemWindow|PlayerUIElementLocker.ExecuteablePropsWindow|PlayerUIElementLocker.EquitWindow;
    }
}
