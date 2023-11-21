using UnityEngine;
using Unity.Mathematics;

namespace FloatingPuppet {

public sealed class PuppetSpineLinker : MonoBehaviour
{
    [field:SerializeField] public Transform Target { get; set; }

    Animator _animator;

    void Start()
      => _animator = GetComponent<Animator>();

    void OnAnimatorIK()
    {
        var rot = math.slerp(quaternion.identity, Target.localRotation, 1.0f / 3);
        _animator.SetBoneLocalRotation(HumanBodyBones.Spine, rot);
        _animator.SetBoneLocalRotation(HumanBodyBones.Chest, rot);
        _animator.SetBoneLocalRotation(HumanBodyBones.UpperChest, rot);
    }
}

} // namespace FloatingPuppet
