using UnityEngine;
using UnityEngine.UI;

public class HueShiftController : MonoBehaviour
{
    public float hueShiftSpeed = 1f; // The speed at which to update the hue shift value
    public float hueShiftAmplitude = 0.5f; // The amplitude of the hue shift sine wave

    private Image image; // The Image component on the object
    private Material spriteMaterial; // The material used by the Image component
    private float hueShiftValue = 0f; // The current hue shift value

    void Start()
    {
        // Get the Image component and material
        image = GetComponent<Image>();
        spriteMaterial = image.material;
    }

    void Update()
    {
        // Update the hue shift value using a sine wave
        hueShiftValue = Mathf.Sin(Time.time * hueShiftSpeed) * hueShiftAmplitude * 360f;

        // Set the hue shift value of the material
        spriteMaterial.SetFloat("_HueShift", hueShiftValue);
    }
}
