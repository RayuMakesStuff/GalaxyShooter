using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float shakeDuration, float magnitude) 
    {
        var position = transform.position;
        Vector3 originalCameraPosition = new Vector3(position.x, position.y, position.z);

        float elapsedTime = 0.0f;

        while (elapsedTime < shakeDuration) 
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, originalCameraPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalCameraPosition;
    }
}
