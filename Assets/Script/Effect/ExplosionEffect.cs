using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : BaseEffect
{
    protected new IEnumerator RunEffect()
    {
        renderer.sprite = null;
        yield return new WaitForSeconds(0.05f);
        renderer.sprite = sprites[0];
        yield return new WaitForSeconds(0.05f);
        renderer.sprite = sprites[1];
        yield return new WaitForSeconds(0.05f);
        renderer.sprite = sprites[0];
        yield return new WaitForSeconds(0.05f);
        renderer.sprite = null;
    }
}
