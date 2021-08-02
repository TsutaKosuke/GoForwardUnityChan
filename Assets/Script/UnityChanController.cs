using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    //アニメーションをするためのコンポーネントを入れる
    Animator animator;
    //Unityちゃんを移動させるコンポーネントを入れる(追加）
    Rigidbody2D rigid2D;
    //地面の位置
    private float groundLevel = -3.0f;
    //ジャンプ速度の減衰(追加）
    private float dump = 0.8f;
    //ジャンプの速度(追加）
    float jumpVelocity = 20;

    //ゲームオーバーになる位置（追加２）
    private float deadLine = -9;

    // Start is called before the first frame update
    void Start()
    {
        //アニメーターのコンポーネントを取得する
        this.animator = GetComponent<Animator>();
        //Rigidbody2Dのコンポーネントを取得する(追加）
        this.rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //走るアニメーションを再生するために、Animatorのパラメータを調節する
        this.animator.SetFloat("Horizontal", 1);
        //着地しているかどうかを調べる
        bool isGround = (transform.position.y > this.groundLevel) ? false : true;
        this.animator.SetBool("isGround", isGround);

        //ジャンプ状態の時にはボリュームを０にする（追加３）
        GetComponent<AudioSource>().volume = (isGround) ? 1 : 0;

        //着陸状態でクリックされた場合(追加）
        if (Input.GetMouseButtonDown(0) && isGround)
        {
            //上方向に力をかける(追加）
            this.rigid2D.velocity = new Vector2(0, this.jumpVelocity);
        }
        //ジャンプをやめたら上方向への速度を減衰する(追加）
        if (Input.GetMouseButton(0) == false)
        {
            if (this.rigid2D.velocity.y > 0)
            {
                this.rigid2D.velocity *= this.dump;
            }
        }
        //デッドラインを超えた場合ゲームオーバーにする（追加２）
        if (transform.position.x < this.deadLine)
        {
            //UIControllerのGameOver関数を呼び出して画面上に「GameOver」と表示する（追加２）
            GameObject.Find("Canvas").GetComponent<UIController>().GameOver();

            //Unityちゃんを破棄する
            Destroy(gameObject);
        }
    }
}
