using Microsoft.Xna.Framework;
using MyGame.GameConponents.ActorPropertyComponents;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Slime
{
    public class BigSlimeHPBar : RenderableComponent
    {
        BigSlime bigSlime;
        ActorPropertyComponent actorProperty;
        Matrix presentMatrix;

        string name = "巨型史莱姆";
        float nameWidth;

        int namePositionY = 520;
        int barPositionY = 550;

        public override float height => 100;
        public override float width => 100;
        #region Constructor
        public BigSlimeHPBar(BigSlime bigSlime)
        {
            this.bigSlime = bigSlime;
            actorProperty = bigSlime.getComponent<ActorPropertyComponent>();
            nameWidth = Graphics.instance.bitmapFont.measureString(name).X;
        }
        #endregion

        public override void render(Graphics graphics, Camera camera)
        {
            presentMatrix = graphics.batcher.transformMatrix;
            graphics.batcher.end();
            graphics.batcher.begin();

            graphics.batcher.drawString(graphics.bitmapFont, "巨型史莱姆", new Vector2(Screen.width / 2f - nameWidth / 2f, 500), new Color(255, 255, 255, 0));
            var currentHP = (int)(((float)actorProperty.HP / (float)actorProperty.MaxHP)*100);
            for (int i = 0; i < currentHP; i++)
            {
                graphics.batcher.drawRect(Screen.width / 2f - 200 + i * 4, 530, 4, 20, Color.Red);
            }
            graphics.batcher.drawHollowRect(Screen.width / 2f - 200, 530, 400, 20, Color.Black,2);
            

            graphics.batcher.end();
            graphics.batcher.begin(presentMatrix);
        }
    }
}
