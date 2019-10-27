using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameResources;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.NormalRecoverMedicine
{
    public class NormalRecoverMedicine:ExecuteAbleProps, IExecuteAbleItem
    {
        public NormalRecoverMedicine()
        {
            id = GameItemId.normalrestoremedicine;
            name = "普通的HP恢复药水";
            itemIcon = new Nez.Textures.Subtexture(Core.content.Load<Texture2D>("Images/ItemsIcon/P_Red04"));
            describetion = "普通的恢复药品，能够恢复使用者少量的生命值";
            properties = new string[] { "HP+10" };
            saleMoney = 2;
        }

        public override void excute(Entity entity)
        {
            var actorProperty = entity.getComponent<ActorPropertyComponent>();
            actorProperty.HP += 10;
            if (actorProperty.HP > actorProperty.MaxHP)
                actorProperty.HP = actorProperty.MaxHP;
        }
    }
}
