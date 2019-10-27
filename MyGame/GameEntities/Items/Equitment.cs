using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Common;

namespace MyGame.GameEntities.Items
{
    public class Equitment:ItemComponent,IEquitable
    {

        public Equitment()
        {
            types = ItemTypes.Equitment;
        }

        public Player.Player equitableer { get; set; }
        public EquipmentTypes equittypes { get; set; }
        public OnEuqitedEventHandler equit { get; set; }
        public OnTackOffEventHandler tackOff { get; set; }
    }
}
