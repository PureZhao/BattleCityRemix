using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStar : MonoBehaviour
{
    private float timer = 0.2f;
    private bool isActive = true;
    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerControl player))
        {
            GameManager.Instance.EnemyKilled(player.PlayerID, EnemyType.Other);
            AudioManager.Play(ResConst.GetBonusSound);
            player.Upgrade();
            AssetsManager.Instance.FreeObject(gameObject);
        }
    }


    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (isActive)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0);
                isActive = false;
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
                isActive = true;
            }
            timer = 0.2f;
        }
    }
}
