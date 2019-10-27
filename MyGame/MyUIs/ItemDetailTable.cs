using MyGame.GameEntities.Items;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.MyUIs
{
    public class ItemDetailTable : Table
    {
        #region Properties
        ItemComponent item;
        #endregion

        public ItemDetailTable(ItemComponent equitable) 
        {
            this.item = equitable;
        }

        

    }
}
