using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlayerDialogueMessages : MonoBehaviour
{
    //Variables
    #region Text settings
    [Header("Text settings")]
    public GameObject messagePrefab;
    public Transform messagePosition;
    #endregion

    #region Audio Settings
    [Header("Audio settings")]
    public AudioClip letterSound;
    public AudioSource source;
    #endregion

    #region Hide Variables
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI textUI;
    private Coroutine coroutine;
    private GameObject currentMessageObject; // Para trackear el mensaje actual
    #endregion

    //Functions

    /// <summary>
    /// FadeIn independiente: Revela el mensaje letra a letra (para OnPointerEnter)
    /// </summary>
    public void FadeIn(string message, float fadeInDuration, float letterSpeed, List<ColorWords> colorwords, GameObject reuseObject = null)
    {
        if (coroutine != null) 
        {
            textUI.text = string.Empty;
            StopCoroutine(coroutine); 
        }
        coroutine = StartCoroutine(FadeInCoroutine(message, fadeInDuration, letterSpeed, colorwords, reuseObject));
    }

    /// <summary>
    /// FadeOut independiente: Oculta y destruye (para OnPointerExit)
    /// </summary>
    public void FadeOut(float fadeOutDuration, float stayDuration = 0f)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(FadeOutCoroutine(fadeOutDuration, stayDuration));
    }

    /// <summary>
    /// Metodo que invoca el fade in out del mensaje
    /// </summary>
    /// <param name="message"></param>
    public void FadeInOutInvoke(string message, float fadeInDuration, float fadeOutDuration, float stayDuration, float letterSpeed, List<ColorWords> colorwords)
    {
        if (coroutine == null) 
        {
            coroutine = StartCoroutine(FadeInOut(message, fadeInDuration, fadeOutDuration, stayDuration, letterSpeed, colorwords));
        }
    }

    string ApplyCustomColorsPerWord(string msg, List<ColorWords> colorWords)
    {
        string result = msg;
        foreach (ColorWords cw in colorWords)
        {
            if (cw.word == null || cw.word == "") continue;
            string hex = ColorUtility.ToHtmlStringRGBA(cw.color);
            string coloredWord = $"<color=#{hex}>{cw.word}</color>";
            result = result.Replace(cw.word, coloredWord);  // Reemplaza todas las ocurrencias
        }
        return result;
    }

    IEnumerator FadeInCoroutine(string message, float fadeInDuration, float letterSpeed, List<ColorWords> colorwords, GameObject _object = null)
    {
        // Crea o reutiliza objeto
        if (_object != null)
        {
            currentMessageObject = _object;
        }
        else
        {
            currentMessageObject = Instantiate(messagePrefab, messagePosition);
            currentMessageObject.transform.position = messagePosition.position;
        }
        canvasGroup = currentMessageObject.GetComponent<CanvasGroup>();
        textUI = currentMessageObject.GetComponentInChildren<TextMeshProUGUI>();

        // Fade in alpha
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Typewriter effect
        textUI.richText = true;

        message = ApplyCustomColorsPerWord(message, colorwords);

        textUI.text = message;
        textUI.maxVisibleCharacters = 0;

        for (int i = 0; i < message.Length; i++)
        {
            textUI.maxVisibleCharacters = i + 1;
            source.PlayOneShot(letterSound);
            yield return new WaitForSeconds(letterSpeed);
        }
    }

    IEnumerator FadeOutCoroutine(float fadeOutDuration, float stayDuration, bool destroyObject = false)
    {
        // Opcional: espera visible
        if (stayDuration > 0) yield return new WaitForSeconds(stayDuration);

        // Fade out
        if (canvasGroup != null)
        {
            float timer = 0f;
            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

        // Limpia
        if (currentMessageObject != null && destroyObject)
        {
            Destroy(currentMessageObject);
            currentMessageObject = null;
        }
        textUI.text = string.Empty;
        canvasGroup = null;
        textUI = null;
        coroutine = null;
    }


    /// <summary>
    /// Enumerator que realiza toda la funcion de la animacion del mensaje
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    IEnumerator FadeInOut(string message, float fadeInDuration, float fadeOutDuration, float stayDuration, float letterSpeed, List<ColorWords> colorwords, GameObject _object = null)
    {
        GameObject g;
        if (_object)
        {
            g = _object;
        }
        else 
        {
            g = Instantiate(messagePrefab, messagePosition);

        }
        g.transform.position = messagePosition.position;
        canvasGroup = g.GetComponent<CanvasGroup>();
        textUI = g.GetComponentInChildren<TextMeshProUGUI>();

        // Fade in
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        textUI.richText = true;
        message = ApplyCustomColorsPerWord(message, colorwords);
        textUI.text = message;  // String completo con tags YA
        textUI.maxVisibleCharacters = 0;  // Oculta todo

        for (int i = 0; i < message.Length; i++)
        {
            textUI.maxVisibleCharacters = i + 1;  // Revela hasta i
            source.PlayOneShot(letterSound);
            yield return new WaitForSeconds(letterSpeed);
        }

        // Espera un tiempo visible
        yield return new WaitForSeconds(stayDuration);

        // Fade out
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        Destroy(g);
        coroutine = null;
    }

}

[System.Serializable]
public class ColorWords 
{
    public Color color;
    public string word;

    public ColorWords(Color _color, string _word) 
    {
        color = _color;
        word = _word;
    }
}