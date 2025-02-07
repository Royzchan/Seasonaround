using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;


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

public class PlayerController_2D : MonoBehaviour, IDamageable
{
    private Rigidbody _rb;

    //現在なんの動物かどうか
    private Animal _nowAnimal = Animal.Normal;
    public Animal NowAnimal {  get { return _nowAnimal; } } 
    [SerializeField, Header("プレイヤーの移動速度")]
    private float _speed = 5.0f;

    [SerializeField, Header("プレイヤーの最高速度")]
    private float _maxSpeed = 5.0f;

    [SerializeField, Header("プレイヤーの泳ぐ速度")]
    private float _swimSpeed = 5.0f;

    [SerializeField, Header("プレイヤーのジャンプ力")]
    private float _jumpPower = 5.0f;

    [SerializeField, Header("スタミナの最大値")]
    private float _maxStamina = 100;
    [SerializeField, Header("スタミナの使用値")]
    private float _useFrameStamina = 20f;
    [SerializeField, Header("スタミナ回復量")]
    private float _healStamina = 5f;
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

    [SerializeField, Header("標準落下速度(正確にはちょっと違う)")]
    private float _defaultDrag;
    [SerializeField, Header("張り付き状態の抵抗")]
    private float _wallDrag = 5f;
    [SerializeField, Header("水中内落下速度")]
    private float _waterDrag;
    //当たったことのある(変身可能な)動物
    private List<Animal> _hitAnimals = new();

    [SerializeField, Header("このステージで変身可能な動物(キー割り当て   0:↑,1:→,2:↓,3:←)")]
    private Animal[] _stageAnimals = new Animal[4];
    //変身キーが押されている
    private bool _canChangeAnimal = false;

    private Vector3 _preGroundPos = new Vector3();
    int _selectAnimalNum = -1;
    Animator _animator;

    public Animator Animator { get { return _animator; } }

    [SerializeField]
    SelectUIScript _selectUI;
    [SerializeField]
    StaminaUIScript _staminaUI;
    [SerializeField, Header("動物のモデル")]
    private GameObject[] _animalModels;
    [SerializeField, Header("変身用の煙")]
    private ParticleSystem _smokeParticle;
    bool _isGround = false;
    public bool IsGround { get { return _isGround; } }

    public bool IsSwimming { get => _isSwimming; set => _isSwimming = value; }

    bool _isSwimming = false;
    bool _isGrap = false;
    bool _isMoveGrap = false;
    bool _usedStamina = false;
    GrapGimmick _grapData = null;
    HingeJoint _joint = null;
    [Header("～インプット関係～")]
    [SerializeField, Header("横移動のキーコン")]
    private InputAction _moveXAction;

    [SerializeField, Header("↓入力のキーコン")]
    private InputAction _underAction;

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

    [SerializeField, Header("プレイヤーのHP")]
    private int _hp;

    [SerializeField, Header("HPの最大値")]
    int _maxHp;

    [SerializeField, Header("揺らすカメラ")]
    public GameObject _movecamera;
    private float moveCameraX;
    private float moveCameraY;
    [SerializeField]
    private Vector3 _cameraDistance = new Vector3(0f, 0f, 0f);

    [SerializeField, Header("下攻撃用のタイムライン")]
    private TimelineAsset _underAttackTimeline;

    [SerializeField, Header("横攻撃用のタイムライン")]
    private TimelineAsset _sideAttackTimeline;
    // ゲームオーバースクリプトを参照
    public GameOverScript gameOverScript;

    private float s;
    private float v;


    // 有効化
    private void OnEnable()
    {
        InputActionEnable();
    }

    // 無効化
    private void OnDisable()
    {
        InputActionDisable();
    }

    public void InputActionEnable()
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
        _underAction?.Enable();
    }

    public void InputActionDisable()
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
        _underAction?.Disable();
    }

    public void MoveStop(Vector3 zoomObjPos)
    {
        //会話になった時の停止用
        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 0.6f);
        LookZoomObj(zoomObjPos);
    }

    void LookZoomObj(Vector3 zoomObjPos)
    {
        if (zoomObjPos.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (zoomObjPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public int Hp
    {
        get { return _hp; }
        set
        {
            _hp = Mathf.Clamp(value, 0, _maxHp);

            if (_hp <= 0)
            {
                Death();
            }
        }
    }

    void Start()
    {
        RestartManager rm = FindAnyObjectByType<RestartManager>();
        if (rm != null)
        {
            transform.position = rm.GetRestartPosition();
        }

        Camera.main.transform.position = transform.position + _cameraDistance;
        _movecamera = Camera.main.gameObject;
        _preGroundPos = transform.position;
        //リジッドボディを取得
        _rb = GetComponent<Rigidbody>();
        //モデルのActiveなやつからAnimatorを取得
        foreach (var body in _animalModels)
        {

            if (body != null && body.activeSelf)
            {
                _animator = body.GetComponent<Animator>();
                break;
            }
        }
        _staminaUI = FindAnyObjectByType<StaminaUIScript>();
        //スタミナを最大値をセット
        _nowStamina = _maxStamina;

        Hp = _maxHp;

        moveCameraX = 1;
        moveCameraY = 1;

    }

    void Update()
    {

        //X軸のキー入力を保存
        _inputValueX = _moveXAction.ReadValue<float>();
        if (_inputValueX != 0)
        {
            //壁張り付き中は振りむかないように
            if (_rb.useGravity)
            {
                transform.localScale = new Vector3(_inputValueX * Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z);
            }

            if (!_isGrap)
            {
                _animator.Play("Walk");
            }

        }

        //ジャンプのボタンが押されたら
        if (_jumpAction.WasPressedThisFrame())
        {
            if (!_isSwimming)
            {
                //ジャンプした回数がジャンプ可能回数より低かったら
                if (_jumpNum < _canJumpNum)
                {
                    //上に力を加える
                    _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                    _rb.AddForce(transform.rotation * new Vector3(0f, _jumpPower, 0f), ForceMode.Impulse);
                    //ジャンプした回数をプラス
                    _jumpNum++;
                    //ジャンプ中の判定をtrueに
                    _jumpNow = true;
                }
            }
            else
            {
                //水中ならそのまま
                _rb.AddForce(new Vector3(0f, _jumpPower, 0f), ForceMode.Impulse);
            }
        }

        //動物の固有能力のボタンが押されていたら
        if (_animalAbilityAction.IsPressed())
        {
            //動物の固有能力を使う
            AnimalAction();
        }
        if (_animalAbilityAction.WasReleasedThisFrame())
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
            _rb.useGravity = true;
            _isGrap = false;
            Destroy(_joint);
            _joint = null;
        }
        //ジャンプ中だったら
        //if (_jumpNow)
        //{
            
        //}

        // Rayをプレイヤーの下方に飛ばす
        Ray ray = new Ray(_animalModels[(int)_nowAnimal].transform.position, -transform.up);
        //Debug.DrawRay(transform.position, -transform.up * 0.01f, Color.yellow,0.1f);
        RaycastHit hit;

        // レイキャストを実行して、何かに当たったかを確認
        if (Physics.Raycast(ray, out hit, 0.2f))
        {

            //レイが敵に当たっていたら
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //敵を倒せるように
                _canDefeatEnemy = true;
            }
            //違う何かに当たっていたら
            else if (hit.collider.CompareTag("Ground"))
            {
                //敵を倒せる判定をfalseに
                _canDefeatEnemy = false;
                JumpReset();
            }
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
            DebugAddHitAnimal(_selectAnimalNum);
        }
        //変身ボタンを押したとき(水中では変身不可)
        if (_changeAction.IsPressed())
        {

            if (!_isSwimming)
            {
                _canChangeAnimal = true;
                _selectUI.gameObject.SetActive(true);
            }
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

        if (_usedStamina)
        {
            if (_isGround)
            {
                _usedStamina = false;
            }

        }
        else
        {
            //回復
            _nowStamina += _healStamina * Time.deltaTime;
        }
        _nowStamina = Mathf.Clamp(_nowStamina, 0, _maxStamina);
        _staminaUI.GaugeUpdate(_nowStamina, _maxStamina);
    }

    private void FixedUpdate()
    {
        if (!_isSwimming && _isGround)
        {
            _preGroundPos = transform.position;
        }
        float speed = _speed;
        if (_isSwimming)
        {
            speed = _swimSpeed;
        }
        //速度を横移動の値に
        if (Mathf.Sign(_rb.velocity.x) == Mathf.Sign(_inputValueX))
        {
            if (Mathf.Abs(_rb.velocity.x) < _maxSpeed)
            {
                _rb.velocity += transform.rotation * new Vector3(_inputValueX * speed, 0f, 0f);
            }
        }
        else
        {
            _rb.velocity += transform.rotation * new Vector3(_inputValueX * speed, 0f, 0f);
        }
        //if(!_isGrap)
        //{
        //    _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -_maxSpeed, _maxSpeed), _rb.velocity.y) + _windPower;
        //}

        WindPowDown();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //敵を倒せる判定がtrueだったら
            if (_canDefeatEnemy)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if (_nowAnimal == Animal.Gecko)
            {
                _isSwimming = true;
                _rb.drag = _waterDrag;
            }
        }
        if (other.CompareTag("Death"))
        {
            Death();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //ツタに当たってる間その情報を取得
        if (other.CompareTag("Grap") && _grapData == null)
        {
            _grapData = other.GetComponent<GrapGimmick>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _isSwimming = false;
            //抵抗をもとに戻す
            _rb.drag = _defaultDrag;
            //追加でジャンプできないように
            _jumpNum = _canJumpNum;
        }
        //移動アニメーション中はストップ
        else if (other.CompareTag("Grap") && !_isMoveGrap)
        {
            _grapData = null;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            _isGround = false;
        }
    }

    void DebugAddHitAnimal(int _selectAnimal)
    {
#if UNITY_EDITOR
        //初期値なら処理を行わない
        if (_selectAnimal == -1) return;
        //hitAnimalsに入っていない&controllが押されている
        if (Input.GetKey(KeyCode.LeftControl) && !_hitAnimals.Contains(_stageAnimals[_selectAnimal]))
        {
            _hitAnimals.Add(_stageAnimals[_selectAnimal]);
            _selectUI.SetHitAnimal(_selectAnimal);
        }
#endif
    }

    private void JumpReset()
    {
        _jumpNum = 0;
        _jumpNow = false;
        _isFly = false;
        _isGround = true;
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

    IEnumerator MoveGrapPosition()
    {
        //animation中は停止
        _isMoveGrap = true;
        _rb.isKinematic = true;
        //空中で一旦静止(演出、いらない気がしないでもない)
        yield return new WaitForSeconds(0.15f);
        Vector3 prePos = transform.position;
        Vector3 grapPos = _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos;
        float timer = 0f;
        //距離が0.3f,又は時間が0.8fをすぎるまで繰り返す
        while (true)
        {
            timer += Time.deltaTime;
            //deltaTimeの分待機(この部分・・・なんか変・・・)
            yield return null;
            grapPos = _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos;
            //移動
            transform.position = Vector3.Lerp(prePos, grapPos, timer / 0.8f);

            if (timer > 0.8f || Vector3.Distance(transform.position, _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos) < 0.3f)
            {
                //位置をgrapPosに固定
                transform.position = _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos;
                transform.rotation = _grapData.transform.rotation;
                _rb.isKinematic = false;
                break;
            }
        }
        //animation終了
        _isMoveGrap = false;
        CreateGrapJoint();
    }

    void CreateGrapJoint()
    {
        if (_grapData != null)
        {
            _joint = gameObject.AddComponent<HingeJoint>();
            _joint.axis = new Vector3(0, 0, 1f);
            _joint.connectedBody = _grapData.gameObject.GetComponent<Rigidbody>();
        }

    }
    //動物の固有アクション
    private void AnimalAction()
    {
        switch (_nowAnimal)
        {
            //スライム
            case Animal.Normal:
                //グラップ可能かつjointが空
                if (_joint == null && !_isGrap && _grapData != null)
                {
                    //transform.position = _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos;
                    _isGrap = true;
                    StartCoroutine(MoveGrapPosition());
                }
                else if (_joint != null && _grapData != null)
                {
                    //ローテーションの固定
                    transform.rotation = _grapData.transform.rotation;
                }
                break;
            //ゴリラ
            case Animal.Colobus:
                //_animator.Play("Attack");
                var playable = _animalModels[(int)Animal.Colobus].GetComponent<PlayableDirector>();
                if (_underAction.IsPressed())
                {
                    playable.playableAsset = _underAttackTimeline;
                }
                else
                {
                    playable.playableAsset = _sideAttackTimeline;
                }
                playable.Play();
                break;

            //トカゲ
            case Animal.Gecko:
                float rayLength = 0.7f;
                //右方向レイ
                Ray ray = new Ray(transform.position, Vector3.right);
                Debug.DrawRay(transform.position, Vector3.right * rayLength, Color.red, 2f);
                RaycastHit hit;
                if (_nowStamina <= 0)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
                    _rb.useGravity = true;
                }
                // レイキャストを実行して、何かに当たったかを確認
                else if (Physics.Raycast(ray, out hit, rayLength, 1 << 8))
                {
                    //Debug.Log("→当たってるよ");
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                    _rb.useGravity = false;
                    _rb.velocity *= _wallDrag * Time.deltaTime;
                    _nowStamina -= _useFrameStamina * Time.deltaTime;
                    _usedStamina = true;
                }
                else
                {
                    //←方向レイ
                    ray = new Ray(transform.position, Vector3.left);
                    // レイキャストを実行して、何かに当たったかを確認
                    if (Physics.Raycast(ray, out hit, rayLength, 1 << 8))
                    {
                        //Debug.Log("←当たってるよ");
                        transform.rotation = Quaternion.Euler(new Vector3(180, 180f, 90f));
                        _rb.useGravity = false;
                        _rb.velocity *= _wallDrag * Time.deltaTime;
                        _nowStamina -= _useFrameStamina * Time.deltaTime;
                        _usedStamina = true;
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
                        _rb.useGravity = true;
                    }
                }

                break;

            //雀
            case Animal.Sparrow:
                //_rb.velocity = new Vector3((_speed * _inputValueX), _flySpeed, 0f);
                _rb.AddForce(new Vector3(0, _flySpeed, 0));
                _isFly = true;
                break;
        }
    }

    //動物に触れた時に返信する処理
    public void ChangeAnimal(Animal animal)
    {
        //_animalModels[(int)_nowAnimal].SetActive(false);
        _nowAnimal = animal;
        //_animalModels[(int)_nowAnimal].SetActive(true);
        //モデルの配列を確認していく
        for (int i = 0; i < _animalModels.Length; i++)
        {
            //もし返信する動物のモデルになったら
            if (i == (int)animal)
            {
                //その動物のモデルをセット
                _animalModels[i].SetActive(true);
                //その動物のアニメーターを取得
                _animator = _animalModels[i].GetComponent<Animator>();
            }
            //それ以外の動物だった場合
            else if (_animalModels[i] != null)
            {
                //動物のモデルを消す
                _animalModels[i].SetActive(false);
            }

        }
        _smokeParticle.Play();
        _hitAnimals.Add(animal);
        for (int i = 0; i < _stageAnimals.Length; ++i)
        {
            if (_stageAnimals[i] == animal)
            {
                _selectUI.SetHitAnimal(i);
                break;
            }
        }
        SetState((int)animal);
    }

    private void SetState(int dataNum)
    {
        _maxSpeed = AnimalData.animalDatas[dataNum].Speed;
        _jumpPower = AnimalData.animalDatas[dataNum].JumpPower;
    }

    public void InputChangeAnimal(Animal animal)
    {

        //前のオブジェクトをfalse
        _animalModels[(int)_nowAnimal].SetActive(false);
        //変更
        _nowAnimal = animal;
        _smokeParticle.Play();
        _animalModels[(int)_nowAnimal].SetActive(true);
        _animator = _animalModels[(int)_nowAnimal].GetComponent<Animator>();
        SetState((int)animal);
    }


    public void Damage(int value)
    {
        Hp -= value;
        StartCoroutine(CameraShake());
    }

    public void Death()
    {
        // GameOverScriptを呼び出してゲームオーバー処理を実行
        if (gameOverScript != null)
        {
            gameOverScript.SendMessage("GameOver");
        }
        else
        {
            Debug.LogError("GameOverScriptが設定されていません！");
        }

        _movecamera = null;

        //// プレイヤーオブジェクトを破壊
        //Destroy(gameObject);
    }

    IEnumerator CameraShake()
    {
        for (int i = 0; i < 10; i++)
        {
            _movecamera.transform.Translate(moveCameraX, moveCameraY, 0);
            moveCameraX *= -1;
            moveCameraY *= -1;
            yield return new WaitForSeconds(0.01f);
        }
    }
}

public interface IDamageable
{
    public void Damage(int value);
    public void Death();
}
