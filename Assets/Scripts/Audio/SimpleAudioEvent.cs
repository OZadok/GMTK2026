using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class 
    SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    public RangedFloat volume = new RangedFloat(0.95f, 1f);

    [MinMaxRange(0, 2)]
    public RangedFloat pitch = new RangedFloat(0.95f, 1.05f);

    /// <summary>
    ///   <para>Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D.</para>
    /// </summary>
    [Range(0,1)]
    public float spatialBlend;

    public AudioMixerGroup audioGroup;

    public override void Play(Vector3 position = new Vector3())
    {
        if (clips.Length == 0) return;

        var audioPlayer = new GameObject("audioPlayer", typeof(AudioSource)).GetComponent<AudioSource>();
        audioPlayer.transform.position = position;
        audioPlayer.outputAudioMixerGroup = audioGroup;

        Play(audioPlayer);

        Destroy(audioPlayer.gameObject, audioPlayer.clip.length * audioPlayer.pitch);
    }
    
    public override void Play(Transform parent)
    {
        if (clips.Length == 0) return;

        var audioPlayer = new GameObject("audioPlayer", typeof(AudioSource)).GetComponent<AudioSource>();
        audioPlayer.outputAudioMixerGroup = audioGroup;
        Transform transform;
        (transform = audioPlayer.transform).SetParent(parent);
        transform.localPosition = Vector3.zero;

        Play(audioPlayer);

        Destroy(audioPlayer.gameObject, audioPlayer.clip.length * audioPlayer.pitch);
    }

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0) return;

        if (source == null)
        {
            var audioPlayer = new GameObject("audioPlayer", typeof(AudioSource)).GetComponent<AudioSource>();
            audioPlayer.transform.position = new Vector3();
            audioPlayer.outputAudioMixerGroup = audioGroup;

            audioPlayer.clip = clips[Random.Range(0, clips.Length)];
            audioPlayer.volume = Random.Range(volume.minValue, volume.maxValue);
            audioPlayer.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.spatialBlend = spatialBlend;
            audioPlayer.Play();

            Destroy(audioPlayer.gameObject, audioPlayer.clip.length * audioPlayer.pitch);
        }

        else
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.volume = Random.Range(volume.minValue, volume.maxValue);
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.spatialBlend = spatialBlend;
            source.Play();
        }
    }

    public override void PlatUISound()
    {
        Play();
    }
}