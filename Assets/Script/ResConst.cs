using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ResConst
{
    private static string MainDir = "Assets/Res/";
    public static string AssetMapping {get;} = MainDir + "AssetMapping.json";
    public static string EnemyIDMapping {get;} = MainDir + "EnemyIDMapping.json";
    public static string MapStructure { get; } = MainDir + "MapStructure.json";

    private static string AudioDir = "Assets/Res/Audio/";
    public static string BonusAppearSound { get; } = AudioDir + "BonusAppear.mp3";
    public static string DieSound { get; } = AudioDir + "Die.aif";
    public static string ExplosionSound { get; } = AudioDir + "Explosion.wav";
    public static string FireSound { get; } = AudioDir + "Fire.mp3";
    public static string GameOverSound { get; } = AudioDir + "GameOver.mp3";
    public static string GameStartSound { get; } = AudioDir + "GameStart.wav";
    public static string GetBonusSound { get; } = AudioDir + "GetBonus.wav";
    public static string GrenadeSound { get; } = AudioDir + "Grenade.mp3";
    public static string HeartExplodeSound { get; } = AudioDir + "HeartExplode.mp3";
    public static string HitWallSound { get; } = AudioDir + "HitWall.mp3";
    public static string IronHitSound { get; } = AudioDir + "IronHit.wav";
    public static string LifeUpSound { get; } = AudioDir + "LifeUp.mp3";
    public static string MoveSound { get; } = AudioDir + "Move.mp3";
    public static string StandSound { get; } = AudioDir + "Stand.mp3";


    private static string ImageDir = "Assets/Res/Image/";

    public static string BonusImage { get; } = ImageDir + "Bonus.png";
    public static string BornImage { get; } = ImageDir + "Born.png";
    public static string BulletImage { get; } = ImageDir + "Bullet.png";
    public static string CurtainImage { get; } = ImageDir + "Curtain.jpg";
    public static string EnemysImage { get; } = ImageDir + "Enemys.png";
    public static string Explode1Image { get; } = ImageDir + "Explode1.png";
    public static string Explode2Image { get; } = ImageDir + "Explode2.png";
    public static string FlagImage { get; } = ImageDir + "Flag.png";
    public static string LimitionImage { get; } = ImageDir + "Limition.png";
    public static string MapImage { get; } = ImageDir + "Map.png";
    public static string Ocean2Image { get; } = ImageDir + "Ocean2.png";
    public static string Player1Image { get; } = ImageDir + "Player1.png";
    public static string Player2Image { get; } = ImageDir + "Player2.png";
    public static string SandLandImage { get; } = ImageDir + "SandLand.png";
    public static string ShieldImage { get; } = ImageDir + "Shield.png";
    public static string TitleImage { get; } = ImageDir + "Title.bmp";
    public static string UIGameOverImage { get; } = ImageDir + "UIGameOver.png";
    public static string UIViewImage { get; } = ImageDir + "UIView.png";

    public static string LevelConfigDir { get; } = "Assets/Res/LevelConfig/";

    private static string BonusDir = "Assets/Res/Prefab/Bonus/";
    public static List<string> Tools { get; } = new List<string>()
    {
        BonusDir + "AddLife.prefab",
        BonusDir + "DestroyEnemy.prefab",
        BonusDir + "ProtectHeart.prefab",
        BonusDir + "Shield.prefab",
        BonusDir + "StopEnemy.prefab",
        BonusDir + "Upgrade.prefab"
    };

    private static string BulletDir = "Assets/Res/Prefab/Bullet/";
    public static Dictionary<Direction, string> Bullet { get; } = new Dictionary<Direction, string>()
    {
        {Direction.Up,  BulletDir + "Up.prefab"},
        {Direction.Right,  BulletDir + "Right.prefab"},
        {Direction.Down,  BulletDir + "Down.prefab"},
        {Direction.Left,  BulletDir + "Left.prefab"},
    };

    private static string MapEditorDir = "Assets/Res/Prefab/MapEditor/";
    public static string MapEditor { get; } = MapEditorDir + "MapEditor.prefab";

    private static string EffectDir = "Assets/Res/Prefab/Effect/";
    public static string BornEffect {get;} = EffectDir + "Born.prefab";
    public static string BulletExplosionEffect {get;} = EffectDir + "BulletExplosion.prefab";
    public static string EnemyImageEffect {get;} = EffectDir + "EnemyImage.prefab";
    public static string ShieldEffect {get;} = EffectDir + "Shield.prefab";
    public static string TankExplosionEffect { get; } = EffectDir + "TankExplosion.prefab";

    private static string EnemyDir = "Assets/Res/Prefab/Enemy/";

    public static List<string> EnemyPrefab { get; } = new List<string>()
    {
        EnemyDir + "Normal.prefab",
        EnemyDir + "NormalTool.prefab",
        EnemyDir + "Quicker.prefab",
        EnemyDir + "QuickerTool.prefab",
        EnemyDir + "Heavyer.prefab",
        EnemyDir + "HeavyerTool.prefab",
        EnemyDir + "PretenderYellow.prefab",
        EnemyDir + "PretenderGreen.prefab"
    };

    private static string MapElementDir = "Assets/Res/Prefab/MapElement/";
    public static string BrickMapElement {get;} = MapElementDir + "Brick.prefab";
    public static string BrokenHeartMapElement {get;} = MapElementDir + "BrokenHeart.prefab";
    public static string GrassMapElement {get;} = MapElementDir + "Grass.prefab";
    public static string HeartMapElement {get;} = MapElementDir + "Heart.prefab";
    public static string IronMapElement {get;} = MapElementDir + "Iron.prefab";
    public static string OceanMapElement {get;} = MapElementDir + "Ocean.prefab";
    public static string SandLandMapElement { get; } = MapElementDir + "SandLand.prefab";

    private static string PlayerDir = "Assets/Res/Prefab/Player/";
    public static string Player1 {get;} = PlayerDir + "Player1.prefab";
    public static string Player2 { get; } = PlayerDir + "Player2.prefab";

    private static string UIDir = "Assets/Res/Prefab/UI/";
    public static string UIMenuCanvas { get; } = UIDir + "MenuCanvas.prefab";
    public static string UIEnemyIcon { get; } = UIDir + "EnemyIcon.prefab";
}
