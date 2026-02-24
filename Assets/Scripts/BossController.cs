using UnityEngine;

public class BossController : MonoBehaviour
{
    public int hp = 10;//ボスの体力
    public float reactionDistance = 10.0f;//攻撃距離
    public GameObject bulletPrefab;//生成オブジェクト
    public float shootSpeed = 5.0f;//弾のスピード
    public float bossSpeed = 0.05f;//反復するボススピード
    Animator animator;
    GameObject player;

    public GameObject gate;//弾を生成する位置を持つ子オブジェクト

    bool inDamage;//ダメージ管理フラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //ダメージ中であれば点滅※returnはしない
        if (inDamage)
        {
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //体力が残っている場合
        if (hp > 0)
        {
            if (player != null)
            {
                Vector2 playerPos = player.transform.position;//その時のPlayerの位置
                float dist = Vector2.Distance(transform.position, playerPos);//PlayerとBossの距離の差
                animator.SetBool("InAttack", dist <= reactionDistance);//第二変数で基準の距離に入っているか判断
            }
            else
            {
                animator.SetBool("InAttack", false);
            }
        }
    }

    //何かに衝突
    void OnCollisionEnter2D(Collision2D collision)
    {
        //ダメージ中じゃなければ
        if (!inDamage)
        {
            if (collision.gameObject.tag == "Arrow")//ぶつかった相手がArrowだったら
            {
                //ぶつかった相手のゲームオブジェクトが持ってるArrowController　scriptを取得
                ArrowController arrow = collision.gameObject.GetComponent<ArrowController>();
                hp -= arrow.attackPower;//体力を減少
                inDamage = true;//ダメージ中
                Invoke("DamageEnd", 0.25f);//0.25秒後にダメージフラグ解除
                
                //体力がなくなったら死
                if (hp <= 0)
                {
                    //Boss　Collider2つ持っているのである分だけゲットして配列に格納
                    CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
                    colliders[0].enabled = false;
                    colliders[1].enabled = false;

                    animator.SetTrigger("IsDead");//アニメのIsDeadパラメーター(Trigger)を発動
                    Invoke("BossSpriteOff", 1.0f);//Deadアニメが終わったころSpriteRenderも無効
                }
            }
        }
    }
    //反復移動
    private void FixedUpdate()
    {
        float val = Mathf.Sin(Time.time);

        transform.position -= new Vector3(val * bossSpeed, 0, 0);
    }

    //ダメージ中フラグOFF
    void DamageEnd()
    {
        inDamage = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void Attack()
    {
        //
        if (player != null)
        {
            //PlayerとGateのX座標の差、Y座標の差
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            //Xの差（底辺）、Yの差（高さ）から逆算して角度を求める
            float rad = Mathf.Atan2(dy, dx);
            //オイラー角に変換
            float angle = rad * Mathf.Rad2Deg;
            //生成されるプレハブの角度をあらかじめ変数ｒに計算
            Quaternion r = Quaternion.Euler(0, 0, angle);
            //Gateオブジェクト位置bullet生成と情報取得
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);

            //（底辺と高さの差の情報はもっているが）あらためて長辺を1としたときの割合で底辺と高さを取得
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            //取得したx,yのデータを元にVector3型として値を生成　※ただし値が弱いので変数shootSpeedで倍増
            Vector3 v = new Vector3(x, y) * shootSpeed;

            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();//生成した弾自身のRigidbodyを取得
            rbody.AddForce(v, ForceMode2D.Impulse);//あらかじめ計算したVector3の方に力を加える
        }
    }
    //SpriteRenderを無効化、3秒後にDestory
    void BossSpriteOff()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Invoke("BossDestroy", 3.0f);
    }
    //ステージクリアのためにGoalメソッドを発動、Bossを削除
    void BossDestroy()
    {
        player.GetComponent<PlayerController>().Goal();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, reactionDistance);
    }


}
