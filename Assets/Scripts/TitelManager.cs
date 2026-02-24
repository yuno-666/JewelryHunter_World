using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class TitelManager : MonoBehaviour
{
    public string sceneName;
    public GameObject startButton;//スタートボタン
    public GameObject continueButton;//コンティニューボタン
    bool selectStart = true;

    //public InputAction submitAction; //InputAction

    //void OnEnable()
    //{
    //    submitAction.Enable(); //InputActionを有効化
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable(); //InputActionを無効化
    //}

    void OnSelect(InputValue value)
    {
        if (value.isPressed)
        {
            
            //PlayerPrefsからJSON文字列ロード
            string jsonData = PlayerPrefs.GetString("SaveData");
            // そもそもセーブデータがなければボタンを切り替えられない
            if (string.IsNullOrEmpty(jsonData))
            {
                return;
            }

            if (selectStart)
            {
                startButton.GetComponent<Button>().interactable = false;//ボタン機能を無効
                continueButton.GetComponent<Button>().interactable = true;
                selectStart = false;
            }
            else
            {
                startButton.GetComponent<Button>().interactable = true;
                continueButton.GetComponent<Button>().interactable = false;
                selectStart = true;
            }
        }
    }

    //Input SystemのActionで決めたSubmitアクションが呼び出されたときに実行されるメソッド
    void OnSubmit(InputValue Value)
    {
        if (selectStart)
        {
            Load();
        }
        else
        {
            ContinueLoad();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        continueButton.GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard kb = Keyboard.current; //current(デバイス情報)をkbに代入
        //if(kb != null)　//接続されているデバイス情報があれば
        //{
        //    if (kb.enterKey.wasPressedThisFrame) //Enterが押された瞬間
        //    {
        //        Load(); //シーン切り替え
        //    }
        //}

        //if (submitAction.WasPressedThisFrame())
        //{
        //    Load();
        //}

    }

    // シーンを読み込むメソッド
    public void Load()
    {
        //GameManager.totalScore = 0;//新しくゲームを始めるにあたりスコアをリセット
        SaveDateManager.Initialize(); //セーブデータを初期化
        SceneManager.LoadScene(sceneName);
    }

    // セーブデータを読み込んでから始める
    public void ContinueLoad()
    {
        SaveDateManager.LoadGameData(); //セーブデータを読み込む
        SceneManager.LoadScene(sceneName);
    }
}
