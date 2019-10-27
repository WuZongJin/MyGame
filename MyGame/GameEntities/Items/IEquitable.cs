using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public delegate void OnEuqitedEventHandler(Equitment equitable, Player.Player equitableer);
    public delegate void OnTackOffEventHandler(Equitment equitable, Player.Player equitableer);

    public interface IEquitable
    {
        Player.Player equitableer { get; set; }
        Subtexture itemIcon { get; set; }
        EquipmentTypes equittypes { get; set; }
        string[] properties { get; set; }
        string describetion { get; set; }
        string name { get; set; }
        int saleMoney { get; set; }
        OnEuqitedEventHandler equit { get; set; }
        OnTackOffEventHandler tackOff { get; set; }

    }
}
