using UnityEngine;
using Unity.Mathematics;

namespace FloatingPuppet {

public sealed class PuppetSpineLinker : MonoBehaviour
{
    [field:SerializeField] public Transform Target1 { get; set; }
    [field:SerializeField] public Transform Target2 { get; set; }
    [field:SerializeField] public Transform Target3 { get; set; }

    Animator _animator;

    void Start()
      => _animator = GetComponent<Animator>();

    void OnAnimatorIK()
    {
        _animator.SetBoneLocalRotation(HumanBodyBones.Spine, Target1.localRotation);
        _animator.SetBoneLocalRotation(HumanBodyBones.Chest, Target2.localRotation);
        _animator.SetBoneLocalRotation(HumanBodyBones.UpperChest, Target3.localRotation);
    }
}

} // namespace FloatingPuppet
