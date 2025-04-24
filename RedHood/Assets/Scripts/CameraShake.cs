using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originPos;

    [Header("카메라 쉐이크 설정")]
    public CinemachineImpulseSource impulseSource;

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (Camera.main == null)
        {
            yield break;
        }
        originPos = Camera.main.transform.localPosition;

        float elapsed = 0.0f;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x + x, originPos.y + y, -10);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originPos;
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }

    public void GenerateCameraImpulse()
    {
        if (impulseSource != null)
        {
            //Debug.Log("카메라 임펄스 발생");
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("ImpulseSource가 연결이 안되어있습니다.");
        }
    }

}
