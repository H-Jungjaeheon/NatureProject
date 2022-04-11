using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonMono<SoundManager>
{
    //#region �̱���
    //void Awake()
    //{
    //    AwakeAfter();
    //}
    //#endregion

    public float masterVolumeSFX;
    public float masterVolumeBGM;

    [SerializeField] AudioClip[] BGMClip; // BGM - ����� �ҽ��� ����.
    [SerializeField] AudioClip[] SFXClip; // SFX - ����� �ҽ��� ����.

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

    public void SettingBGM(string Name) // BgmSetting���� : SoundMgr�ν��Ͻ� �ҷ��ͼ� �Լ��� ���� Name ���ڰ� �ְ� ����
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

    // �� �� ��� : ���� �Ű������� ����
    public void PlaySound(string a_name, float a_volume = 1f)
    {
        if (SfxAudioClipsDic.ContainsKey(a_name) == false)
        {
            Debug.Log(a_name + " is not Contained SfxAudioClipsDic");
            return;
        }
        sfxPlayer.PlayOneShot(SfxAudioClipsDic[a_name], a_volume * masterVolumeSFX);
    }

    // �������� �� �� ��� : ���� �Ű������� ����
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

    // �����Ҷ��� ���ϰ��� GameObject�� �����ؼ� �����Ѵ�. ���߿� �ɼǿ��� ���� ũ�� �����ϸ� �̰� ���� �����ؼ� �ٲ�����..
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

    // �ַ� ���� ����� ������ ����.
    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    #region �ɼǿ��� ��������
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
