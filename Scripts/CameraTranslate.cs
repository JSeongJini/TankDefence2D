using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTranslate : MonoBehaviour
{
    private Vector3 userCampOffset = new Vector3(-0.5f, -0.5f, -10f);
    private Vector3 enemyCampOffset = new Vector3(-18.5f, -0.5f, -10f);
    private Vector3 upgradeOffset = new Vector3(17.5f, -0.5f, -10f);

    private Vector3 curOffset;

    private void Awake()
    {
        curOffset = userCampOffset;
    }

    public void GoToEnemyCamp()
    {
        StopCoroutine("SmoothTranslate");
        StartCoroutine("SmoothTranslate", enemyCampOffset);
    }

    public void GoToUserCamp()
    {
        StopCoroutine("SmoothTranslate");
        StartCoroutine("SmoothTranslate", userCampOffset);
    }

    public void GoToUpgrade()
    {
        StopCoroutine("SmoothTranslate");
        StartCoroutine("SmoothTranslate", upgradeOffset);
    }

    private IEnumerator SmoothTranslate(Vector3 _nextOffset)
    {
        float elapsed = 0f;

        while(elapsed <= 1f)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(curOffset, _nextOffset, elapsed);
            yield return null;
        }
        transform.position = _nextOffset;
        curOffset = _nextOffset;
    }
}
