using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(PlayerInput))]
[RequireComponent (typeof(CapsuleCollider))]
/// <summary>
/// プレイヤー操作用のクラス ver - alpha
/// </summary>
/// 
public class PlayerController : MonoBehaviour
{
    /// <summary> InputSystem の PlayerInputコンポーネント </summary>
    PlayerInput _playerInput;

    /// <summary> キャラ移動に使う </summary>
    Rigidbody _rigidbody;

    /// <summary> キャラ移動に使うコライダー </summary>
    CapsuleCollider _capsuleCollider;

    /// <summary> 移動ベクトル入力値の代入先 </summary>
    Vector2 _moveVector = Vector2.zero;
    /// <summary> 振り向きベクトル入力値の代入先 </summary>
    Vector2 _lookVector = Vector2.zero;

    /// <summary> これが真の値の時には懐中電灯はついているようにする </summary>
    private bool _illuminate = !true;
    /// <summary> Lightコンポーネント　ここではプレイヤーの懐中電灯のコンポーネント </summary>
    Light _light;
    /// <summary> プレイヤーの懐中電灯のオブジェクト </summary>
    GameObject _flashLight;

    /// <summary> 移動速度float型引数 </summary>
    [Tooltip("移動速度float型引数")]
    [SerializeField] private float _moveSpeed = 1.0f;
    /// <summary> 移動速度float型引数 </summary>
    [Tooltip("振り向き速度float型引数")]
    [SerializeField] private float _lookSpeed = 1.0f;

    /// <summary> プレイヤーのカメラのオブジェクト </summary>
    GameObject _playerCamera;

    /// <summary> 懐中電灯をつけてるか判定フラグ : 敵オブジェクトの有効無効判断用 読み込み専用 </summary>
    public bool _flashLightIsOn = true;

    /// <summary> SE用のオブジェクトのタグが紐づけされてるオブジェクト格納用 </summary>
    GameObject[] _compareTagSoundEffect;
    /// <summary> プレイヤー歩行時の効果音再生用のオブジェクト格納用 </summary>
    GameObject _walkingSoundEffectObject = null;

    PlayerMover _playerMover;
    PlayerLooker _playerLooker;
    MouseCursoreLocker _cursoreLocker;
    FlashLightController _flashLightController;
    PlayerCameraController _playerCameraController;
    WalkingSoundEffectController _walkingSoundEffectController;

    private void Start()
    {
        if (TryGetComponent<PlayerInput>(out PlayerInput player))//PlayerInputコンポーネントがあるかチェック、あるならゲットする
        {
            _playerInput = player;//入手したコンポーネントの送信
            _playerInput.defaultActionMap = "Player";//DefaultMapの初期化
        }
        else
            Debug.LogError("The Component PlayerInput Is Not Found");

        if (TryGetComponent<Rigidbody>(out Rigidbody rigid))//Digidbodyコンポーネントがあるかチェック、あるならゲットする
        {
            _rigidbody = rigid;//入手したコンポーネントの送信
            //各パラメーターの初期化
            _rigidbody.mass = 1.0f;
            _rigidbody.drag = 1.5f;
            _rigidbody.angularDrag = .05f;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = !true;
            _rigidbody.freezeRotation = true;
        }
        else
            Debug.LogError("The Component Rigidbody Is Not Found");

        if (TryGetComponent<CapsuleCollider>(out CapsuleCollider capsule))//Digidbodyコンポーネントがあるかチェック、あるならゲットする
        {
            //各パラメーターの初期化
            _capsuleCollider = capsule;
            _capsuleCollider.radius = .3f;
            _capsuleCollider.height = 1.5f;
        }
        else
            Debug.LogError("The Component CapsuleCollider Is Not Found");


        { _flashLight = GameObject.FindGameObjectWithTag("FlashLight"); }//懐中電灯のオブジェクトの検索
        if (_flashLight.TryGetComponent<Light>(out Light light))//Lightコンポーネントの検索
            _light = light;
        else
            Debug.LogError("The Component Light Is Not Found");


        { _playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera"); }


        _compareTagSoundEffect = GameObject.FindGameObjectsWithTag("SoundEffect");//効果音用のタグがアタッチされているオブジェクト（配列）を検索
        foreach (GameObject @object in _compareTagSoundEffect)//ボトムからトップまでの要素のチェック
        {
            if (@object.CompareTag("SoundEffect") && @object.name == "Walk_SE")//特定の条件を満たすオブジェクトを見つけた場合
            {
                _walkingSoundEffectObject = @object; // 変数にオブジェクトを格納
                break; // 条件を満たすオブジェクトが見つかったらループを終了
            }
        }

        _cursoreLocker = new MouseCursoreLocker();//マウスカーソルのロック用クラス使用
        _cursoreLocker.MouseCursoreLock();//マウスカーソルのロック

        _flashLightController = new FlashLightController();
        _playerMover = new PlayerMover();
        _playerLooker = new PlayerLooker();
        _playerCameraController = new PlayerCameraController();
        _walkingSoundEffectController = new WalkingSoundEffectController();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        { _flashLightIsOn = _illuminate; }//ほかクラスから懐中電灯のステータスをスコープするため

        _playerMover.PlayerMove(this.gameObject, this._rigidbody, this._moveVector, this._moveSpeed);//移動
        _playerLooker.PlayerLooking(this.gameObject.transform, this._lookVector, this._lookSpeed);//振り向き
        _flashLightController.FlushLightLight(this._light, this._illuminate);//懐中電灯のONOFF
        _playerCameraController.PlayerCameraMove(this._playerCamera, this._lookVector, this._lookSpeed, 30f);//カメラ上下回転
        _walkingSoundEffectController.WalkingSoundEffectPlayStatusSet(this._walkingSoundEffectObject, this._moveVector);//歩行効果音操作
    }

    /// <summary>
    /// PlayerInputからのプレイヤー移動用のVector2の受取関数
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != null)
        {
            _moveVector = context.ReadValue<Vector2>().normalized;//ベクトルの正規化をして単位ベクトルに変換かけて斜め移動の不自然さを解消
            //print($"{_moveVector} : InputDevice - InputValue");//デバッグ用
        }
    }

    /// <summary>
    /// PlayerInputからのプレイヤー振り向き用のVector2の受取関数
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != null)
        {
            _lookVector= context.ReadValue<Vector2>().normalized;//ベクトルの正規化をして単位ベクトルに変換かけて斜めふり向きの不自然さを解消
            //print($"{_lookVector} : InputDevice - InputValue : Look");//デバッグ用
        }
    }

    /// <summary>
    /// 懐中電灯をつける入力受取関数
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        _illuminate = context.ReadValueAsButton();//マウスボタン左のクリックの状態の格納
        //print($"{_illuminate} : is Illuminate");//デバッグ用
    }
}

/// <summary>
/// キャラ移動用のクラス
/// </summary>
public class PlayerMover
{
    public void PlayerMove(GameObject playerObject, Rigidbody rigidbody, Vector2 moveVector, float moveSpeed)
    {
        if (moveVector != Vector2.zero)
        {
            rigidbody.WakeUp();//何かしらの入力値があった力を加える
            rigidbody.AddForce(playerObject.transform.forward * moveVector.y * moveSpeed * .01f, ForceMode.Impulse);//前後の移動　正面が変動するのでその時々の正面基準で動く
            rigidbody.AddForce(playerObject.transform.right * moveVector.x * moveSpeed * .01f, ForceMode.Impulse);//左右の移動　正面が変動するのでその時々の正面基準で動く
        }
        else
        {
            rigidbody.Sleep();//何も入力値がないので力は加えない
        }
    }
}

/// <summary>
/// プレイヤー振り向き移動用クラス
/// </summary>
public class PlayerLooker
{
    public void PlayerLooking(Transform transform, Vector2 lookVector, float lookSpeed)
    {
        transform.Rotate(new Vector3(0, lookVector.x * lookSpeed * .5f, 0));
    }
}

/// <summary>
/// マウスカーソル固定用クラス
/// </summary>
public class MouseCursoreLocker
{
    public void MouseCursoreLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}

/// <summary>
/// 懐中電灯操作クラス
/// </summary>
public class FlashLightController
{
    private static float _batteryLife = 10f;
    public void FlushLightLight(Light light, bool lightOrnot)
    {
        if (_batteryLife > 0)
        {
            light.enabled = lightOrnot;
            _batteryLife -= Time.deltaTime;
            if(lightOrnot) _batteryLife -= Time.deltaTime * 1.5f;
            Debug.Log($"current battery life : {_batteryLife}");
        }
        else
        {
            light.enabled = false;
            Debug.Log($"current battery is death");
        }
    }

    public void FlashLightBatteryCharge(float batteryPower)
    {
        _batteryLife = batteryPower;
        Debug.Log($"battery charged");
    }
}

/// <summary>
/// カメラの制御クラス
/// </summary>
public class PlayerCameraController
{
    private float _minLimit = 0f;
    private Vector3 _localAngle = Vector3.zero;
    public void PlayerCameraMove(GameObject gameObject, Vector2 lookVector, float lookSpeed, float maxLimit)
    {
        //上方向に向かって回転＝負の数　下方向に向かって回転＝正の数 -180[Deg] ~ 180[Deg]
        _minLimit = 360 - maxLimit;
        _localAngle = gameObject.transform.localEulerAngles;
        _localAngle.x += -lookVector.y * lookSpeed * .1f;
        if (_localAngle.x > maxLimit && _localAngle.x < 180)//maxLimit[Deg] ~ 180[Deg]
        {
            //Debug.Log("MaxLimit!");
            _localAngle.x = maxLimit;
        }

        if (_localAngle.x < _minLimit && _localAngle.x > 180)//minLimit[Deg] ~ 180[Deg]
        {
            //Debug.Log("MinLimit!");
            _localAngle.x = _minLimit;
        }

        gameObject.transform.localEulerAngles = _localAngle;
    }
}

/// <summary>
/// プレイヤー歩行時のSE再生用クラス
/// </summary>
public class WalkingSoundEffectController
{
    public void WalkingSoundEffectPlayStatusSet(GameObject gameObject,Vector2 moveVector)
    {
        if(moveVector != Vector2.zero)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

/// <summary>
/// タイマーのクラス
/// </summary>
public class Timer
{
    public float targetTime = 10.0f; // 目標時間（秒）
    private float currentTime = 0.0f; // 現在の経過時間

    private bool isTimerRunning = false; // タイマーが動作中かどうか
    public bool isTimerPaused = false;

    public void UpdateTimer()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime; // 経過時間を更新
            Debug.Log($"CurrentTime : {currentTime}");
            if (currentTime >= targetTime)
            {
                // 目標時間を経過したら、メッセージを表示
                Debug.Log("Time's up!");

                // タイマーを停止する（任意の処理を行う場合はここに追加）
                isTimerRunning = false;
                isTimerPaused = !isTimerRunning;//公開変数の値設定
            }
        }
    }

    public void StartTimer()
    {
        // タイマーを開始する（任意の処理を行う場合はここに追加）
        isTimerRunning = true;
        isTimerPaused = !isTimerRunning;//公開変数の値設定

        // 経過時間をリセット
        currentTime = 0.0f;
    }
}