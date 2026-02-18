using System.Collections.Generic;
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

    public bool isGameClear = false; //ゲームクリア判定
    public bool isGameOver = false;//ゲーム終了判定

    //ワールドマップで最後に入ったドアの番号
    public static int currentDoorNumber=0;
    
    public static int keys = 1; //鍵の数
    
    //どのステージで鍵を取ったかの判定
    public static Dictionary<string, bool> keyGot; //シーン名、true,false

    public static int arrows = 10; //矢の数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameState = GameState.InGame; //ステータスをゲーム中にする

        soundPlayer = GetComponent<AudioSource>(); //AudioSourceを参照する

        //keyGotが何もない状態だった時のみ初期化
        if (keyGot == null) 
        {
            keyGot = new Dictionary<string, bool>(); //keyGotを初期化する
        }

        if (!keyGot.ContainsKey(SceneManager.GetActiveScene().name)) //現在のシーン名がkeyGotに登録されてなければ
        {
            //現在のシーン名をDictionary(keyGot)に登録する
            keyGot.Add(SceneManager.GetActiveScene().name, false); 
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameState == GameState.GameClear)
        {
            soundPlayer.Stop(); //音を止める
            soundPlayer.PlayOneShot(meGameClear); //ゲームクリアの音を1回鳴らす
            isGameClear = true; //ゲームクリアフラグを立てる
            gameState = GameState.GameEnd; //ゲームの状態を更新
        }
        else if (gameState == GameState.GameOver)
        {
            soundPlayer.Stop(); //音を止める
            soundPlayer.PlayOneShot(meGameOver); //ゲームオーバーの音を1回鳴らす
            isGameOver = true; //ゲームオーバーフラグを立てる
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

    public void GameEnd()
    {
        if (gameState != GameState.GameEnd) //ゲーム中でなければ
        {
            if (isGameClear) //ゲームクリアなら
            {
                Next(); //クリアなら次のシーンへ
            }

            else if (isGameOver)//ゲームオーバーなら
            {
                Restart(); //オーバーならリスタート
            }
        }

    }
}
