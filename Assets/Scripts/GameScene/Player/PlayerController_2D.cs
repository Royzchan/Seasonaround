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
    private Animal _nowAnimal = Animal.Squid;

    [SerializeField, Header("プレイヤーの移動速度")]
    private float _speed = 5.0f;

    [SerializeField, Header("プレイヤーのジャンプ力")]
    private float _jumpPower = 5.0f;

    [SerializeField, Header("プレイヤーのHP")]
    private int _hp;

    public int HP { get { return _hp; } }

    [SerializeField, Header("スタミナの最大値")]
    private float _maxStamina = 100;

    private float _nowStamina;

    [SerializeField, Header("飛ぶスピード")]
    private float _flySpeed = 5.0f;

    //入力の値を保存する変数
    private float _inputValueX = 0;

    //ジャンプの回数
    private int _jumpNum = 0;

    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;

    //ジャンプ中かどうかの判定
    private bool _jumpNow = false;

    //飛んでいるかどうか
    private bool _isFly = false;

    //飛べるかどうか
    private bool _canFly = true;

    public bool ISFly { get { return _isFly; } }

    //触れた敵を倒せるかどうか
    private bool _canDefeatEnemy = false;

    [SerializeField, Header("風力減速(掛ける量)"), Range(0.9f, 0.99f)]
    private float _windLatePower;
    private Vector3 _windPower;

    //当たったことのある(変身可能な)動物
    private List<Animal> _hitAnimals = new();

    [SerializeField, Header("このステージで変身可能な動物(キー割り当て   0:↑,1:→,2:↓,3:←)")]
    private Animal[] _stageAnimals = new Animal[4];
    //変身キーが押されている
    private bool _canChangeAnimal = false;

    int _selectAnimalNum = -1;
    [SerializeField]
    SelectUIScript _selectUI;
    [SerializeField, Header("動物のモデル")]
    private GameObject[] _animalModels;

    [Header("～インプット関係～")]
    [SerializeField, Header("横移動のキーコン")]
    private InputAction _moveXAction;

    [SerializeField, Header("ジャンプのキーコン")]
    private InputAction _jumpAction;

    [SerializeField, Header("固有能力のキーコン")]
    private InputAction _animalAbilityAction;

    [SerializeField, Header("変身用のキーコン")]
    private InputAction _changeAction;

    [SerializeField, Header("方向入力のキーコン")]
    private InputAction _cursorUpAction;
    [SerializeField]
    private InputAction _cursorRightAction;
    [SerializeField]
    private InputAction _cursorDownAction;
    [SerializeField]
    private InputAction _cursorLeftAction;

    // 有効化
    private void OnEnable()
    {
        // InputActionを有効化
        _moveXAction?.Enable();
        _jumpAction?.Enable();
        _animalAbilityAction?.Enable();
        _changeAction?.Enable();
        _cursorUpAction?.Enable();
        _cursorRightAction?.Enable();
        _cursorDownAction?.Enable();
        _cursorLeftAction?.Enable();
    }

    // 無効化
    private void OnDisable()
    {
        // 自身が無効化されるタイミングなどで
        _moveXAction?.Disable();
        _jumpAction?.Disable();
        _animalAbilityAction?.Disable();
        _changeAction?.Disable();
        _cursorUpAction?.Disable();
        _cursorRightAction?.Disable();
        _cursorDownAction?.Disable();
        _cursorLeftAction?.Disable();
    }

    void Start()
    {
        //リジッドボディを取得
        _rb = GetComponent<Rigidbody>();

        //スタミナを最大値をセット
        _nowStamina = _maxStamina;
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

        //動物の固有能力のボタンが押されていたら
        if (_animalAbilityAction.IsPressed())
        {
            //動物の固有能力を使う
            AnimalAction();
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

        if (_canChangeAnimal)
        {
            //↑
            if (_cursorUpAction.WasPressedThisFrame())
            {
                _selectAnimalNum = 0;
                _selectUI.ScaleMove(_selectAnimalNum);
            }
            //→
            else if (_cursorRightAction.WasPressedThisFrame())
            {
                _selectAnimalNum = 1;
                _selectUI.ScaleMove(_selectAnimalNum);
            }
            //↓
            else if (_cursorDownAction.WasPressedThisFrame())
            {
                _selectAnimalNum = 2;
                _selectUI.ScaleMove(_selectAnimalNum);
            }
            //←
            else if (_cursorLeftAction.WasPressedThisFrame())
            {
                _selectAnimalNum = 3;
                _selectUI.ScaleMove(_selectAnimalNum);
            }
        }
        //変身ボタンを押したとき
        if (_changeAction.IsPressed())
        {
            _canChangeAnimal = true;
            _selectUI.gameObject.SetActive(true);
        }
        //離したとき
        else if (_canChangeAnimal)
        {


            //初期値でなければ
            if (_selectAnimalNum != -1)
            {
                foreach (var animal in _hitAnimals)
                {
                    if (animal == _stageAnimals[_selectAnimalNum])
                    {
                        InputChangeAnimal(_stageAnimals[_selectAnimalNum]);

                        break;
                    }
                }
            }
            _selectUI.ResetScale();
            _selectAnimalNum = -1;
            _selectUI.gameObject.SetActive(false);
            _canChangeAnimal = false;
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
            _isFly = false;
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
        switch (_nowAnimal)
        {
            //ゴリラ
            case Animal.Colobus:

                break;

            //トカゲ
            case Animal.Gecko:

                break;

            //雀
            case Animal.Squid:
                _rb.velocity = new Vector3((_speed * _inputValueX), _flySpeed, 0f);
                _isFly = true;
                break;
        }
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
            else if (_animalModels[i] != null)
            {

                //動物のモデルを消す
                _animalModels[i].SetActive(false);
            }
        }

        _hitAnimals.Add(animal);
        for (int i = 0; i < _stageAnimals.Length; ++i)
        {
            if (_stageAnimals[i] == animal)
            {
                _selectUI.SetHitAnimal(i);
                break;
            }
        }
    }

    private void SetState()
    {
        _speed = AnimalData.animalDatas[0].Speed;
        _jumpPower = AnimalData.animalDatas[0].JumpPower;
    }

    public void InputChangeAnimal(Animal animal)
    {
        _nowAnimal = animal;
        for (int i = 0; i < _animalModels.Length; i++)
        {
            //もし返信する動物のモデルになったら
            if (i == (int)animal)
            {
                //その動物のモデルをセット
                _animalModels[i].SetActive(true);
            }
            //それ以外の動物だった場合
            else if (_animalModels[i] != null)
            {
                //動物のモデルを消す
                _animalModels[i].SetActive(false);
            }
        }
    }
}
