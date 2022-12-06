using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    protected SpriteRenderer renderer;
    public bool undead = false;
    public float deadTime = 0f;
    public float switchTime = 0.2f;
    public Sprite[] sprites;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        if (sprites.Length > 0)
        {
            StartCoroutine(nameof(RunEffect));
        }
    }

    private void Start()
    {
        if (!undead)
        {
            Destroy(gameObject, deadTime);
        }
    }

    protected virtual IEnumerator RunEffect()
    {
        int count = sprites.Length;
        WaitForSeconds wait = new WaitForSeconds(switchTime);
        for(int i = 0; i < count; i++)
        {
            renderer.sprite = sprites[i];
            yield return wait;
            if(i + 1 == count)
            {
                i = -1;
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


}
