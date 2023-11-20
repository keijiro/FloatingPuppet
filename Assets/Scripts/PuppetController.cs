using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

namespace FloatingPuppet {

public sealed class PuppetController : MonoBehaviour
{
    [field:SerializeField] public Transform Root { get; set; }
    [field:SerializeField] public uint Seed { get; set; }

    (Transform xform, float3 pos, quaternion rot)
      _root, _handL, _handR, _legL, _legR;

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
        _root = GetTarget(transform);
        _handL = FindTarget("Left Hand");
        _handR = FindTarget("Right Hand");
        _legL = FindTarget("Left Leg");
        _legR = FindTarget("Right Leg");
    }

    void Update()
    {
        var seed = Seed;
        UpdateTarget(_root, seed++);
        UpdateTarget(_handL, seed++);
        UpdateTarget(_handR, seed++);
        UpdateTarget(_legL, seed++);
        UpdateTarget(_legR, seed++);

        Root.position = _root.xform.position;
        Root.rotation = _root.xform.rotation;
    }
}

} // namespace FloatingPuppet
