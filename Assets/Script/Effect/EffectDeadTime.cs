using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class EffectDeadTime : MonoBehaviour
{
    public float time = 0.1f;
    void Start()
    {
        Destroy(gameObject, time);
    }
}
