using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonMono<SoundManager>
{
    //#region 싱글톤
    //void Awake()
    //{
    //    AwakeAfter();
    //}
    //#endregion

    public float masterVolumeSFX;
    public float masterVolumeBGM;

    [SerializeField] AudioClip[] BGMClip; // BGM - 오디오 소스들 지정.
    [SerializeField] AudioClip[] SFXClip; // SFX - 오디오 소스들 지정.

    Dictionary<string, AudioClip> SfxAudioClipsDic;
    Dictionary<string, AudioClip> BgmAudioClipsDic;

    AudioSource sfxPlayer;
    AudioSource bgmPlayer;

    void AwakeAfter()
    {
        sfxPlayer = GetComponent<AudioSource>();

        SfxAudioClipsDic = new Dictionary<string, AudioClip>();
        foreach (AudioClip a in SFXClip)
        {
            SfxAudioClipsDic.Add(a.name, a);
        }

        BgmAudioClipsDic = new Dictionary<string, AudioClip>();
        foreach (AudioClip a in BGMClip)
        {
            Debug.Log($"{a.name} : {a}");

            BgmAudioClipsDic.Add(a.name, a);
        }

        SetupBGM();
    }

    void SetupBGM()
    {
        if (BGMClip == null) return;

        GameObject child = new GameObject("BGM");
        child.transform.SetParent(transform);
        bgmPlayer = child.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.clip = BgmAudioClipsDic["BasicBGM"];
        bgmPlayer.volume = masterVolumeBGM;
    }

    public void SettingBGM(string Name) // BgmSetting사용법 : SoundMgr인스턴스 불러와서 함수에 사운드 Name 인자값 넣고 실행
    {
        bgmPlayer.clip = BgmAudioClipsDic[Name];
        bgmPlayer.volume = masterVolumeBGM;
        bgmPlayer.Play();
    }

    private void Start()
    {
        AwakeAfter();

        if (bgmPlayer != null)
            bgmPlayer.Play();
    }

    // 한 번 재생 : 볼륨 매개변수로 지정
    public void PlaySound(string a_name, float a_volume = 1f)
    {
        if (SfxAudioClipsDic.ContainsKey(a_name) == false)
        {
            Debug.Log(a_name + " is not Contained SfxAudioClipsDic");
            return;
        }
        sfxPlayer.PlayOneShot(SfxAudioClipsDic[a_name], a_volume * masterVolumeSFX);
    }

    // 랜덤으로 한 번 재생 : 볼륨 매개변수로 지정
    public void PlayRandomSound(string[] a_nameArray, float a_volume = 1f)
    {
        string l_playClipName;

        l_playClipName = a_nameArray[Random.Range(0, a_nameArray.Length)];

        if (SfxAudioClipsDic.ContainsKey(l_playClipName) == false)
        {
            Debug.Log(l_playClipName + " is not Contained SfxAudioClipsDic");
            return;
        }
        sfxPlayer.PlayOneShot(SfxAudioClipsDic[l_playClipName], a_volume * masterVolumeSFX);
    }

    // 삭제할때는 리턴값은 GameObject를 참조해서 삭제한다. 나중에 옵션에서 사운드 크기 조정하면 이건 같이 참조해서 바뀌어야함..
    public GameObject PlayLoopSound(string a_name)
    {
        if (SfxAudioClipsDic.ContainsKey(a_name) == false)
        {
            Debug.Log(a_name + " is not Contained SfxAudioClipsDic");
            return null;
        }

        GameObject l_obj = new GameObject("LoopSound");
        AudioSource source = l_obj.AddComponent<AudioSource>();
        source.clip = SfxAudioClipsDic[a_name];
        source.volume = masterVolumeSFX;
        source.loop = true;
        source.Play();
        return l_obj;
    }

    // 주로 전투 종료시 음악을 끈다.
    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    #region 옵션에서 볼륨조절
    public void SetVolumeSFX(float a_volume)
    {
        masterVolumeSFX = a_volume;
    }

    public void SetVolumeBGM(float a_volume)
    {
        masterVolumeBGM = a_volume;
        bgmPlayer.volume = masterVolumeBGM;
    }
    #endregion
}
