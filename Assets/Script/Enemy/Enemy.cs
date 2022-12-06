using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class Enemy : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected EnemyType type = EnemyType.Normal;
    protected Direction curDir = Direction.Down;
    protected float moveSpeed = 3f;
    protected float shootInterval = 3f;
    protected float shootTimer = 3f;
    protected float changeDirTimer = 5f;
    protected float changeDirInterval = 5f;

    protected float stopTImer = 5f;
    protected float stopInterval = 5f;
    protected bool isStop = false;

    protected int curPicSet = 1;
    public Sprite[] curSprite1;
    public Sprite[] curSprite2;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDefaultProperty();
    }

    public virtual void SetDefaultProperty()
    {

    }


    public virtual void Move()
    {
        if (curDir == Direction.Up)
        {
            transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
        else if (curDir == Direction.Right)
        {
            transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
        else if (curDir == Direction.Down)
        {
            transform.Translate(-transform.up * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
        else if (curDir == Direction.Left)
        {
            transform.Translate(-transform.right * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
    }

    //Change Direction
    public virtual void ChangeDirection()
    {
        int t = Random.Range(0, 100);
        if (t < 10)
        {
            curDir = Direction.Up;
            return;
        }
        if (t >= 75)
        {
            curDir = Direction.Right;
            return;
        }
        if (t >= 10 && t <= 50)
        {
            curDir = Direction.Down;
            return;
        }
        if (t > 50 && t < 75)
        {
            curDir = Direction.Left;
            return;
        }
    }

    //Switch Sprite
    public virtual void SwitchSprite()
    {
        int index = (int)curDir;
        if (curPicSet == 1)
        {
            spriteRenderer.sprite = curSprite1[index];
            curPicSet++;
        }
        else if (curPicSet == 2)
        {
            spriteRenderer.sprite = curSprite2[index];
            curPicSet--;
        }
    }

    //Shoot
    public virtual void Shoot()
    {
        AudioManager.Play(ResConst.FireSound);
        string bulletAssetPath = ResConst.Bullet[curDir];
        Vector3 bulletSpawnPos = Vector3.zero;
        switch (curDir)
        {
            case Direction.Up: bulletSpawnPos = transform.position + transform.up * 0.3f; break;
            case Direction.Right: bulletSpawnPos = transform.position + transform.right * 0.3f; break;
            case Direction.Down: bulletSpawnPos = transform.position - transform.up * 0.3f; break;
            case Direction.Left: bulletSpawnPos = transform.position - transform.right * 0.3f; break;
        }
        AssetsManager.Instance.LoadGameObject(bulletAssetPath, (obj) => {
            obj.transform.SetPositionAndRotation(bulletSpawnPos, Quaternion.identity);
        });
    }

    //Stop
    public virtual void StopMove()
    {
        isStop = true;
    }

    public virtual void Dead(int val)
    {

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
