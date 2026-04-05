using UnityEngine;

public class EnemyAudioPlayer : MonoBehaviour
{
    public EnemySFXTable sfxTable;

    public void Play<T>(T sfxEnum, Vector3 pos)
    {
        string key = sfxEnum.ToString();
        AudioClip clip = sfxTable.GetClip(key);

        if (clip != null)
            AudioManager.Instance.PlaySFX(clip, pos);
    }
}