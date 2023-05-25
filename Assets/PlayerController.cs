using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary> InputSystem の PlayerInputコンポーネント </summary>
    PlayerInput _playerInput;

    /// <summary> キャラ移動に使う </summary>
    Rigidbody _rigidbody;

    /// <summary> 移動ベクトル入力値の代入先 </summary>
    Vector2 _moveVector = Vector2.zero;
    /// <summary> 振り向きベクトル入力値の代入先 </summary>
    Vector2 _lookVector = Vector2.zero;

    /// <summary> 移動速度float型引数 </summary>
    [Tooltip("移動速度float型引数")]
    [SerializeField] private float _moveSpeed = 1.0f;
    /// <summary> 移動速度float型引数 </summary>
    [Tooltip("振り向き速度float型引数")]
    [SerializeField] private float _lookSpeed = 1.0f;


    private void Start()
    {
        if (TryGetComponent<PlayerInput>(out PlayerInput player))//PlayerInputコンポーネントがあるかチェック、あるならゲットする
        {
            _playerInput = player;//入手したコンポーネントの送信
            _playerInput.defaultActionMap = "Player";//DefaultMapの初期化
        }

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
    }

    private void FixedUpdate()
    {
        new PlayerMover(this.gameObject, _rigidbody, _moveVector, _moveSpeed);//移動
        new PlayerLooker(this.gameObject.transform, _lookVector, _lookSpeed);
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
}

/// <summary>
/// キャラ移動用のクラス
/// </summary>
public class PlayerMover
{
    public PlayerMover(GameObject playerObject, Rigidbody rigidbody, Vector2 moveVector, float moveSpeed)
    {
        if (moveVector != Vector2.zero)
        {
            rigidbody.WakeUp();
            rigidbody.AddForce(playerObject.transform.forward * moveVector.y *  moveSpeed * .01f, ForceMode.Impulse);
            rigidbody.AddForce(playerObject.transform.right * moveVector.x *  moveSpeed * .01f, ForceMode.Impulse);
        }
        else
        {
            rigidbody.Sleep();
        }
    }
}

public class PlayerLooker
{
    public PlayerLooker(Transform transform, Vector2 lookVector, float lookSpeed)
    {
        transform.Rotate(new Vector3(0, lookVector.x * lookSpeed * .5f, 0));
    }
}