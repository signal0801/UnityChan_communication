using UnityEngine;
using System.Collections;

public delegate void UserLookUnityChan (float duration);
public delegate void UserLookOutUnityChan ();

public class PlayerSight : MonoBehaviour
{
    float distance = 10.0f;
    [SerializeField]
    float lookTime = 1.5f;
    public UnitychanAnimationControll unitychanAnimationControll;
    public LayerMask characterLayer;

    /// <summary>
    /// ユーザーの注視点にUnityちゃんが一定時間居たら発火する
    /// </summary>
    public event UserLookUnityChan UserLookUnityChan;

    /// <summary>
    /// ユーザーがUnityちゃんの注視を外したら呼び出される
    /// </summary>
    public event UserLookOutUnityChan UserLookOutUnityChanEvent;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine ("CO_MonitorLookUnityChan");
    }

    /// <summary>
    /// 無限ループでUnityちゃんをトラッキングし続けるコルーチン
    /// </summary>
    /// <returns>The o_ monitor look unity chan.</returns>
    IEnumerator CO_MonitorLookUnityChan ()
    {
        while (true) {

            yield return null;

            if (Physics.Raycast (transform.position, transform.forward, distance, characterLayer)) {
                StartCoroutine("CO_StartLookingUnityChan");
            } else {
                StopCoroutine ("CO_StartLookingUnityChan");
                Debug.Log ("Fire UserLookOutUnityChanEvent");
                if (UserLookOutUnityChanEvent != null) {
                    UserLookOutUnityChanEvent ();
                }
            }

        }

    }

    IEnumerator CO_StartLookingUnityChan()
    {
        yield return new WaitForSeconds (lookTime);

        Debug.Log ("Fire UserLookUnityChan");
        if (UserLookUnityChan != null) {
            UserLookUnityChan (lookTime);
        }
    }

}
