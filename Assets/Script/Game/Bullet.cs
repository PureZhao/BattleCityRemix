using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int bulletLevel = 0;
    private int playerID;
    private bool isPlayerBullet = false;
    private float moveSpeed = 5f;
    private Vector3 dir;
    public Direction direction;
    void Awake()
    {
        switch (direction)
        {
            case Direction.Up: dir = Vector3.up; break;
            case Direction.Right: dir = Vector3.right; break;
            case Direction.Left: dir = Vector3.left; break;
            case Direction.Down: dir = Vector3.down; break;
        }
    }

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * dir, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 pos = transform.position;
        if(other.TryGetComponent(out MapElement mapElement))
        {
            mapElement.GetHit(bulletLevel, pos);
        }
        else if(other.TryGetComponent(out PlayerControl playerControl))
        {
            if (isPlayerBullet)
            {
                playerControl.GetFriendHit();
            }
            else
            {
                playerControl.GetHit();
            }
        }
        else if(other.TryGetComponent(out Enemy enemy) && isPlayerBullet)
        {
            enemy.Dead(playerID);
        }
        AssetsManager.Instance.FreeObject(gameObject);
    }

    public void SetOwnPlayer(int player, int bulletRank)
    {
        isPlayerBullet = true;
        playerID = player;
        bulletLevel = bulletRank;
        if(bulletLevel >= 1)
        {
            moveSpeed = 7.5f;
        }
    }
}
