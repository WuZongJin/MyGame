using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GlobalManages.Notification
{
    public class NotificationMessageComponent : RenderableComponent
    {
        NotificationMessage message;
        Matrix presentMatrix;
        Vector2 position;
        Point textureSize = new Point(16);
        public override float width => 200;
        public override float height => 200;

        public NotificationMessageComponent(NotificationMessage message, Vector2 position)
        {
            this.message = message;
            this.position = position;
        }

        public override void render(Graphics graphics, Camera camera)
        {
            presentMatrix = graphics.batcher.transformMatrix;
            graphics.batcher.end();
            graphics.batcher.begin();

            graphics.batcher.draw(message.texture, new Rectangle(position.ToPoint(), textureSize),message.texture.sourceRect,Color.White);
            graphics.batcher.drawString(graphics.bitmapFont, message.message, new Vector2(position.X+ textureSize.X,position.Y), new Color(255, 255, 255, 0));


            graphics.batcher.end();
            graphics.batcher.begin(presentMatrix);
        }
    }
}
