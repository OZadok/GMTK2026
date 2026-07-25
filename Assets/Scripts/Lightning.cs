using UnityEngine;
using UnityEngine.Serialization;

public class Lightning : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [SerializeField] private string _blendParameter = "RandomValue";

    public void Play()
    {
        PlayRandom();
    }

    private void PlayRandom()
    {
        var randomValue = Random.Range(0f, 1f);
        _animator.SetFloat(_blendParameter, randomValue);
    }
}
