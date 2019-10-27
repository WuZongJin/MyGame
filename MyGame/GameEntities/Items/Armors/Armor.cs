using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using Nez;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.Armors
{
    public class Armor : Equitment, IEquitable
    {
        #region Properties


        #endregion

        #region Constructor
        public Armor(Texture2D icon,string name,string describetion, int saleMoney, params string[] properties)
        {
            this.itemIcon = new Subtexture(icon);
            equittypes = EquipmentTypes.Armor;
            this.name = name;
            this.saleMoney = saleMoney;
            this.describetion = describetion;
            this.properties = properties;
            
        }
        #endregion
    }
}
