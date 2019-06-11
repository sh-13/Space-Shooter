using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        int noMusicPlayer = FindObjectsOfType<MusicPlayer>().Length;
        if (noMusicPlayer > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
}
