using UnityEngine;

namespace FloatingPuppet {

public sealed class PuppetLookAtLinker : MonoBehaviour
{
    [field:SerializeField] public Transform Target { get; set; }

    Animator _animator;

    void Start()
      => _animator = GetComponent<Animator>();

    void OnAnimatorIK()
    {
        _animator.SetLookAtPosition(Target.position);
        _animator.SetLookAtWeight(1, 0.2f, 0.8f);
    }
}

} // namespace FloatingPuppet
