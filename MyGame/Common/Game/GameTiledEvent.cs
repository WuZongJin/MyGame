using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Common.Game
{
    public enum GameHomeEvent
    {
        Null = 0,
        initWeaponBoxOpened = 1,        //初始房间内的宝箱是否被打开
        openTheWayToVillage = 2,        //炸开前往东庄小镇的路
        Cave2TreasureBoxOpened = 4,     //Cave2的宝箱是否被打开
        killShuChengCaveBoss = 8,       //是否打败了自家地窖里面的boss
    }

    public enum DongZhunagVillageEvent
    {
        Null= 0,
        ClearDongZhuangVillageCave = 1,         //通关东庄镇的迷宫
        AcceptVillageMayorTask = 2,             //接受了村长去洞穴救女儿
    }

    public enum DongZhuangMazeEvent
    {
        Null = 0,
        Maze1GateOpened = 1,                    //Maze1的大门是否被打开
        Maze2KeyHasGeted = 2,                   //Maze2的钥匙是否拿到了
        Maze6KeyHasGeted = 4,                   //Maze6的钥匙是否拿到了
        Maze5GateOpened = 8,                    //Maze5的大门是否打开
        PlayerGetArrow = 16,                    //玩家是否获得了弓箭
        MazeBossKilled = 32,                    //dzmazz的boss是否被杀死了
    }


    public static class GameEvent
    {
        public static GameHomeEvent homeEvent = GameHomeEvent.Null;
        public static DongZhunagVillageEvent dzVillageEvent = DongZhunagVillageEvent.Null;
        public static DongZhuangMazeEvent dzMazeEvent = DongZhuangMazeEvent.Null;
    }
}
