using MyGame.Common;
using MyGame.GameEntities.Items.Weapon;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.AttackComponents
{
    public class AttackTrigerComponent:Component
    {
        #region 
        Weapon weapon;
        public Action onAdded;
        public AttackTypes attackType
        {
            get { return weapon.attackType; }
        }
        public int damage
        {
            get
            {
                return weapon.equitableer.actorProperty.totalDamage + Nez.Random.range(weapon.minDamage,weapon.maxDamage);
            }
        }
        public float excuteTime;
        #endregion

        #region Constructor
        public AttackTrigerComponent(Weapon weapon,float excuteTime = 0.25f)
        {
            this.weapon = weapon;
            this.excuteTime = excuteTime;
        }

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            onAdded?.Invoke();
        }
        #endregion
    }
}
