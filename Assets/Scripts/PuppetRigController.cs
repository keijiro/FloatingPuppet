using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

namespace FloatingPuppet {

public sealed class PuppetRigController : MonoBehaviour
{
    [field:SerializeField] public Transform Root { get; set; }
    [field:SerializeField] public uint Seed { get; set; }

    Animator _animator;

    (Transform xform, float3 pos, quaternion rot)
      _root, _handL, _handR, _footL, _footR;

    (Transform xform, float3 pos, quaternion rot) GetTarget(Transform xform)
      => (xform, xform.localPosition, xform.localRotation);

    (Transform xform, float3 pos, quaternion rot) FindTarget(string name)
      => GetTarget(transform.Find(name));

    void UpdateTarget
      ((Transform xform, float3 pos, quaternion rot) target, uint seed)
    {
        var t = Time.time;
        target.xform.localPosition = target.pos + Noise.Float3(t, seed++);
        target.xform.localRotation = math.mul(target.rot, Noise.Rotation(t, math.PI, seed++));
    }

    void Start()
    {
        _animator = Root.GetComponent<Animator>();
        _root = GetTarget(transform);
        _handL = FindTarget("Left Hand");
        _handR = FindTarget("Right Hand");
        _footL = FindTarget("Left Foot");
        _footR = FindTarget("Right Foot");
    }

    void LateUpdate()
    {
        var seed = Seed;
        UpdateTarget(_root, seed++);
        UpdateTarget(_handL, seed++);
        UpdateTarget(_handR, seed++);
        UpdateTarget(_footL, seed++);
        UpdateTarget(_footR, seed++);
    }
}

} // namespace FloatingPuppet
