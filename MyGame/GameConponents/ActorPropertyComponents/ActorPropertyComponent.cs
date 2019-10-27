using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.GameConponents.AttackComponents;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.ActorPropertyComponents
{
    public class ActorPropertyComponent:Component
    {
        public Action<AttackTypes,int> executeDamage;

        #region Properties
        public int HP;              //血量
        public int MaxHP;           //最大血量
        public int MP;              //魔量
        public int MaxMP;           //最大魔法值


        public float moveSpeed;     //移动速度
        public float moveSpeedUpValue;      //装备和道具给移速增加的数值
        public float moveSpeedUpRate;       //装备和道具给移速增加的比率
        public float totalMoveSpeed
        {
            get { return moveSpeed + moveSpeedUpValue + moveSpeed * moveSpeedUpRate; }
        }

        public int damage;          //伤害
        public int damageUpValue;
        public float damageUpRate;
        public int totalDamage
        {
            get { return damage + damageUpValue + (int)(damage * damageUpRate); }
        }

        public int armor;           //护甲
        public int armorUpValue;

        public float magicResistance;  //魔抗
        public float magicResistanceUpValue;

        public float fireResistance;    //火抗
        public float fireResistanceUpValue;

        public float forzenResistance;  //冰冻抗性
        public float forzenResistanceUpValue;

        public float paralysisResistance;   //麻痹抗性
        public float paralysisResistanceUpValue;

        public float poisonResistance;      //毒抗
        public float poisonResistanceUpValue;
        #endregion


        #region Constractor
        public ActorPropertyComponent()
        {
            executeDamage += executeDamageMethod; 
        }
        #endregion

        //public void excuteDamage(AttackTrigerComponent component)
        //{
        //    var mdamange = (int)(component.damage * (1 - (Math.Sqrt(armor) * 10) / 100));
        //    excuteDamage(component.attackType, mdamange);
        //    //if(component.attackType == AttackTypes.Physics)
        //    //{
        //    //    var mdamange = (int)(component.damage * (1 - (Math.Sqrt(armor) * 10) / 100));
        //    //    this.HP -= mdamange;
        //    //    var damagenumEntity = Core.scene.createEntity("damageNum").setPosition(entity.position);
        //    //    damagenumEntity.addComponent(new Text(Graphics.instance.bitmapFont, mdamange.ToString(), Vector2.Zero, Color.White));
        //    //    Core.startCoroutine(damageNumEntityThrowUp(damagenumEntity));

        //    //    var sprite = entity.getComponent<Sprite>();
        //    //    sprite.color = Color.Red;
        //    //    Core.schedule(0.2f, timer => { if (entity!=null) sprite.color = Color.White; });
        //    //}
        //}

        private IEnumerator<object> damageNumEntityThrowUp(Entity entity)
        {
            float timer = 0f;
            while (true)
            {
                timer += Time.deltaTime;
                if (timer > 0.5f)
                {
                    entity.destroy();
                    yield break;
                }
                entity.position -= new Vector2(0, 1);
                yield return null;

            }
        }

        private  void executeDamageMethod(AttackTypes attackType,int damage)
        {
            var mdamange = (int)(damage * (1 - (Math.Sqrt(armor) * 10) / 100));


            this.HP -= mdamange;
            var damagenumEntity = Core.scene.createEntity("damageNum").setPosition(entity.position);
            damagenumEntity.addComponent(new Text(Graphics.instance.bitmapFont, damage.ToString(), Vector2.Zero, new Color(255,255,255,0)));
            Core.startCoroutine(damageNumEntityThrowUp(damagenumEntity));

            var sprite = entity.getComponent<Sprite>();
            sprite.color = Color.Red;
            Core.schedule(0.2f, timer => { if (entity != null) sprite.color = Color.White; });
        }
    }
}
