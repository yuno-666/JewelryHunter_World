using UnityEngine;
using UnityEngine.SceneManagement;

//BGMタイプの列挙
public enum BGMType
{
    None,
    Title,
    InGame,
    InBoss,
    GameClear,
    GameOver
}

public enum SEType
{
    Shoot,
    DoorOpen,
    DoorClosed,
    ItemGet,
    GetDamage,
    Enemykilled,
    BossKilled
}


public class SoundManager : MonoBehaviour
{
    //実体化した自分をstaticなものとして全シーンで唯一無二のクラスとして扱える（シングルトン）
    public static SoundManager currentSoundManager;
    public bool restartBGM;//WorldMapを行き来する際

    public AudioClip bgmTitle;
    public AudioClip bgmInGame;
    public AudioClip bgmInBoss;
    public AudioClip bgmGameClear;
    public AudioClip bgmGameOver;

    public AudioClip seShoot;
    public AudioClip seDoorOpen;
    public AudioClip seDoorClosed;
    public AudioClip seItemGet;
    public AudioClip seGetDamage;
    public AudioClip seEnemykilled;
    public AudioClip seBosskilled;
    //自分についている2つのAudioSouceコンポーネントを格納する配列
    static AudioSource[] audios;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //BGM再生
        if (currentSoundManager == null)
        {
            //soundmasterが最初に存在するシーンにてそのオブジェクトをstaticなオブジェクトとして扱う
            currentSoundManager = this;//thisとはヒエラルキーに存在しているオブジェクト（についているクラス）
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //2つのAudioSauce
        audios = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBGM(BGMType type)
    {
        switch (type)
        {
            case BGMType.Title:
                audios[0].clip = bgmTitle;
                audios[0].Play();
                break;
            case BGMType.InGame:
                audios[0].clip = bgmInGame;
                audios[0].Play();
                break;
            case BGMType.InBoss:
                audios[0].clip = bgmInBoss;
                audios[0].Play();
                break;
            case BGMType.GameClear:
                audios[0].PlayOneShot(bgmGameClear);
                break;
            case BGMType.GameOver:
                audios[0].PlayOneShot(bgmGameOver);
                break;
        }
    }

    public void PlaySE(SEType type)
    {
        switch (type)
        {
            case SEType.Shoot:
                audios[1].PlayOneShot(seShoot);
                break;
            case SEType.DoorOpen:
                audios[1].PlayOneShot(seDoorOpen);
                break;
            case SEType.DoorClosed:
                audios[1].PlayOneShot(seDoorClosed);
                break;
            case SEType.ItemGet:
                audios[1].PlayOneShot(seItemGet);
                break;
            case SEType.GetDamage:
                audios[1].PlayOneShot(seGetDamage);
                break;
            case SEType.Enemykilled:
                audios[1].PlayOneShot(seEnemykilled);
                break;
            case SEType.BossKilled:
                audios[1].PlayOneShot(seBosskilled);
                break;
        }
    }

    public void StopBGM()
    {
        audios[0].Stop();
    }
}
