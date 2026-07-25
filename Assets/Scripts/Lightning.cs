using UnityEngine;
using UnityEngine.Serialization;

public class Lightning : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [SerializeField] private string _blendParameter = "RandomValue";
    
    [SerializeField] private SimpleAudioEvent _audioEvent;

    public void Play()
    {
        _audioEvent.Play(transform);
        PlayRandom();
    }

    private void PlayRandom()
    {
        var randomValue = Random.Range(0f, 1f);
        _animator.SetFloat(_blendParameter, randomValue);
    }
}
