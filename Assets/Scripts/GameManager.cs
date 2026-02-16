using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState           // ゲームの状態
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class GameManager : MonoBehaviour
{
    // ゲームの状態
    public static GameState gameState;

    public string nextSceneName;            // 次のシーン名

    //スコア追加
    public static int totalScore;          //  合計スコア

    //サウンド関連
    AudioSource soundPlayer; //AudioSource
    public AudioClip meGameClear;    // ゲームクリアの音
    public AudioClip meGameOver;     // ゲームオーバーの音


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.InGame; //ステータスをゲーム中にする

        soundPlayer = GetComponent<AudioSource>(); //AudioSourceを参照する

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameState == GameState.GameClear)
        {
            soundPlayer.Stop(); //音を止める
            soundPlayer.PlayOneShot(meGameClear); //ゲームクリアの音を1回鳴らす

            gameState = GameState.GameEnd; //ゲームの状態を更新
        }
        else if (gameState == GameState.GameOver)
        {
            soundPlayer.Stop(); //音を止める
            soundPlayer.PlayOneShot(meGameOver); //ゲームオーバーの音を1回鳴らす
            gameState = GameState.GameEnd; //ゲームの状態を更新
        }

    }

    //リスタート
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
