using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GameResources;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.Bombs
{
    public class BombComponent : ExecuteAbleProps
    {
        public BombComponent()
        {
            id = GameItemId.bomb;
            name = "炸弹";
            itemIcon = GameTextureResource.boomTexture.Packer.getSubtexture("bomb");
            describetion = "炸弹，放置3S后发生爆炸";
            properties = new string[] { "爆炸伤害：30" };
            this.saleMoney = 10;
        }



        public override void excute(Entity entity)
        {
            Core.scene.addEntity(new BombEntity()).setPosition(entity.position);
        }
    }
}
