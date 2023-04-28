using UnityEngine;
using Cinemachine;

namespace Lunarsoft
{
    public class CameraShakeController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private SignalSourceAsset rawSignal;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CinemachineImpulseSource impulseSource;

        private void Start()
        {
            CreateVirtualCamera();
            CreateImpulseSource();
        }

        private void CreateVirtualCamera()
        {
            virtualCamera.m_Lens.OrthographicSize = mainCamera.orthographicSize;
            virtualCamera.m_Lens.NearClipPlane = mainCamera.nearClipPlane;
            virtualCamera.m_Lens.FarClipPlane = mainCamera.farClipPlane;
            virtualCamera.gameObject.AddComponent<CinemachineImpulseListener>();
            virtualCamera.Priority = 11;
        }

        private void CreateImpulseSource()
        {
            GameObject impulseSourceGameObject = new GameObject("Impulse Source");
            impulseSource = impulseSourceGameObject.AddComponent<CinemachineImpulseSource>();
            impulseSource.m_ImpulseDefinition.m_RawSignal = rawSignal;
            impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 1f;
            impulseSource.m_ImpulseDefinition.m_FrequencyGain = 1f;
        }

        public void ShakeCamera()
        {
            impulseSource.GenerateImpulse();
        }
    }
}
