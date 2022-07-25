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

            //����Ʈ�� �ʱ�ȭ �� ��, ����Ʈ�� ������� Ǯ�� �ǵ��ư��� �ݹ� ����
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
