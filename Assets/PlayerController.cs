using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(PlayerInput))]
[RequireComponent (typeof(CapsuleCollider))]
/// <summary>
/// プレイヤー操作用のクラス ver - alpha
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary> InputSystem の PlayerInputコンポーネント </summary>
    PlayerInput _playerInput;

    /// <summary> キャラ移動に使う </summary>
    Rigidbody _rigidbody = default;

    /// <summary> キャラ移動に使うコライダー </summary>
    CapsuleCollider _capsuleCollider = default;

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
    private bool _flashLightIsOn = true;

    /// <summary> プレイヤーがアイテムを拾った時 </summary>
    private bool _pickkingNow = false;

    /// <summary> プレイヤーがリロードキーを押したとき </summary>
    private bool _reloadNow = false;

    /// <summary> 次に使うバッテリー </summary>
    [SerializeField] private GameObject _currentBattery = null;
    
    /// <summary> 今使うバッテリー </summary>
    [SerializeField] private GameObject _nextBattery = null;
    
    /// <summary> 次に使うバッテリー </summary>
    [SerializeField] private GameObject[] _batteriesInInventry = null;

    /// <summary> SE用のオブジェクトのタグが紐づけされてるオブジェクト格納用 </summary>
    GameObject[] _compareTagSoundEffect;
    /// <summary> プレイヤー歩行時の効果音再生用のオブジェクト格納用 </summary>
    GameObject _walkingSoundEffectObject = null;

    /// <summary> アイテムポーチのオブジェクト </summary>
    [SerializeField] GameObject _itemPoach;
    /// <summary> 懐中電灯のホルダーオブジェクト </summary>
    [SerializeField] GameObject _playerHolder;

    //以下プレイヤーの操作に必要なクラス
    PlayerMover _playerMover;//プレイヤーの移動用クラス
    PlayerLooker _playerLooker;//Player振り向き用クラス
    MouseCursoreLocker _cursoreLocker;//カーソルロッククラス
    FlashLightController _flashLightController;//懐中電灯操作用クラス
    PlayerCameraController _playerCameraController;//Playerカメラの操作用クラス
    WalkingSoundEffectController _walkingSoundEffectController;//歩行SE操作用クラス

    /// <summary> アイテムポーチに必要な昨日を盛り込んでいるクラス </summary>
    InventrySystem _inventrySystem;

    private void Start()
    {
        //各必要なクラスのインスタンス化（以下）
        _itemPoach = GameObject.FindGameObjectWithTag("Inventry_Poach");//インベントリーのオブジェクトの検索
        _playerHolder = GameObject.FindGameObjectWithTag("Player_Holder");//フラッシュライトのオブジェクトの検索

        _cursoreLocker = new MouseCursoreLocker();//マウスカーソルのロック用クラス使用
        _cursoreLocker.MouseCursoreLock();//マウスカーソルのロック

        _flashLightController = new FlashLightController();//懐中電灯の操作クラス
        _playerMover = new PlayerMover();//プレイヤーの操作クラス
        _playerLooker = new PlayerLooker();//プレイヤーの振り向きの操作クラス
        _playerCameraController = new PlayerCameraController();//プレイヤーの視野操作クラス
        _walkingSoundEffectController = new WalkingSoundEffectController();//プレイヤー歩行SEの操作クラス
        //インベントリシステムクラス
        _inventrySystem = new InventrySystem();


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

        //懐中電灯のオブジェクトの検索
        { _flashLight = this._inventrySystem.GetRandomChildObjectWithTag(_playerHolder, "FlashLight"); }
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
        _playerCameraController.PlayerCameraMove(this._playerCamera, this._lookVector, this._lookSpeed, 45f);//カメラ上下回転
        _walkingSoundEffectController.WalkingSoundEffectPlayStatusSet(this._walkingSoundEffectObject, this._moveVector);//歩行効果音操作

        if(this._nextBattery == null)
        {
            this._batteriesInInventry = this._inventrySystem.GetChildObjectsWithTag(this._itemPoach, "Battery");
            if (this._batteriesInInventry != null)
            {
                this._nextBattery = this._batteriesInInventry[0];
            }
        }

        if (this._reloadNow)
        {
            _flashLightController.FlashLightBatteryCharge(100f);
            Destroy(this._nextBattery);
            this._nextBattery = null;
        }
    }

    /// <summary>
    /// PlayerInputからのプレイヤー移動用のVector2の受取関数
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != null)
        {
            this._moveVector = context.ReadValue<Vector2>().normalized;//ベクトルの正規化をして単位ベクトルに変換かけて斜め移動の不自然さを解消
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
            this._lookVector = context.ReadValue<Vector2>().normalized;//ベクトルの正規化をして単位ベクトルに変換かけて斜めふり向きの不自然さを解消
            //print($"{_lookVector} : InputDevice - InputValue : Look");//デバッグ用
        }
    }

    /// <summary>
    /// 懐中電灯をつける入力受取関数
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        this._illuminate = context.ReadValueAsButton();//マウスボタン左のクリックの状態の格納
        //print($"{_illuminate} : is Illuminate");//デバッグ用
    }

    /// <summary>
    /// インタラクトの入力があったとき
    /// </summary>
    /// <param name="context"></param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        this._pickkingNow = context.ReadValueAsButton();//キーボードEの入力値を格納
        //Debug.Log("pikkedNOW" + _pickkingNow);
    }
    
    /// <summary>
    /// リロードの入力があったとき
    /// </summary>
    /// <param name="context"></param>
    public void OnReload(InputAction.CallbackContext context)
    {
        this._reloadNow = context.ReadValueAsButton();//キーボードEの入力値を格納
        //Debug.Log("pikkedNOW" + _pickkingNow);
    }

    private void OnTriggerStay(Collider other)
    {
        if (TriggeredObjectCheckToPickUp(other.gameObject))
        {
            Debug.Log($"{other.name} Is Can Pick");
            if (this._pickkingNow)
            {
                if (other.gameObject.CompareTag("Battery"))
                { 
                    if (this._nextBattery != null)
                    {
                        AttachItemToPoach(other.gameObject, this._itemPoach);
                    }
                    else
                    {
                        this._nextBattery = other.gameObject;
                    }
                }
                Debug.Log($"Picked {other.name}");
            }
        }
    }

    /// <summary>
    /// リストに登録してあるタグと照合して引数のオブジェクトがリストにあるものと同じタグを持っていればTrueを返す
    /// </summary>
    /// <param name="triggeredObject"></param>
    /// <returns></returns>
    private bool TriggeredObjectCheckToPickUp(GameObject triggeredObject)
    {
        List<string> _objectTagList = new List<string>() { "FlashLight", "Battery", "Empty" };//拾えるアイテムのタグのリスト
        bool returnValue = true;
        if (triggeredObject != null)//ひとまずnullチェック
        {
            string objectTag = triggeredObject.tag;//タグの送信
            foreach (string tag in _objectTagList)//foreach検索
            {
                Debug.Log("TriggeredObjectSearchingTAG : " + tag);
                if (objectTag == tag)//もし一致したら
                {
                    returnValue = true;//返り値はtrueでこれ以降は検索をかけないのでbreakする。余計に検索を掛けると想定しない値が返る
                    break;
                }
                else
                {
                    returnValue = false;
                }
            }
        }
        Debug.Log("TriggeredObjectRETURN : " + returnValue);
        return returnValue;
    }

    /// <summary>
    /// アイテムポーチにアイテムをアタッチしてアイテムポーチの配下にする
    /// </summary>
    /// <param name="itemObject"></param>
    /// <param name="itemPoachObject"></param>
    private void AttachItemToPoach(GameObject itemObject,GameObject itemPoachObject)
    {
        _inventrySystem.MakeParenToChild(itemObject, itemPoachObject.transform);
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
    private static float _batteryLife = 10f;//バッテリーの寿命の初期値
    public void FlushLightLight(Light light, bool lightOrnot)
    {
        if (_batteryLife > 0)//バッテリー残量があるとき
        {
            light.enabled = lightOrnot;//点灯の入力値に応じてコンポーネントの有効無効を切り替え
            _batteryLife -= Time.deltaTime;//通常消費
            if(lightOrnot) _batteryLife -= Time.deltaTime * 1.5f;//点灯をすれば消費量は増えるから点灯時余分に消費
            Debug.Log($"current battery life : {_batteryLife}");
        }
        else
        {
            light.enabled = false;//点灯できないようにする
            Debug.Log($"current battery is death");
        }
    }

    public void FlashLightBatteryCharge(float batteryPower)
    {
        _batteryLife = batteryPower;//バッテリーの寿命の回復をする
        Debug.Log($"battery charged");
    }

    public float GetBatteryLife()
    {
        return _batteryLife;
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
        if(moveVector != Vector2.zero)//歩行の入力の有無で歩行用SEのオブジェクトの有効無効を切り替え
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}