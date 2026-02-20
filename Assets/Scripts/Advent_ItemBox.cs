using UnityEngine;
using UnityEngine.SceneManagement;

public class Advent_ItemBox : MonoBehaviour
{
    public Sprite openImage; //開いたときの画像
    public GameObject itemPrefab; //アイテムのプレハブ
    public bool isClosed = false;
    public AdventItemType type = AdventItemType.None; //アイテムの種類


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //もし自分のタイプがKeyだったら
        if (type == AdventItemType.Key)
        {
            //もしGameManagerのkeyGotディクショナリーの該当シーンの記憶がTrueだったら※取得済み
            if (GameManager.keyGot[SceneManager.GetActiveScene().name] == true)
            {
                {
                    //close状態をfalse
                    isClosed = false;
                    //見た目をオープンの絵にすること
                    GetComponent<SpriteRenderer>().sprite = openImage;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClosed && collision.gameObject.tag == "Player")
        {
            //宝箱の絵をOpenの絵に変更
            GetComponent<SpriteRenderer>().sprite = openImage;
            //closeのフラグを解除
            isClosed = false;
            //その場に変数指定したプレハブオブジェクトを生成
            //(もし変数にプレハブオブジェクトが指定されていれば)
            if (itemPrefab != null)
            {
                Instantiate(
                    itemPrefab,
                    transform.position,
                    Quaternion.identity);
            }
        }
    }
}

