using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class World_UIController : MonoBehaviour
{
    //各エントランスごとに解錠か未開錠か
    public static Dictionary<int, bool> keyOpened;

    public TextMeshProUGUI keyText;
    int currentKeys;
    public TextMeshProUGUI arrowText;
    int currentArrows;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //WorldMapシーンに存在するEntranceオブジェクトを全て取得する
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Entrance");

        //リストがない時の情報取得とセッティング
        if (keyOpened == null)
        {
            keyOpened = new Dictionary<int, bool>(); // 最初に初期化が必要

            //集めてきたEntranceオブジェクトを全点検
            for (int i = 0; i < obj.Length; i++)
            {
                //EntranceControllerコンポーネントを取得
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    //帳簿(keyOpenedディクショナリー)に変数doorNumberとopenedの情報を記録
                    keyOpened.Add(
                        entranceController.doorNumber,
                        entranceController.opened
                    );
                }
            }
        }
        //プレイヤーの位置
        player = GameObject.FindGameObjectWithTag("Player");
        //暫定のプレイヤーの位置
        //Vector2 currentPlayerPos = new Vector2(0, 0);
        Vector2 currentPlayerPos = Vector2.zero;
        //GameManagerに記録されているcurrentDoorNumberと同じdoorNumberを持つEntranceを探す
        for (int i = 0; i < obj.Length; i++)
        {
            //EntranceControllerの変数doorNumberとGameManagerに記録されているcurrentDoorNumberが同じかどうかチェック
            if (obj[i].GetComponent<EntranceController>().doorNumber == GameManager.currentDoorNumber)
            {
                //暫定のプレイヤーの位置を一致したEntranceの位置に書き換え
                currentPlayerPos = obj[i].transform.position;
            }
        }
        //最終的にcurrentPlayerPosの座標がプレイヤーの座標になる
        player.transform.position = currentPlayerPos;

    }

    // Update is called once per frame
    void Update()
    {
        //把握していた鍵の数とGameManagerの鍵の数が違うとき正しい数になるようUIを更新
        if (currentKeys != GameManager.keys)
        {
            currentKeys = GameManager.keys;
            keyText.text = currentKeys.ToString();
        }
        //把握していた矢の数とGameManagerの矢の数が違うとき正しい数になるようUIを更新
        if (currentArrows != GameManager.arrows)
        {
            currentArrows = GameManager.arrows;
            arrowText.text = currentArrows.ToString();
        }

    }
}

