using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectPool : ObjectPool
{
    private static DamageEffectPool instance = null;
    public static DamageEffectPool GetInstance { get { return instance; } }

    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            onInstantiate = (GameObject _go) =>
            {
                _go.GetComponent<FadeOut>().onFadeOut += () =>
                {
                    ReturnObject(_go);
                };
            };
        }
        else
        {
            Destroy(gameObject);
        }

        base.Awake();
    }
}
