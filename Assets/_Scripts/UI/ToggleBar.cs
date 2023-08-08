using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleBar : MonoBehaviour
{
    private Slider slider;
    private Animation wobbleAnim;

    public UnityEvent<bool> OnToggle;

    private Button button;
    private bool isOn = true;
    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        wobbleAnim = GetComponentInChildren<Animation>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnBtnToggle);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnBtnToggle);
    }
    private void OnBtnToggle()
    {
        isOn = !isOn;
        if (isOn)
        {
            slider.value = 1;
        }
        else
        {
            slider.value = 0;
        }
        wobbleAnim.Play();
        OnToggle?.Invoke(isOn);
    }
}
