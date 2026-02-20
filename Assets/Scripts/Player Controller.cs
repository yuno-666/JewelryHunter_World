using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Rigidbody2D rbody;　　//Rigidbody2D 型の変数

    float axisH = 0.0f;　　//入力

    public float speed = 3.0f; //移動速度

    public float jump = 9.0f;//ジャンプ力

    public LayerMask groundLayer;//着地できるレイヤー

    bool goJump = false;//ジャンプ開始フラグ
    bool onGround = false;//地面フラグ


    // アニメーション対応
    Animator animator; // アニメーター

    //値はあくまでアニメーションクリップ名
    public string stopAnime = "Idle";
    public string moveAnime = "Run";
    public string jumpAnime = "Jump";
    public string goalAnime = "Goal";
    public string deadAnime = "Dead";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0; //スコア

    InputAction moveAction;//moveアクション
    InputAction jumpAction;//jumpアクション
    PlayerInput input;//PlayerInputコンポーネント
    GameManager gm;//GameManagerスクリプト

    public static int playerLife = 10; //プレイヤーの体力

    bool inDamage = false; //ダメージ管理フラグ

    //PlayerLifeの回復メソッド
    public static void PlayerRecovery(int life)
    {
        playerLife += life; //プレイヤーの体力を回復する
        if (playerLife > 10)//プレイヤーの体力が10を超えないようにする
        {
            playerLife = 10;
        }

    }

    static public void playerDamage(int damage)
    {
        playerLife -= damage;
        if (playerLife < 0)
        {
            playerLife = 0;
        }
    }

    void OnMove(InputValue value)
    {
        //取得した情報をVector2形式で抽出
        Vector2 moveInput = value.Get<Vector2>();
        axisH = moveInput.x; //そのX成分をaxisHに代入
        //Debug.Log("Move: " + axisH);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            goJump = true;
        }

    }


    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();  //Rigidbody2D を取ってくる
        animator = GetComponent<Animator>();        // Animator を取ってくる
        nowAnime = stopAnime;                       // 停止から開始する
        oldAnime = stopAnime;                       // 停止から開始する

        playerLife = 10; //プレイヤーの体力を10にする


        input = GetComponent<PlayerInput>();       // PlayerInput を取ってくる
        moveAction = input.currentActionMap.FindAction("Move"); // Move アクションを取得
        jumpAction = input.currentActionMap.FindAction("Jump"); // Jump アクション取得

        InputActionMap uiMap = input.actions.FindActionMap("UI"); //UIマップ取得
        uiMap.Disable(); // UI アクションマップを無効化

        // GameObject型の特定コンポーネントを探してくるメソッド
        gm = GameObject.FindFirstObjectByType<GameManager>();


    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.InGame || inDamage)
        {
            //もしダメージ管理フラグが立っていたら点滅処理
            if (inDamage)
            {
                float val = Mathf.Sin(Time.time * 50);
                if (val > 0)
                {


                    //Sin関数の角度に経過時間（一定リズムの値）を与えると等間隔でプラスマイの結果が得られる

                    GetComponent<SpriteRenderer>().enabled = true;
                }
                //等間隔でかわっているであろうValのをチェックし。プラスなら表示、マイナスなら非表示にする
                else
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }

            }
            return;//Updeteを中断
        }



        //地上判定
        onGround = Physics2D.CircleCast(transform.position,//発射位置
                0.2f,//円の半径
                Vector2.down,//発射方向
                0.0f,//発射距離
                groundLayer);//検出するレイヤー

        //if (Input.GetButtonDown("Jump"))//キャラクターをジャンプさせる
        //{
        //    goJump = true;//ジャンプフラグをたてる

        //}

        //if (jumpAction.WasPressedThisFrame())//キャラクターをジャンプさせる
        //{
        //    goJump = true;//ジャンプフラグをたてる
        //}
        //水平方向の入力をチェックする
        //axisH = Input.GetAxisRaw("Horizontal");

        // InputActionのPlayerマップのMoveアクションVector2値のx成分 
        //axisH = moveAction.ReadValue<Vector2>().x; 

        if (axisH > 0.0f)                           // 向きの調整
        {
            //Debug.Log("右おされている");
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            //Debug.Log("左おされている");
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }

        // アニメーション更新
        if (onGround)       // 地面の上
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime; // 停止中
            }
            else
            {
                nowAnime = moveAnime; // 移動
            }
        }
        else                // 空中
        {
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // アニメーション再生
        }


    }





    private void FixedUpdate()
    {
        //ゲームステータスがInGameでない、もしくはダメージ管理がtrue
        if (GameManager.gameState != GameState.InGame || inDamage)
        {
            return;
        }
        if (onGround || axisH != 0)//地面の上or速度が0でない
        {
            //速度を更新する
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)//地面の上でジャンプキーが押された
        {
            Vector2 jumpPw = new Vector2(0, jump);//ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);//瞬間的な力を加える
            goJump = false;//ジャンプフラグをおろす
        }
    }
    //接触

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();         // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // スコアアイテム
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();  // ScoreItemを得る			
            score = item.itemdeta.value;                // スコアを得る
            UIController ui = Object.FindFirstObjectByType<UIController>();      // UIControllerを探す
            if (ui != null)
            {
                ui.UpdateScore(score);                  // スコア表示を更新する
            }
            score = 0; //次に備えてスコアをリセット
            Destroy(collision.gameObject);              // アイテム削除する
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (!inDamage) //ダメージ中でなければ
            {
                GetDamage(collision.gameObject);

            }
        }
    }
    //ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        GameManager.gameState = GameState.GameClear;
        GameStop();             // ゲーム停止
    }

    //ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        GameManager.gameState = GameState.GameOver;
        GameStop();             // ゲーム停止
                                // ゲームオーバー演出
        GetComponent<CapsuleCollider2D>().enabled = false;      // 当たりを消す
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 上に少し跳ね上げる

        Destroy(gameObject, 2.0f); // 2秒後にヒエラルキーからオブジェクトを抹消
    }

    // プレイヤー停止
    void GameStop()
    {
        rbody.linearVelocity = new Vector2(0, 0);           // 速度を0にして強制停止

        //InputSystemのPlayerマップとUIマップの切り替え
        input.currentActionMap.Disable(); //いったん現状のPlayerマップを無効化
        input.SwitchCurrentActionMap("UI"); //アクションマップをUIに切り替え
        input.currentActionMap.Enable(); //UIマップを有効化
    }
    //UI表示にSubmitボタンが押されたら
    void OnSubmit(InputValue value)
    {
        //もしゲーム中でなければ
        if (GameManager.gameState != GameState.InGame)
        {
            //GameManagerスクリプトのGameEndメソッドの発動
            gm.GameEnd();
        }
    }


    //プレイヤーのaxisH()の値を取得
    public float GetAxisH()
    {
        return axisH;
    }
    //ダメージメソッド
    void GetDamage(GameObject target)
    {
        //プレイ中のみ発動
        if (GameManager.gameState == GameState.InGame)
        {
            playerLife -= 1;//体力減少
            if (playerLife > 0)//まだゲームオーバーじゃなければ
            {
                //相手と反対方向にノックバック
                rbody.linearVelocity = new Vector2(0, 0);
                Vector3 v = (transform.position - target.transform.position).normalized;
                rbody.AddForce(new Vector2(v.x * 4, v.y * 4), ForceMode2D.Impulse);
                //ダメージ管理フラグを立てる
                inDamage = true;
                //時間差でダメージ管理フラグを下ろす
                Invoke("DamageEnd", 0.25f);
            }
            else//PlayerLifeが0以下になったら
            {
                GameOver();
            }
        }

    }
    void DamageEnd()
    {
        inDamage = false;
        //ダメージ終了（点滅終了）と同時に確実に姿を表示させる
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
