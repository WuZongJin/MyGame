using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public class ItemMaterials:ItemComponent
    {
        public ItemMaterials()
        {
            types = ItemTypes.Material;
        }
    }
}
