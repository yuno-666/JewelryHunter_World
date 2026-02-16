using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;//

public class TitelManager : MonoBehaviour
{
    public string sceneName;

    //public InputAction submitAction; //InputAction

    //void OnEnable()
    //{
    //    submitAction.Enable(); //InputActionを有効化
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable(); //InputActionを無効化
    //}

    //Input SystemのActionで決めたSubmitアクションが呼び出されたときに実行されるメソッド
    void OnSubmit(InputValue Value)
    {
        Load();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        GameManager.totalScore = 0; //新しいゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }
}
