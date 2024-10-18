using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public enum Animal
{
    Normal,//通常状態
    Colobus,//ゴリラ
    Gecko,//トカゲ
    Herring,//魚
    Muskrat,//リス
    Pudu,//鹿
    Sparrow,//鳥
    Squid,//イカ
    Taipan//蛇
}

public class PlayerController_2D : MonoBehaviour
{
    private Rigidbody _rb;

    //現在なんの動物かどうか
    private Animal _nowAnimal = Animal.Normal;

    [SerializeField, Header("プレイヤーの移動速度")]
    private float _speed = 5.0f;

    [SerializeField, Header("プレイヤーのジャンプ力")]
    private float _jumpPower = 5.0f;

    [SerializeField, Header("プレイヤーのHP")]
    private int _hp;

    //入力の値を保存する変数
    private float _inputValueX = 0;

    //ジャンプの回数
    private int _jumpNum = 0;

    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;

    //ジャンプ中かどうかの判定
    private bool _jumpNow = false;

    //触れた敵を倒せるかどうか
    private bool _canDefeatEnemy = false;

    [SerializeField, Header("風力減速(掛ける量)"), Range(0.9f, 0.99f)]
    private float _windLatePower;
    private Vector3 _windPower;

    [SerializeField, Header("動物のモデル")]
    private GameObject[] _animalModels;

    [Header("～インプット関係～")]
    [SerializeField, Header("横移動のキーコン")]
    private InputAction _moveXAction;

    [SerializeField, Header("ジャンプのキーコン")]
    private InputAction _jumpAction;

    // 有効化
    private void OnEnable()
    {
        // InputActionを有効化
        _moveXAction?.Enable();
        _jumpAction?.Enable();
    }

    // 無効化
    private void OnDisable()
    {
        // 自身が無効化されるタイミングなどで
        _moveXAction?.Disable();
        _jumpAction?.Disable();
    }

    void Start()
    {
        //リジッドボディを取得
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        //X軸のキー入力を保存
        _inputValueX = _moveXAction.ReadValue<float>();

        //ジャンプのボタンが押されたら
        if (_jumpAction.WasPressedThisFrame())
        {
            //ジャンプした回数がジャンプ可能回数より低かったら
            if (_jumpNum < _canJumpNum)
            {
                //上に力を加える
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                //ジャンプした回数をプラス
                _jumpNum++;
                //ジャンプ中の判定をtrueに
                _jumpNow = true;
            }
        }

        //ジャンプ中だったら
        if (_jumpNow)
        {
            // Rayをプレイヤーの前方に飛ばす
            Ray ray = new Ray(transform.position, -transform.up);

            RaycastHit hit;

            // レイキャストを実行して、何かに当たったかを確認
            if (Physics.Raycast(ray, out hit))
            {
                //レイが敵に当たっていたら
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    //敵を倒せるように
                    _canDefeatEnemy = true;
                }
                //違う何かに当たっていたら
                else
                {
                    //敵を倒せる判定をfalseに
                    _canDefeatEnemy = false;
                }
            }
            //何にも当たっていなかったら
            else
            {
                //敵を倒せる判定をfalseに
                _canDefeatEnemy = false;
            }
            //Debug.DrawRay(ray.origin, ray.direction, UnityEngine.Color.red, 5.0f);
        }
    }

    private void FixedUpdate()
    {
        //速度を横移動の値に
        _rb.velocity = new Vector3((_speed * _inputValueX), _rb.velocity.y, 0f) + _windPower;
        WindPowDown();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //敵を倒せる判定がtrueだったら
            if (_canDefeatEnemy)
            {
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
            _jumpNow = false;
        }
    }

    //ダメージ
    public void HitDamage()
    {
        //HPを減らす
        _hp--;
    }

    public void SetWindPow(Vector3 addPower)
    {
        _windPower = addPower;
    }

    private void WindPowDown()
    {
        //_windPowerが一定値まで下がったなら0にしてreturn
        if (Mathf.Abs(_windPower.x) < 0.1f)
        {
            _windPower.x = 0;
        }
        if (Mathf.Abs(_windPower.y) < 0.1f)
        {
            _windPower.y = 0;
        }
        if (_windPower == Vector3.zero) return;

        //減速
        _windPower *= _windLatePower;

        //↓勇気のあるものがはずせ、私にはこれが限界だった
        if (_windPower.y != 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _windPower.y);
        }

        //符号が異なるとき
        if (Mathf.Sign(_inputValueX) != Mathf.Sign(_windPower.x) && _inputValueX != 0)
        {
            //さらに追加で減速
            _windPower.x *= _windLatePower;
            //_speedより小さい力になれば0に
            if (Mathf.Abs(_windPower.x) < _speed)
            {
                _windPower.x = 0;
            }
        }
    }

    //動物の固有アクション
    private void AnimalAction()
    {

    }

    //動物に触れた時に返信する処理
    public void ChangeAnimal(Animal animal)
    {
        //モデルの配列を確認していく
        for (int i = 0; i < _animalModels.Length; i++)
        {
            //もし返信する動物のモデルになったら
            if (i == (int)animal)
            {
                //その動物のモデルをセット
                _animalModels[i].SetActive(true);
            }
            //それ以外の動物だった場合
            else
            {
                //動物のモデルを消す
                _animalModels[i].SetActive(false);
            }
        }
    }

    private void SetState()
    {
        _speed = AnimalData.animalDatas[0].Speed;
        _jumpPower = AnimalData.animalDatas[0].JumpPower;
    }
}
