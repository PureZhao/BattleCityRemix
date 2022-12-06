using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class Heavyer : Enemy
{
    private int life = 3;
    public Sprite[] level11;
    public Sprite[] level12;
    public Sprite[] level21;
    public Sprite[] level22;

    //Set Default Property
    public override void SetDefaultProperty()
    {
        type = EnemyType.Heavyer;
        curDir = Direction.Down;
        moveSpeed = 1f;
        shootInterval = 3f;
        shootTimer = 3f;
        changeDirTimer = 5f;
        changeDirInterval = 5f;
        stopTImer = 5f;
        stopInterval = 5f;
        isStop = false;
        life = 3;
        curPicSet = 1;
    }
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDefaultProperty();
    }

    void Update()
    {
        if (isStop)
        {
            stopTImer -= Time.fixedDeltaTime;
            if (stopTImer < 0)
            {
                isStop = false;
                stopTImer = stopInterval;
            }
            return;
        }
        shootTimer -= Time.fixedDeltaTime;
        changeDirTimer -= Time.fixedDeltaTime;
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
        life--;
        if (life == 2) {
            AudioManager.Play(ResConst.IronHitSound);
            curSprite1 = level11;
            curSprite2 = level12;
            return;
        }
        if (life == 1)
        {
            AudioManager.Play(ResConst.IronHitSound);
            curSprite1 = level21;
            curSprite2 = level22;
            return;
        }
        AssetsManager.Instance.LoadGameObject(ResConst.TankExplosionEffect, (obj) =>
        {
            obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        });
        GameManager.Instance.EnemyKilled(val, type);
        AssetsManager.Instance.FreeObject(gameObject);
    }
}
