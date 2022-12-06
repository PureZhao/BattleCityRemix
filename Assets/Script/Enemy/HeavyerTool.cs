using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyerTool : Enemy
{

    private float redSpriteTimer = 0.2f;
    private float redSpriteInterval = 0.2f;

    private int life = 4;

    public Sprite[] level11;
    public Sprite[] level12;
    public Sprite[] level21;
    public Sprite[] level22;
    public Sprite[] level31;
    public Sprite[] level32;
    //Set Default Property
    public override void SetDefaultProperty()
    {
        type = EnemyType.Heavyer;
        curDir = GameCore.Direction.Down;
        moveSpeed = 1f;
        shootInterval = 3f;
        shootTimer = 3f;
        changeDirTimer = 5f;
        changeDirInterval = 5f;
        redSpriteTimer = 0.2f;
        redSpriteInterval = 0.2f;
        stopTImer = 5f;
        stopInterval = 5f;
        isStop = false;
        life = 4;
        curPicSet = 1;
    }
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDefaultProperty();
    }

    void FixedUpdate()
    {
        if (life == 4)
        {
            RedSprite();
        }
        else
        {
            SwitchSprite();
        }
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
        if (shootTimer < 0f)
        {
            shootTimer = shootInterval;
            Shoot();
        }
    }

    //Switch Sprite
    void RedSprite() {
        int index = (int)curDir;
        redSpriteTimer -= Time.fixedDeltaTime;
        if (redSpriteTimer < 0 && curPicSet == 1) {
            spriteRenderer.sprite = curSprite1[index];
            curPicSet++;
            redSpriteTimer = redSpriteInterval;
        }
        else if (redSpriteTimer < 0 && curPicSet == 2) {
            spriteRenderer.sprite = curSprite2[index];
            curPicSet--;
            redSpriteTimer = redSpriteInterval;
        }
    }

    //Get Damage
    public override void Dead(int val)
    {
        life--;
        if (life == 3)
        {
            AudioManager.Play(ResConst.IronHitSound);

            MapManager.Instance.CreateTool();
            curSprite1 = level11;
            curSprite2 = level12;
            return;
        }
        if (life == 2)
        {
            AudioManager.Play(ResConst.IronHitSound);
            curSprite1 = level21;
            curSprite2 = level22;
            return;
        }
        if (life == 1)
        {
            AudioManager.Play(ResConst.IronHitSound);
            curSprite1 = level31;
            curSprite2 = level32;
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
