using Microsoft.Xna.Framework;
using MyGame.Common.Game;
using MyGame.GameEntities.Items;
using MyGame.GlobalManages.Notification;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GlobalManages
{
    public class NotificationManager:GlobalManager
    {
        #region Properties
        Queue<NotificationMessage> leftbottomNotification;
        List<NotificationMessage> notificationToAdded;
        float minInterval = 0.5f;
        float timer = 0f;
        bool isNotificationChanged = false;
        #endregion

        #region Constructor
        public NotificationManager()
        {
            leftbottomNotification = new Queue<NotificationMessage>();
            notificationToAdded = new List<NotificationMessage>();
        }
        #endregion

        #region Override Method
        public override void update()
        {
            updateList();
            foreach(var message in leftbottomNotification)
            {
                message.timer += Time.deltaTime;
                if (message.timer > message.showTime)
                {
                    message.shoudRemove = true;
                }
            }

            if (isNotificationChanged)
            {
                notificationChanged();
            }
                

        }

        #endregion

        #region Method
        public void addNotification(NotificationMessage message) 
        {
            notificationToAdded.Add(message);
        }

        public void getItemNotifacation(ItemComponent item,int count = 1)
        {
            NotificationMessage message = new NotificationMessage(item.itemIcon,$"获得{item.name} x {count}");
            notificationToAdded.Add(message);
        }

        void notificationChanged()
        {
            if (Core.scene != null)
            {
                var entity = Core.scene.findEntity("notification");
                if (entity == null)
                {
                    entity = Core.scene.createEntity("notification");
                }
                entity.removeAllComponents();
                int startY = 500;
                int startX = 10;
                foreach(var message in leftbottomNotification)
                {
                    entity.addComponent(new NotificationMessageComponent(message,new Vector2(startX, startY)))
                        .setRenderLayer(GameLayerSetting.playerUiLayer);
                    startY += 20;
                }

                isNotificationChanged = false;
            }
        }

        void updateList()
        {
            if (leftbottomNotification.Count>0 && leftbottomNotification.Peek().shoudRemove)
            {
                leftbottomNotification.Dequeue();
                isNotificationChanged = true;
            }
                

            timer += Time.deltaTime;
            if(notificationToAdded.Count > 0 && leftbottomNotification.Count < 4)
            {
                leftbottomNotification.Enqueue(notificationToAdded.First());
                notificationToAdded.Remove(notificationToAdded.First());
                isNotificationChanged = true;
            }
            else if (notificationToAdded.Count > 0 && leftbottomNotification.Count>4 &&timer>minInterval)
            {
                timer = 0f;
                leftbottomNotification.Dequeue();
                leftbottomNotification.Enqueue(notificationToAdded.First());
                notificationToAdded.Remove(notificationToAdded.First());
                isNotificationChanged = true;
            }
        }
        #endregion

    }
}
