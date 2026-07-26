using UnityEngine;

public class Angel : MonoBehaviour
{
    private static readonly int Level = Animator.StringToHash("Level");
    [SerializeField] private Animator _animator;

    public void SetLevel(int level)
    {
        _animator.SetInteger(Level, level);
    }
}
