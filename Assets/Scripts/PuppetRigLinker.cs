using UnityEngine;

namespace FloatingPuppet {

public sealed class PuppetRigLinker : MonoBehaviour
{
    [field:SerializeField] public Transform Root { get; set; }
    [field:SerializeField] public Transform LeftHand { get; set; }
    [field:SerializeField] public Transform RightHand { get; set; }
    [field:SerializeField] public Transform LeftFoot{ get; set; }
    [field:SerializeField] public Transform RightFoot { get; set; }

    Animator _animator;

    void ApplyLink(AvatarIKGoal goal, Transform target)
    {
        _animator.SetIKPosition(goal, target.position);
        _animator.SetIKPositionWeight(goal, 1);
    }

    void Start()
      => _animator = GetComponent<Animator>();

    void LateUpdate()
    {
        transform.position = Root.position;
        transform.rotation = Root.rotation;
    }

    void OnAnimatorIK()
    {
        ApplyLink(AvatarIKGoal.LeftHand, LeftHand);
        ApplyLink(AvatarIKGoal.RightHand, RightHand);
        ApplyLink(AvatarIKGoal.LeftFoot, LeftFoot);
        ApplyLink(AvatarIKGoal.RightFoot, RightFoot);
    }
}

} // namespace FloatingPuppet
