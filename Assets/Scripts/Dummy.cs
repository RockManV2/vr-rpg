
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private Animator _dummyAnimator;

    private void Start()
    {
        _dummyAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _dummyAnimator.Play("pushed");
    }
}
