using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction
{
    none,
    left,
    right
}

public class World_PlayerController : MonoBehaviour
{
    public float speed = 3.0f; //移動速度

     Vector2 moveVec = Vector2.zero; //InputSystemからの入力値
    float angleZ;//Playerの向き
    Rigidbody2D rbody;
    Animator animator;

    bool isActionButtonPressed;//ActionButtonが押されたらtrue
    public bool IsActionButtonPressed//同名の変数のプロパティ
    {
        get { return isActionButtonPressed; }
        set { isActionButtonPressed = value; }
    }

    void OnActionButton(InputValue value)
    {
        IsActionButtonPressed = value.isPressed; // ボタンが押され続けている間はtrue
    }

    //Inputsystemの入力値を変数moveVecにVector2形式で代入
    void OnMove(InputValue value)
    {
        moveVec = value.Get<Vector2>();
    }

    //Inputsystemの入力値からPlayerの角度を算出
    float GetAngle()
    {
        float angle = angleZ;
        if (moveVec != Vector2.zero)
        {
            float rad = Mathf.Atan2(moveVec.y, moveVec.x);
            angle = rad * Mathf.Rad2Deg;
        }
        return angle;
    }

    //その時のangleZからPlayerの向きを右か左か判断する
    Direction AngleToDirection()
    {
        Direction dir;
        if (angleZ >= -89 && angleZ <= 89)
        {
            dir = Direction.right;
        }
        else if (angleZ >= 91 && angleZ <= 269)
        {
            dir = Direction.left;
        }
        else
        {
            dir = Direction.none;
        }
        return dir;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        angleZ = GetAngle();
        Direction dir = AngleToDirection();
        //右向きならそのまま左向きなら逆
        if (dir == Direction.right)
        {
            transform.localScale = new Vector2(1, 1);//絵はそのまま
        }
        else if (dir == Direction.left)
        {
            transform.localScale = new Vector2(-1, 1);//絵を逆転
        }
        //移動キー/移動スティックが入力されていそうならRun
        if (moveVec != Vector2.zero)
        {
            animator.SetBool("Run", true);//Runパラメーターをtrue
        }
        else
        {
            animator.SetBool("Run", false);//RunパラメーターをFalse
        }

    }
    //実際にPlayerを動かす
    void FixedUpdate()
    {
        rbody.linearVelocity = moveVec * speed;
    }
}
