using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    private AudioSource m_audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        m_audioSource = GetComponent<AudioSource>();
    }
  
    // Start is called before the first frame update
    void Start()
    {
        m_audioSource.Play();
    }
}
