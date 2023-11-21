using UnityEngine;
using Unity.Mathematics;
using Klak.Math;
using Random = Unity.Mathematics.Random;

namespace FloatingPuppet {

[System.Serializable]
public struct Motion
{
    public float frequency;
    public float3 displacement;
    public float3 rotation;
}

public sealed class PuppetRigController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public Transform Root { get; set; }
    [field:SerializeField] public uint Seed { get; set; }
    [field:SerializeField] public Motion RootMotion { get; set; }
    [field:SerializeField] public Motion HandMotion { get; set; }
    [field:SerializeField] public Motion FootMotion { get; set; }
    [field:SerializeField] public float SpineFrequency { get; set; }
    [field:SerializeField] public float3 SpineRotation { get; set; }

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

    Target _root, _spine, _handL, _handR, _footL, _footR;

    uint UpdateTarget(in Target target,
                      float3 pos, float3 rot,
                      float freq, float time, uint seed)
    {
        var rand = Random.CreateFromIndex(seed++);
        var t = rand.NextFloat3(0.95f, 1.05f) * time * freq;
        var disp = Noise.Float3(t, seed++) * pos;
        var angle = Noise.Float3(t, seed++) * math.radians(rot);
        var q = quaternion.Euler(angle);
        target.xform.localPosition = target.pos + disp;
        target.xform.localRotation = math.mul(target.rot, q);
        return seed;
    }

    uint UpdateTarget(in Target target, in Motion motion, float time, uint seed)
      => UpdateTarget(target, motion.displacement, motion.rotation, motion.frequency, time, seed);

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _root = new Target(transform);
        _spine = FindTarget("Spine");
        _handL = FindTarget("Spine/Left Hand");
        _handR = FindTarget("Spine/Right Hand");
        _footL = FindTarget("Left Foot");
        _footR = FindTarget("Right Foot");
    }

    void LateUpdate()
    {
        var time = Time.time + 100;
        var seed = Seed;
        seed = UpdateTarget(_root, RootMotion, time, seed);
        seed = UpdateTarget(_spine, 0, SpineRotation, SpineFrequency, time, seed);
        seed = UpdateTarget(_handL, HandMotion, time, seed);
        seed = UpdateTarget(_handR, HandMotion, time, seed);
        seed = UpdateTarget(_footL, FootMotion, time, seed);
        seed = UpdateTarget(_footR, FootMotion, time, seed);
    }

    #endregion
}

} // namespace FloatingPuppet
