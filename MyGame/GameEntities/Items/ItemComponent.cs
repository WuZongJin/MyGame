using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public abstract class ItemComponent :IPickupable
    {
        public enum ItemTypes
        {
            Equitment,      //装备
            Material,       //材料
            RestoreProp,    //恢复道具
            ExecuteableProp,    //可使用的道具
            Other,            //其它
        }

        #region Properties
        public string id { get; set; }
        public ItemTypes types { get; set; }
        public Subtexture itemIcon { get; set; }
        public string name { get; set; }
        public string describetion { get; set; }
        public string[] properties { get; set; }
        public int saleMoney { get; set; }

        #endregion

        public override string ToString()
        {
            return String.Format("name:{0},Type:{1}", name,types);
        }

    }
}
