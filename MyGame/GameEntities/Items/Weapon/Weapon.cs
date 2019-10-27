using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.Weapon
{
    public class Weapon : Equitment
    {
        #region Iequitable 
        public int maxDamage;
        public int minDamage;
        public float Attackinterval = 0.3f;
        public bool isAttacked = false;
        public bool isAttackOver = false;
        public AttackTypes attackType = AttackTypes.Physics;
        #endregion

        #region Constructor
        public Weapon(Texture2D texture, string name,int maxDamage,int minDamage,string desc, int saleMoney, params string[] properties)
        {
            this.itemIcon = new Nez.Textures.Subtexture(texture);
            this.equittypes = EquipmentTypes.Weapon;
            this.name = name;
            this.maxDamage = maxDamage;
            this.minDamage = minDamage;
            this.describetion = desc;
            this.saleMoney = saleMoney;
            this.properties = properties;
        }
        public Weapon() { }

        #endregion

        #region Method
        public virtual void update()
        {  }
        public virtual void beginAttack()
        { }
        public virtual void attack()
        { }
        public virtual void endAttack()
        { }
        #endregion

    }
}
