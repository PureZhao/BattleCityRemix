using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElement : MonoBehaviour
{
    public bool isDamageable = false;
    public MapElementType type;
    private int maxBulletLevel;
    private void Awake()
    {
        switch (type)
        {
            case MapElementType.Iron: maxBulletLevel = 3; break;
            case MapElementType.AirWall: maxBulletLevel = 9999; break;
            case MapElementType.Brick:
            case MapElementType.Heart:
                maxBulletLevel = 0;
                break;
        }
    }

    public void GetHit(int bulletLevel, Vector2 bulletPos)
    {
        if (!isDamageable)
            return;
        AssetsManager.Instance.LoadGameObject(ResConst.BulletExplosionEffect, (obj) =>
        {
            obj.transform.SetPositionAndRotation(bulletPos, Quaternion.identity);
        });

        if (type == MapElementType.Iron && bulletLevel < maxBulletLevel)
        {
            AudioManager.Play(ResConst.IronHitSound);
        }
        if (type == MapElementType.AirWall)
        {
            AudioManager.Play(ResConst.HitWallSound);
        }
        if(type == MapElementType.Heart)
        {
            AudioManager.Play(ResConst.HeartExplodeSound);
            AssetsManager.Instance.LoadGameObject(ResConst.BrokenHeartMapElement, (obj) =>
            {
                obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            });
            AssetsManager.Instance.LoadGameObject(ResConst.TankExplosionEffect, (obj) =>
            {
                obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            });
            GameManager.Instance.StartCoroutine("RoundFail");
        }

        if (bulletLevel >= maxBulletLevel)
        {
            AssetsManager.Instance.FreeObject(gameObject);
        }
    }

}
