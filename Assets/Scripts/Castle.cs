using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private GameObject _flagLeft;
    [SerializeField] private GameObject _flagRight;

    public void ShowLeftFlag()
    {
        _flagLeft.SetActive(true);
    }
    
    public void ShowRightFlag()
    {
        _flagRight.SetActive(true);
    }
}
