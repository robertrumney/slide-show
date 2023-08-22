using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlideShow : MonoBehaviour
{
    [System.Serializable]
    public class Slide
    {
        public Sprite image;
        public AudioClip audioClip;
        public float triggerTime; 
    }

    public Image image1;
    public Image image2;
    public Slide[] slides;

    private bool isImage1Active = true;

    private IEnumerator SlideShowRoutine()
    {
        for (int i = 0; i < slides.Length; i++)
        {
            if (isImage1Active)
            {
                image1.sprite = slides[i].image;
                image1.canvasRenderer.SetAlpha(0f);
                image1.CrossFadeAlpha(1f, 1f, false);
                image2.canvasRenderer.SetAlpha(1f);
                image2.CrossFadeAlpha(0f, 1f, false);
            }
            else
            {
                image2.sprite = slides[i].image;
                image2.canvasRenderer.SetAlpha(0f);
                image2.CrossFadeAlpha(1f, 1f, false);
                image1.canvasRenderer.SetAlpha(1f);
                image1.CrossFadeAlpha(0f, 1f, false);
            }

            isImage1Active = !isImage1Active;

            if (slides[i].audioClip)
            {
                AudioSource.PlayClipAtPoint(slides[i].audioClip, transform.position);
            }

            yield return new WaitForSeconds(slides[i].triggerTime);
        }
    }

    private void Start()
    {
        StartCoroutine(SlideShowRoutine());
    }
}
