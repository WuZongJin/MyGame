using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Common
{
    public static class CollisionSetting
    {
        #region Collision Tags
        


        public static Category wallCategory = Category.Cat1;            //墙壁
        public static Category enemyCategory = Category.Cat2;           //怪物
        public static Category playerCategory = Category.Cat3;          //玩家
        public static Category ItemsCategory = Category.Cat4;           //可拾取物品
        public static Category TriggerCategory = Category.Cat5;         //触发器
        public static Category npcCategory = Category.Cat6;             //npc
        public static Category tiledObjectCategory = Category.Cat7;     //地图中可交互的物体
        public static Category tiledHoleCategory = Category.Cat8;       //地图上的悬崖，掉落受伤


        public static Category playerAttackCategory = Category.Cat11;
        public static Category enemyAttackCategory = Category.Cat12;
        public static Category allAttackCategory = playerAttackCategory | enemyAttackCategory;

        public static Category expositionCategory = Category.Cat21;
        public static Category fireAttackCategory = Category.Cat22;
        public static Category posionAttackCategory = Category.Cat23;
        public static Category thumderAttackCategory = Category.Cat24;

        public static Category allActorCategory = enemyCategory | playerCategory | npcCategory;                 //所有的角色
        public static Category allTriggerAbleCategory = ItemsCategory | TriggerCategory | tiledObjectCategory;  //所有的可触发的物体
        public static Category allAttackTypeCategory = expositionCategory | fireAttackCategory | posionAttackCategory | thumderAttackCategory;  //所有攻击类型

        public static Category playerShouldTriggerCategory = wallCategory|tiledHoleCategory|allActorCategory|allAttackTypeCategory|allTriggerAbleCategory| enemyAttackCategory;
        #endregion

        #region 

        #endregion
    }
}
