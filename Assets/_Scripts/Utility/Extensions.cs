using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class Extensions
{
    public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector3 explosionPosition)
    {
        Vector3 direction = (rb.position - (Vector2)explosionPosition).normalized;

        rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
    }
    public static IEnumerator Shake(this Transform transform, float amplitude, float period)
    {
        Vector3 startPos = transform.position;
        while (period > 0)
        {
            transform.position = startPos + UnityEngine.Random.insideUnitSphere * amplitude;
            period -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.position = startPos;
        yield return null;
    }
    public static IEnumerator ShakeChild(this Transform transform, float amplitude, float period)
    {
        Vector3 startPos = transform.localPosition;
        while (period > 0)
        {
            transform.localPosition = startPos + UnityEngine.Random.insideUnitSphere * amplitude;
            period -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.localPosition = startPos;
        yield return null;
    }
    public static IEnumerator FreeShake(this Transform transform, float amplitude, float period)
    {
        while (period > 0)
        {
            transform.position = transform.position + UnityEngine.Random.insideUnitSphere * amplitude;
            period -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
    public static IEnumerator Blink(this SpriteRenderer spriteRenderer, float period, float delta)
    {
        while (period > 0)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            period -= delta;
            yield return new WaitForSeconds(delta);
        }
        spriteRenderer.enabled = true;
        yield return null;
    }
    public static IEnumerator TransitionToSceneAsync(this Scene scene, string sceneName, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (operation.isDone == false)
        {
            yield return null;
        }
    }
    public static IEnumerator TransitionToScene(this Scene scene, string sceneName, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(sceneName);
    }
    public static IEnumerator TransitionToScene(this Scene scene, string sceneName, float delay, Action<AsyncOperation> action)
    {
        yield return new WaitForSecondsRealtime(delay);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (operation.isDone == false)
        {
            action.Invoke(operation);
            yield return null;
        }
    }
    public static IEnumerator Flash(this Graphic graphic, Color color, float amplitude, float riseTime, float fallTime, int repetition)
    {
        graphic.color = color;
        while (repetition > 0)
        {
            graphic.canvasRenderer.SetAlpha(0);
            graphic.CrossFadeAlpha(amplitude, riseTime, true);
            yield return new WaitForSecondsRealtime(riseTime);
            graphic.CrossFadeAlpha(0, fallTime, true);
            yield return new WaitForSecondsRealtime(fallTime);
            repetition--;
        }
    }
    public static IEnumerator FlashInverse(this Graphic graphic, Color color, float amplitude, float riseTime, float fallTime, int repetition)
    {
        graphic.color = color;
        while (repetition > 0)
        {
            graphic.CrossFadeAlpha(1, 0, true);
            graphic.CrossFadeAlpha(amplitude, riseTime, true);
            yield return new WaitForSecondsRealtime(riseTime);
            graphic.CrossFadeAlpha(1, fallTime, true);
            yield return new WaitForSecondsRealtime(fallTime);
            repetition--;
        }
    }
    public static IEnumerator SpriteFlash(this SpriteRenderer spriteRenderer, float value, float riseTime, float fallTime, int repetition)
    {
        Color tempColor = spriteRenderer.color;
        Color startColor = spriteRenderer.color;
        startColor.a = 1;

        float increment;
        float delta;
        while (repetition > 0)
        {
            delta = 0;
            increment = Time.fixedDeltaTime / riseTime;
            while (delta <= 1)
            {
                tempColor.a = Mathf.Lerp(startColor.a, value, delta);
                spriteRenderer.color = tempColor;

                delta += increment;
                yield return new WaitForFixedUpdate();
            }

            delta = 0;
            increment = Time.fixedDeltaTime / fallTime;
            while (delta <= 1)
            {
                tempColor.a = Mathf.Lerp(value, startColor.a, delta);
                spriteRenderer.color = tempColor;

                delta += increment;
                yield return new WaitForFixedUpdate();
            }
            tempColor.a = 1;
            spriteRenderer.color = tempColor;

            repetition--;
        }
    }
    public static IEnumerator FadeIn(this Graphic graphic, Color color, float time, Action action = null)
    {
        graphic.color = color;
        graphic.CrossFadeAlpha(0, 0, true);
        graphic.CrossFadeAlpha(1, time, true);
        yield return new WaitForSecondsRealtime(time);
        action?.Invoke();
    }
    public static IEnumerator FadeOut(this Graphic graphic, Color color, float time, Action action = null)
    {
        graphic.color = color;
        graphic.CrossFadeAlpha(1, 0, true);
        graphic.CrossFadeAlpha(0, time, true);
        yield return new WaitForSecondsRealtime(time);
        action?.Invoke();
    }
    public static IEnumerator FadeInFromCurrent(this Graphic graphic, Color color, float time, Action action = null)
    {
        graphic.color = color;
        graphic.CrossFadeAlpha(1, time, true);
        yield return new WaitForSecondsRealtime(time);
        action?.Invoke();
    }
    public static IEnumerator FadeOutFromCurrent(this Graphic graphic, Color color, float time, Action action = null)
    {
        graphic.color = color;
        graphic.CrossFadeAlpha(0, time, true);
        yield return new WaitForSecondsRealtime(time);
        action?.Invoke();
    }
    public static IEnumerator FadeOut(this CanvasGroup cg, float time, Action action = null)
    {
        float elapsedTime = 0f;

        while(elapsedTime <= time)
        {
            cg.alpha = Mathf.Lerp(1, 0, elapsedTime / time);
            elapsedTime += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        cg.alpha = 0;
        action?.Invoke();
        yield return null;
    }
    public static IEnumerator FadeIn(this CanvasGroup cg, float time, Action action = null)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= time)
        {
            cg.alpha = Mathf.Lerp(0, 1, elapsedTime / time);
            elapsedTime += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        cg.alpha = 1;
        action?.Invoke();
        yield return null;
    }

    public static IEnumerator FadeInText(this TextMeshPro text, float time)
    {
        float elapsedTime = Mathf.InverseLerp(0, time, text.alpha);
        while(elapsedTime < time)
        {
            text.alpha = Mathf.Lerp(0, 1, elapsedTime / time);
            elapsedTime += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        text.alpha = 1;
    }
    public static IEnumerator FadeOutText(this TextMeshPro text, float time)
    {
        float elapsedTime = Mathf.InverseLerp(time, 0, text.alpha);
        while (elapsedTime < time)
        {
            text.alpha = Mathf.Lerp(1, 0, elapsedTime / time);
            elapsedTime += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        text.alpha = 0;
    }

    public static IEnumerator FadeIn(this SpriteRenderer spriteRenderer, float time)
    {
        float elapsedTime = Mathf.InverseLerp(0, time, spriteRenderer.color.a);
        Color tempColor = spriteRenderer.color;

        while (elapsedTime < time)
        {
            tempColor.a = Mathf.Lerp(0, 1, elapsedTime / time);
            spriteRenderer.color = tempColor;

            elapsedTime += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        tempColor.a = 1;
        spriteRenderer.color = tempColor;
    }
    public static IEnumerator FadeOut(this SpriteRenderer spriteRenderer, float time)
    {
        float elapsedTime = Mathf.InverseLerp(time, 0, spriteRenderer.color.a);
        Color tempColor = spriteRenderer.color;

        while (elapsedTime < time)
        {
            tempColor.a = Mathf.Lerp(1, 0, elapsedTime / time);
            spriteRenderer.color = tempColor;

            elapsedTime += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        tempColor.a = 0;
        spriteRenderer.color = tempColor;
    }

    public static IEnumerator RotateFromToAsync(this Transform transform, Quaternion targetRotation, float time, Action action = null)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < time)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / time);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.rotation = targetRotation;
        action?.Invoke();
    }
    public static IEnumerator MoveTransformAsync(this Transform transform, Vector3 targetPosition, float time, Action action = null)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / time);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.position = targetPosition;
        action?.Invoke();
    }
    public static Vector2 CalculateSymmetry(this Vector2 point, Vector2 axis)
    {
        float dotProduct = Vector2.Dot(axis, point - axis);
        float sx = 2 * dotProduct * axis.x - point.x;
        float sy = 2 * dotProduct * axis.y - point.y;
        return new Vector2(sx, sy);
    }
    public static Vector2 SymmetricPointToLine(this Vector2 point, Vector2 linePointStart, Vector2 linePointEnd)
    {
        Vector2 lineVector = linePointEnd - linePointStart;
        Vector2 pointVector = point - linePointStart;
        float dotProduct = Vector2.Dot(pointVector, lineVector.normalized);
        Vector2 projection = dotProduct * lineVector.normalized;
        Vector2 symmetricPoint = 2 * projection - pointVector;
        return symmetricPoint + linePointStart;
    }
    public static Vector2 RotateVector(this Vector2 vec, Vector3 targetDirection)
    {
        // Get the current direction of the vector
        Quaternion currentRotation = Quaternion.LookRotation(vec);

        // Get the target direction of the vector
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Calculate the rotation needed to go from the current direction to the target direction
        Quaternion deltaRotation = targetRotation * Quaternion.Inverse(currentRotation);

        // Apply the rotation to the vector and return the result
        return deltaRotation * vec;
    }

}


