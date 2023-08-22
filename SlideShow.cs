using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class SlideShow : MonoBehaviour
{
    [System.Serializable]
    public class SlideEvent
    {
        [Tooltip("The slide to display")]
        public Sprite slide;

        [Tooltip("Duration (in seconds) this slide should be displayed after the previous slide")]
        public float duration;
    }

    [System.Serializable]
    public class AudioEvent
    {
        [Tooltip("The audio clip to play")]
        public AudioClip audioClip;

        [Tooltip("Duration (in seconds) to wait after the previous audio clip to play this one")]
        public float duration;
    }

    [Header("UI Elements")]
    [Tooltip("First image used for crossfade")]
    public Image image1;

    [Tooltip("Second image used for crossfade")]
    public Image image2;

    public List<SlideEvent> slideEvents = new List<SlideEvent>();
    public List<AudioEvent> audioEvents = new List<AudioEvent>();

    private bool isImage1Active = true;  // To keep track of which image is active for crossfading
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        image1.color = new Color(image1.color.r, image1.color.g, image1.color.b, 1);
        image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, 0);

        if (slideEvents.Count > 0)
        {
            StartSlideShow();
        }
    }

    private void StartSlideShow()
    {
        StartCoroutine(HandleSlideEvents());
        StartCoroutine(HandleAudioEvents());
    }

    private IEnumerator HandleSlideEvents()
    {
        foreach (var slideEvent in slideEvents)
        {
            yield return new WaitForSeconds(slideEvent.duration);

            if (isImage1Active)
            {
                image2.sprite = slideEvent.slide;
                StartCoroutine(Crossfade(image2, image1));
            }
            else
            {
                image1.sprite = slideEvent.slide;
                StartCoroutine(Crossfade(image1, image2));
            }

            // Toggle the active image
            isImage1Active = !isImage1Active;
        }
    }

    private IEnumerator HandleAudioEvents()
    {
        foreach (var audioEvent in audioEvents)
        {
            yield return new WaitForSeconds(audioEvent.duration);

            audioSource.clip = audioEvent.audioClip;
            audioSource.Play();
        }
    }

    private IEnumerator Crossfade(Image fadeInImage, Image fadeOutImage)
    {
        float duration = 1f;  // Assume a 1-second crossfade duration. Adjust as needed.
        float progress = 0f;

        while (progress < duration)
        {
            progress += Time.deltaTime;
            float normalizedProgress = progress / duration;  // Value between 0 and 1

            // Calculate the new alpha values
            float fadeInAlpha = Mathf.Lerp(0, 1, normalizedProgress);
            float fadeOutAlpha = Mathf.Lerp(1, 0, normalizedProgress);

            // Apply the new alpha values
            fadeInImage.color = new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, fadeInAlpha);
            fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, fadeOutAlpha);

            yield return null;
        }
    }

}
