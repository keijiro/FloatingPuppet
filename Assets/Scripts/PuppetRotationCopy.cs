using UnityEngine;

namespace FloatingPuppet {

public sealed class PuppetRotationCopy : MonoBehaviour
{
    [field:SerializeField] public Transform Target { get; set; }

    void LateUpdate()
      => transform.localRotation = Target.localRotation;
}

} // namespace FloatingPuppet
