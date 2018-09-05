using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanControllerroll : MonoBehaviour
{
    //アニメーションをいれるためのコンポーネント
    private Animator myAnimator;
    //unityちゃんを移動させるコンポーネント
    private Rigidbody myRigibody;
    //前進するための力
    private float forwardForce = 800.0f;
    //左右に移動するための力
    private float trunForce = 500.0f;
    //ジャンプするための力
    private float upForce = 500.0f;
    //左右の移動できる範囲
    private float movableRange = 3.4f;
    //動きを減速させる係数
    private float coefficient = 0.95f;

    //ゲーム終了の判定
    private bool isEnd = false;

    //ゲーム終了時に表示するテキスト
    private GameObject stateText;
    //スコアを表示するテキスト
    private GameObject scoreText;
    //得点
    private int score = 0;
    //左下ボタン押下の判定
    private bool isLButtonDown = false;
    //右下ボタン押下の判定
    private bool isRButtonDown = false;
    // Use this for initialization
    void Start()
    {
        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();

        //走るアニメーションを取得
        this.myAnimator.SetFloat("Speed", 1);

        //Rigidbodyコンポーネントを取得
        this.myRigibody = GetComponent<Rigidbody>();

        //シーン中のstatetextオブジェクトを取得
        this.stateText = GameObject.Find("GameResultText");

        //シーン中のscoreTextオブジェクトを取得
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了ならunityちゃんの動きを減衰する
        if (this.isEnd)
        {
            this.forwardForce *= this.coefficient;
            this.trunForce *= this.coefficient;
            this.upForce *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }
        //unityちゃんに前方向の力を加える
        this.myRigibody.AddForce(this.transform.forward * this.forwardForce);

        //Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
        if((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x)
        {
            //左に移動
            this.myRigibody.AddForce(-this.trunForce, 0, 0);
        }else if((Input.GetKey(KeyCode.RightArrow) ||this.isRButtonDown)&& this.transform.position.x < this.movableRange)
        {
            //右に移動
            this.myRigibody.AddForce(this.trunForce, 0, 0);
        }

        //unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
        if ((Input.GetKey(KeyCode.LeftArrow)) && -this.movableRange < this.transform.position.x)
        {
            //左に移動
            this.myRigibody.AddForce(-this.trunForce, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && this.transform.position.x < this.movableRange)
            //右に移動
            this.myRigibody.AddForce(this.trunForce, 0, 0);

        //jumpステートの場合はJumpにfalseをセットする
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }
        //jumpしていないときにスペースが押されたらジャンプする
        if (Input.GetKeyDown(KeyCode.Space) && this.transform.position.y < 0.5f)
        {
            //ジャンプアニメを再生
            this.myAnimator.SetBool("Jump", true);
            //unityちゃんに上方向の力を加える
            this.myRigibody.AddForce(this.transform.up * this.upForce);
        }
    }
        //トリガーモードで他のオブジェクトと接触した処理
        void OnTriggerEnter(Collider other)
        {
            //障害物に衝突した場合
            if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
            {
                this.isEnd = true;
            //stateTextにGameOverを表示
            this.stateText.GetComponent<Text>().text = "GAMEOVER";
            }
            //ゴール地点に到達した場合
            if (other.gameObject.tag == "GoalTag")
            {
                this.isEnd = true;
            //stateTextにGAMECLEARを表示
            this.stateText.GetComponent<Text>().text = "CLEAR";
            }
            //コインに衝突した場合
            if(other.gameObject.tag =="CoinTag")
        {
            //スコアを加算
            this.score += 10;
            //scoreText獲得した点数を表示
            this.scoreText.GetComponent<Text>().text = "Score" + this.score + "pt";
            //パーティクル再生
            GetComponent<ParticleSystem>().Play();
            //接触したオブジェクトの排除
            Destroy(other.gameObject);
        }
        }
    //ジャンプボタンを押した処理
    public void GetMyJumpButtonDown()
    {
        if(this.transform.position.y < 0.5f)
        {
            this.myAnimator.SetBool("Jump", true);
            this.myRigibody.AddForce(this.transform.up * this.upForce);
        }
    }
    //左ボタンを押し続けた場合
    public void GetMyLeftButtonDown()
    {
        this.isLButtonDown = true;
    }
    //左ボタンを離した場合
    public void GetMyLeftButtonUp()
    {
        this.isLButtonDown = false;
    }
    //右ボタンを押し続けた場合
    public void GetMyRightButtonDown()
    {
        this.isRButtonDown = true;
    }
    //右ボタンを離した場合
    public void GetMyRightButtonUp()
    {
        this.isRButtonDown = false;
    }

    }