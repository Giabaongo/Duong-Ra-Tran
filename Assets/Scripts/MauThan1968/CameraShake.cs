using UnityEngine;
using System.Collections;
using Unity.Cinemachine; // Unity 6 / Cinemachine 3.x

/// <summary>
/// Script rung camera khi có explosion hoặc impact
/// Gắn vào Main Camera hoặc Cinemachine Virtual Camera
/// </summary>
public class CameraShake : MonoBehaviour
{
    [Header("⚙️ SHAKE SETTINGS")]
    [Tooltip("Cường độ rung tối đa")]
    [SerializeField] private float maxShakeMagnitude = 5.0f;
    
    [Tooltip("Thời gian rung mặc định (giây)")]
    [SerializeField] private float defaultDuration = 0.5f;
    
    [Tooltip("Tốc độ giảm dần cường độ")]
    [SerializeField] private float dampingSpeed = 1.0f;
    
    [Tooltip("Có dùng Cinemachine không?")]
    [SerializeField] private bool useCinemachine = false;
    
    private Vector3 originalPosition;
    private float shakeTimer = 0f;
    private float shakeMagnitude = 0f;
    private bool isShaking = false;
    
    // Cinemachine support (Unity 6 / Cinemachine 3.x)
    private CinemachineCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    
    void Awake()
    {
        Debug.Log($"[CameraShake] 🚀 Awake called on {gameObject.name}, useCinemachine: {useCinemachine}");
        
        // Tìm Cinemachine Camera nếu có (Unity 6 / Cinemachine 3.x)
        if (useCinemachine)
        {
            cinemachineCamera = GetComponent<CinemachineCamera>();
            if (cinemachineCamera != null)
            {
                Debug.Log($"[CameraShake] ✅ Found CinemachineCamera: {gameObject.name}");
                
                // Unity 6: GetCinemachineComponent
                perlinNoise = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
                if (perlinNoise != null)
                {
                    Debug.Log($"[CameraShake] ✅ Cinemachine 3.x setup successful! Noise: {perlinNoise.NoiseProfile}");
                    // Reset về 0 để script này control
                    perlinNoise.AmplitudeGain = 0;
                    perlinNoise.FrequencyGain = 0;
                    Debug.Log($"[CameraShake] ✅ Reset Amplitude: {perlinNoise.AmplitudeGain}, Frequency: {perlinNoise.FrequencyGain}");
                }
                else
                {
                    Debug.LogError("[CameraShake] ❌ CinemachineBasicMultiChannelPerlin not found! Thêm 'Basic Multi Channel Perlin' trong Procedural Components → Noise");
                }
            }
            else
            {
                Debug.LogError("[CameraShake] ❌ CinemachineCamera component not found! Gắn script này vào GameObject có CinemachineCamera.");
            }
        }
        else
        {
            Debug.LogWarning("[CameraShake] ⚠️ useCinemachine is FALSE! Please enable it in Inspector!");
        }
    }
    
    void Start()
    {
        originalPosition = transform.localPosition;
    }
    
    void LateUpdate()
    {
        if (!isShaking) return;
        
        if (shakeTimer > 0)
        {
            // Cinemachine shake
            if (useCinemachine)
            {
                // Lazy init nếu chưa có
                if (perlinNoise == null && cinemachineCamera != null)
                {
                    perlinNoise = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
                    if (perlinNoise != null)
                    {
                        Debug.Log("[CameraShake] 🔄 Lazy initialized perlinNoise!");
                    }
                }
                
                if (perlinNoise != null)
                {
                    float oldAmp = perlinNoise.AmplitudeGain;
                    perlinNoise.AmplitudeGain = shakeMagnitude;
                    perlinNoise.FrequencyGain = shakeMagnitude * 2f;
                    
                    // Log lần đầu để verify
                    if (oldAmp == 0 && shakeMagnitude > 0)
                    {
                        Debug.Log($"[CameraShake] ✅ Setting Perlin! Amp: {shakeMagnitude:F2}, Freq: {(shakeMagnitude * 2f):F2}");
                    }
                }
                else
                {
                    Debug.LogWarning("[CameraShake] ⚠️ perlinNoise is NULL! Cannot shake Cinemachine camera!");
                }
            }
            else
            {
                // Manual shake (cho regular camera)
                Vector3 randomOffset = Random.insideUnitCircle * shakeMagnitude;
                transform.localPosition = originalPosition + randomOffset;
            }
            
            // Giảm dần
            shakeTimer -= Time.deltaTime * dampingSpeed;
            shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0f, Time.deltaTime * dampingSpeed * 2f);
        }
        else
        {
            // Kết thúc shake
            StopShake();
        }
    }
    
    /// <summary>
    /// Bắt đầu rung camera
    /// </summary>
    /// <param name="duration">Thời gian rung (giây)</param>
    /// <param name="magnitude">Cường độ (0-1)</param>
    public void Shake(float duration = -1f, float magnitude = 1f)
    {
        if (duration < 0)
            duration = defaultDuration;
        
        shakeTimer = duration;
        // Cho phép magnitude > 1 cho explosions mạnh
        shakeMagnitude = Mathf.Clamp(magnitude, 0f, 3f) * maxShakeMagnitude;
        isShaking = true;
        
        Debug.Log($"[CameraShake] 📳 Shaking! Duration: {duration:F2}s, Input: {magnitude:F2}, Final: {shakeMagnitude:F2}");
    }
    
    /// <summary>
    /// Rung ngay lập tức với intensity (dùng cho bom nổ)
    /// </summary>
    /// <param name="intensity">Độ mạnh (0-1, có thể > 1 cho explosion lớn)</param>
    public void ShakeExplosion(float intensity = 1f)
    {
        float duration = Mathf.Lerp(0.2f, 0.5f, Mathf.Clamp01(intensity));
        float magnitude = Mathf.Clamp(intensity, 0f, 2f); // Cho phép > 1 cho bom
        
        Shake(duration, magnitude);
    }
    
    /// <summary>
    /// Dừng rung
    /// </summary>
    public void StopShake()
    {
        isShaking = false;
        shakeTimer = 0f;
        shakeMagnitude = 0f;
        
        // Reset position
        if (useCinemachine && perlinNoise != null)
        {
            perlinNoise.AmplitudeGain = 0f;
            perlinNoise.FrequencyGain = 0f;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }
    
    /// <summary>
    /// Rung theo khoảng cách từ explosion (tự động giảm theo distance)
    /// </summary>
    /// <param name="explosionPosition">Vị trí nổ</param>
    /// <param name="maxDistance">Khoảng cách tối đa còn rung</param>
    /// <param name="baseIntensity">Cường độ cơ bản tại tâm nổ</param>
    public void ShakeFromExplosion(Vector3 explosionPosition, float maxDistance = 20f, float baseIntensity = 1f)
    {
        float distance = Vector3.Distance(transform.position, explosionPosition);
        
        if (distance > maxDistance)
        {
            Debug.Log($"[CameraShake] Too far from explosion ({distance:F1} > {maxDistance})");
            return;
        }
        
        // Giảm intensity theo khoảng cách
        float distanceFactor = 1f - (distance / maxDistance);
        float finalIntensity = baseIntensity * distanceFactor;
        
        ShakeExplosion(finalIntensity);
        
        Debug.Log($"[CameraShake] 💥 Explosion at {distance:F1}m, Intensity: {finalIntensity:F2}");
    }
    
    /// <summary>
    /// Test rung camera (dùng trong Inspector)
    /// </summary>
    [ContextMenu("Test Shake (Weak)")]
    void TestShakeWeak()
    {
        Shake(0.3f, 0.5f);
        Debug.Log("[CameraShake] 🧪 Testing WEAK shake...");
    }
    
    [ContextMenu("Test Shake (Medium)")]
    void TestShakeMedium()
    {
        Shake(0.4f, 1.0f);
        Debug.Log("[CameraShake] 🧪 Testing MEDIUM shake...");
    }
    
    [ContextMenu("Test Shake (Strong)")]
    void TestShakeStrong()
    {
        Shake(0.5f, 1.5f);
        Debug.Log("[CameraShake] 🧪 Testing STRONG shake...");
    }
    
    [ContextMenu("Test Explosion Shake")]
    void TestExplosionShake()
    {
        ShakeExplosion(1.2f);
        Debug.Log("[CameraShake] 🧪 Testing EXPLOSION shake (like B52 bomb)...");
    }
    
    void OnDestroy()
    {
        StopShake();
    }
}


