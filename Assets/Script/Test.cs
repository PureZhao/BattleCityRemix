using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using LitJson;
using Util;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        
    }

    IEnumerator TemplateCorountine<T>(string message)
    {
        yield return new WaitForSeconds(1f);
        Debug.Log(default(T));
        Debug.Log(message);
    }
    private void OnEnable()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(TemplateCorountine<int>("Pure"));
        }
    }

    
}
