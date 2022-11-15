using Sirenix.OdinInspector;
using UnityEngine;


public class UIRadialSlider : MonoBehaviour
{
    [SerializeField, TitleGroup("Properties"), PropertyRange(0, 1), OnValueChanged(nameof(UpdateSliderUI))]
    private float slider;
    [SerializeField, TitleGroup("Properties"), OnValueChanged(nameof(UpdateSliderUI))]
    private bool counterClockSize;
    [SerializeField, TitleGroup("Properties"), OnValueChanged(nameof(UpdateSliderRadius))]
    private float radius;

    [SerializeField, TitleGroup("Properties"), MinMaxSlider(0, 360), OnValueChanged(nameof(UpdateSliderUI))]
    private Vector2 range;

    [SerializeField, TitleGroup("Properties"), PropertyRange(0, 360), OnValueChanged(nameof(UpdateSliderUI))]
    private float offset;

    [SerializeField, TitleGroup("Refs")]
    private RectTransform sliderHandle;

    [SerializeField, TitleGroup("Refs")]
    private RectTransform sliderHandleParent;

    private float _dummyAngle;
    private float _dummyModifiedSlider;
    private Vector3 _dummyEuler = Vector3.zero;

    public float Slider
    {
        get => slider;
        set
        {
            slider = value;
            UpdateSliderUI();
        }
    }

    private void UpdateSliderUI()
    {
        _dummyModifiedSlider = counterClockSize ? 1 - slider : slider;
        _dummyAngle = (range.x + (_dummyModifiedSlider * (range.y - range.x))) + offset;
        _dummyEuler.z = _dummyAngle;
        sliderHandleParent.localRotation = Quaternion.Euler(_dummyEuler);
    }

    private void UpdateSliderRadius()
    {
        sliderHandle.localPosition = Vector3.right * radius;
        UpdateSliderUI();
    }

}
