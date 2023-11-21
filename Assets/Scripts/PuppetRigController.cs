using UnityEngine;
using Unity.Mathematics;
using Klak.Math;
using Random = Unity.Mathematics.Random;

namespace FloatingPuppet {


public sealed class PuppetRigController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public Transform Root { get; set; }
    [field:SerializeField] public uint Seed { get; set; }

    [field:SerializeField, Space] public float RootFrequency { get; set; }
    [field:SerializeField] public float3 RootDisplacement { get; set; }
    [field:SerializeField] public float3 RootRotation { get; set; }

    [field:SerializeField, Space] public float SpineFrequency { get; set; }
    [field:SerializeField] public float3 SpineRotation { get; set; }

    [field:SerializeField, Space] public float LimbFrequency { get; set; }
    [field:SerializeField] public float3 HandDisplacement { get; set; }
    [field:SerializeField] public float3 FootDisplacement { get; set; }

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
        seed = UpdateTarget(_root, RootDisplacement, RootRotation, RootFrequency, time, seed);
        seed = UpdateTarget(_spine, 0, SpineRotation, SpineFrequency, time, seed);
        seed = UpdateTarget(_handL, HandDisplacement, 0, LimbFrequency, time, seed);
        seed = UpdateTarget(_handR, HandDisplacement, 0, LimbFrequency, time, seed);
        seed = UpdateTarget(_footL, FootDisplacement, 0, LimbFrequency, time, seed);
        seed = UpdateTarget(_footR, FootDisplacement, 0, LimbFrequency, time, seed);
    }

    #endregion
}

} // namespace FloatingPuppet
