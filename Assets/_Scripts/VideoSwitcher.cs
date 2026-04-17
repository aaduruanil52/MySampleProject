using UnityEngine;
using UnityEngine.Video;
using System.Collections;
public class VideoSwitcher : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    private int currentIndex = 0;

    public AudioSource audioplayer;
    public AudioClip[] AudioClips;

    public Renderer targetRenderer;
    public Texture loadingTexture;
    public RenderTexture videoRenderTexture;
    public GameObject logoBG;

    void Start()
    {
        logoBG.SetActive(false);

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoRenderTexture;

        targetRenderer.material.mainTexture = loadingTexture;

        StartCoroutine(PlayClip(currentIndex));
    }

    public void PlayNext()
    {
        StopCoroutine(PlayAudio());
        audioplayer.Stop();
        currentIndex = (currentIndex + 1) % videoClips.Length;
        StartCoroutine(PlayClip(currentIndex));
    }

    public void PlayPrevious()
    {
        StopCoroutine(PlayAudio());
        audioplayer.Stop();
        currentIndex = (currentIndex - 1 + videoClips.Length) % videoClips.Length;
        StartCoroutine(PlayClip(currentIndex));
    }

    IEnumerator PlayClip(int index)
    {
        logoBG.SetActive(true);

        targetRenderer.material.mainTexture = loadingTexture;

        videoPlayer.clip = videoClips[index];
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return null;

        targetRenderer.material.mainTexture = videoRenderTexture;

        logoBG.SetActive(true);
        videoPlayer.Play();
        yield return new WaitForSeconds(.5f);
        logoBG.SetActive(false);
        StartCoroutine(PlayAudio());
    }

    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(1.5f);
        audioplayer.clip = AudioClips[currentIndex];
        audioplayer.Play();
    }
}
