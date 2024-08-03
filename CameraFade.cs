using UnityEngine;
using System.Collections;

public class CameraFade : MonoBehaviour
{
    public Material fadeMaterial;
    private float fadeAlpha = 0;

    private void OnGUI()
    {
        if (fadeAlpha > 0)
        {
            Color color = new Color(0, 0, 0, fadeAlpha);
            fadeMaterial.color = color;
            Graphics.Blit(null, fadeMaterial);
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            fadeAlpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeAlpha = 0;
    }

    public IEnumerator FadeOut(float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            fadeAlpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeAlpha = 1;
    }
}
