using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GlobalManages.Notification
{
    public class NotificationMessage
    {
        #region Properties
        public Subtexture texture;
        public string message;
        public float showTime = 3f;
        public float timer = 0f;
        public bool shoudRemove = false;
        #endregion

        public NotificationMessage(Subtexture texture, string message)
        {
            this.texture = texture;
            this.message = message;
        }
    }
}
