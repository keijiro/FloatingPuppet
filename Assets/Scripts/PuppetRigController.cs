using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

namespace FloatingPuppet {

public sealed class PuppetRigController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public Transform Root { get; set; }
    [field:SerializeField] public uint Seed { get; set; }

    #endregion

    #region Rig target struct

    readonly struct Target
    {
        public readonly Transform xform;
        public readonly float3 pos;
        public readonly quaternion rot;

        public Target(Transform t)
          => (xform, pos, rot) = (t, t.localPosition, t.localRotation);
    }

    Target FindTarget(string name) => new Target(transform.Find(name));

    #endregion

    #region Rig target operations

    Target _root, _handL, _handR, _footL, _footR;

    void UpdateTarget(in Target target, uint seed)
    {
        var t = Time.time;
        target.xform.localPosition = target.pos + Noise.Float3(t, seed++);
        target.xform.localRotation = math.mul(target.rot, Noise.Rotation(t, math.PI, seed++));
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _root = new Target(transform);
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

    #endregion
}

} // namespace FloatingPuppet
