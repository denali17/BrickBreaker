using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    public Slider m_volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        m_volumeSlider.onValueChanged.AddListener(SetVolume);

        m_volumeSlider.value = 0.5f;
    }

    public void SetVolume(float sliderValue)
    {
        AudioListener.volume = sliderValue;
    }
}
