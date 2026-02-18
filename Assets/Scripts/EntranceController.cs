using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EntranceController : MonoBehaviour
{
    public int doorNumber;//ドア番号
    public string sceneName;//移行したいシーン名
    public bool opened;//ドアが開いているかどうか

    bool isPlayerTouch;//プレイヤーとの接触状態

    bool announcement;//アナウンス中かどうか

    GameObject worldUI;//Canvasオブジェクト
    GameObject talkPanel;//TalkPanelオブジェクト
    TextMeshProUGUI messageText; // World_Playerオブジェクトの World_PlayerControllerコンポーネント
    World_PlayerController worldPlayerCnt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        worldPlayerCnt = GameObject.FindGameObjectWithTag("Player").GetComponent<World_PlayerController>();
        worldUI = GameObject.FindGameObjectWithTag("WorldUI");
        talkPanel = worldUI.transform.Find("TalkPanel").gameObject;
        messageText = talkPanel.transform.Find("MessageText").gameObject.GetComponent<TextMeshProUGUI>();

        if (World_UIController.keyOpened != null)
        {
            opened = World_UIController.keyOpened[doorNumber];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //playerと接触かつアクションボタンが押されている
        if (isPlayerTouch && worldPlayerCnt.IsActionButtonPressed)
        {
            //アナウンス中でない場合
            if (!announcement)
            {
                Time.timeScale = 0;//ゲーム進行をstop
                if (opened)//開錠済みの場合
                {
                    Time.timeScale = 1;//ゲーム進行を再開
                    //該当ドア番号をGameManagerに管理してもらう
                    GameManager.currentDoorNumber = doorNumber;
                    SceneManager.LoadScene(sceneName);
                    return;
                }
                //未開錠の場合
                else if (GameManager.keys > 0)//鍵を持っている場合
                {
                    messageText.text = "新たなステージへの扉を開けた！";
                    GameManager.keys--;//鍵を消耗
                    opened = true;//開錠フラグをたてる
                    //World_UIControllerが所持する開錠の帳簿（keyOpenedディクショナリー）に開錠したことを記録する
                    World_UIController.keyOpened[doorNumber] = true;
                    announcement = true;//アナウンス中フラグ
                }
                else//未開錠かつ鍵を持っていない場合   
                {
                    messageText.text = "鍵が足りません！";
                    announcement = true;//アナウンス中フラグ
                }
            }
            else//すでにアナウンス中ならannauncement==true
            {
                Time.timeScale = 1;//ゲーム進行を再開
                string s = "";
                if (!opened)
                {
                    s = "(ロック)";
                }
                messageText.text = sceneName + s;
                announcement = false;//アナウンス中フラグを解除
            }

            //連続入力にならないように一度リセット　※次にボタンが押されるまではfalse
            worldPlayerCnt.IsActionButtonPressed = false;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーと接触したときtrueにしパネルを表示する
        if (collision.gameObject.tag == "Player")
        {
            isPlayerTouch = true;
            talkPanel.SetActive(true);
            //パネルのメッセージに行先のシーン名を表示する
            //未開錠の場合はシーン名の後ろに(ロック)と書き換える
            string s = "";
            if (!opened)
            {
                s = "(ロック)";
            }
            messageText.text = sceneName + s;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーと離れたらfalseにしパネルを消す
        if (collision.gameObject.tag == "Player")
        {
            isPlayerTouch = false;
            if (messageText != null) // NullReferenceExceptionを防ぐ
            {
                talkPanel.SetActive(false);
                Time.timeScale = 1f; // ゲーム進行を再開
            }
        }
    }

}
