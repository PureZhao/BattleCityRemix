using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class Quicker : Enemy
{
    //Set Default Property
    public override void SetDefaultProperty()
    {
        type = EnemyType.Quicker;
        curDir = Direction.Down;
        moveSpeed = 3f;
        shootInterval = 3f;
        shootTimer = 3f;
        changeDirTimer = 5f;
        changeDirInterval = 5f;
        stopTImer = 5f;
        stopInterval = 5f;
        isStop = false;
        curPicSet = 1;
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
