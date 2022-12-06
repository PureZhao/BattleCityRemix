using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public enum GameMode
    {
        Single = 0,
        Double,
        CustomerConstruct,
    }

    public enum Direction
    {
        Up = 0,
        Right,
        Down,
        Left,
    }

    public enum UIUpdateType
    {
        OnRoundStart,
        PlayerLifeChange,
        EnemyInit,
        EnemySpawn,
        EnemyKilled,
        LevelChange,
    }

    public enum EnemyType
    {
        //N Q P H
        Normal = 0,
        Quicker = 1,
        Pretender = 2,
        Heavyer = 3,
        Other = 4,
    }

    public enum MapElementType
    {
        Brick,
        Iron,
        Heart,
        BrokenHeart,
        Grass,
        SandLand,
        Ocean,
        AirWall,
    }

}