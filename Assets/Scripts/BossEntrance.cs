using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEntrance : MonoBehaviour
{
    //各エントランスのクリア状況を管理
    public static Dictionary<int,bool> stagesClear;
    public string sceneName;//シーン切り替え先
    bool isOpened;//開いているかどうか状況
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] obj =
            GameObject.FindGameObjectsWithTag("Entrance");
        //リストがない時の情報取得とセッティング
        if (stagesClear == null)
        {
            stagesClear = new Dictionary<int, bool>(); // 最初に初期化が必要

            //集めてきたEntranceオブジェクトを全点検
            for (int i = 0; i < obj.Length; i++)
            {
                //Entranceオブジェクトが持っているEntrancecontrollerを取得
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    //帳簿（keyOpenedディクショナリー）に変数doorNumberと変数opendの情報を記録
                    stagesClear.Add(
                        entranceController.doorNumber,
                        false
                    );
                }
            }
        }
        else
        {
            int sum = 0;//クリアがどのくらいあるのかカウント用
            //Entranceの数だけstageClearの中身を確認
            for (int i = 0; i < obj.Length; i++)
            {
                if (stagesClear[i])//stageClearディクショナリーの中身順にチェック
                {
                    sum++;//もしtrue（clear済み）ならカウント
                }
            }
            if (sum >= obj.Length) //もしクリア数（trueの数）とEntranceの数が一致していたら
            {
                //全部クリアしたので扉を開ける
                GetComponent<SpriteRenderer>().enabled = false;　//見た目をなくす

                isOpened = true; //扉が開いたという状態にする 

            }
        }
            }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //触れた相手がPlayerかつ扉があいていれば
        if(collision.gameObject.tag == "Player" && isOpened)
        {
            SceneManager.LoadScene(sceneName);　//Bossの部屋に行く
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
