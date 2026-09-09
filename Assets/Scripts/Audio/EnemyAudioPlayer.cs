using UnityEngine;

public class EnemyAudioPlayer : MonoBehaviour
{
    public EnemySFXTable sfxTable;

    //从播放表中播放
    public void Play<T>(T sfxEnum, Vector3 pos , bool isPosFree = false , float Pitch = 1.0f)
    {
        
        string key = sfxEnum.ToString();
        AudioClip clip = sfxTable.GetClip(key);

        if (clip != null)
            AudioManager.Instance.PlaySFX(clip, pos , isPosFree , Pitch);
    }

    //直接播放音效片段
    public void PlayClip(AudioClip clip, Vector3 pos, bool isPosFree = false, float Pitch = 1.0f)
    {
        if (clip != null)
            AudioManager.Instance.PlaySFX(clip, pos, isPosFree, Pitch);
    }

}