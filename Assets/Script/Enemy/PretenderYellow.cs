using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PretenderYellow : Enemy
{
    public Sprite[] level11;
    public Sprite[] level12;
    public Sprite[] level21;
    public Sprite[] level22;
    public Sprite[] level31;
    public Sprite[] level32;
    public Sprite[] level41;
    public Sprite[] level42;
    //Set Default Property
    public override void SetDefaultProperty()
    {
        type = EnemyType.Pretender;
        curDir = GameCore.Direction.Down;
        moveSpeed = 1.5f;
        shootInterval = 3f;
        shootTimer = 3f;
        changeDirTimer = 5f;
        changeDirInterval = 5f;
        stopTImer = 5f;
        stopInterval = 5f;
        isStop = false;
        curPicSet = 1;
    }
    void Awake()
    {
        SetDefaultProperty();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in players) {
            if (g.GetComponent<PlayerControl>().PlayerID == 1) {
                int curLevel = g.GetComponent<PlayerControl>().Level;
                switch (curLevel) {
                    case 0: curSprite1 = level11; curSprite2 = level12; return;
                    case 1: curSprite1 = level21; curSprite2 = level22; return;
                    case 2: curSprite1 = level31; curSprite2 = level32; return;
                    case 3: curSprite1 = level41; curSprite2 = level42; return;
                }
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (isStop)
        {
            stopTImer -= Time.deltaTime;
            if (stopTImer < 0)
            {
                isStop = false;
                stopTImer = stopInterval;
            }
            return;
        }
        shootTimer -= Time.deltaTime;
        changeDirTimer -= Time.deltaTime;
        if (changeDirTimer < 0)
        {
            changeDirTimer = changeDirInterval;
            ChangeDirection();
        }
        Move();
        SwitchSprite();
        if (shootTimer < 0f)
        {
            shootTimer = shootInterval;
            Shoot();
        }
    }
    
    //Get Damage
    public override void Dead(int val)
    {
        AssetsManager.Instance.LoadGameObject(ResConst.TankExplosionEffect, (obj) =>
        {
            obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        });
        GameManager.Instance.EnemyKilled(val, type);
        AssetsManager.Instance.FreeObject(gameObject);
    }
}
