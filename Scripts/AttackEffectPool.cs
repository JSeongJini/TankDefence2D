using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectPool : ObjectPool
{
    private static AttackEffectPool instance = null;
    public static AttackEffectPool GetInstance { get { return instance; } }

    protected override void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            //이펙트가 초기화 될 때, 이펙트가 사라지면 풀로 되돌아가는 콜백 설정
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
