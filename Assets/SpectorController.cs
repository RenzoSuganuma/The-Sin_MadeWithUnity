using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 敵キャラ操作用クラス ver - alpha
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class SpectorController : MonoBehaviour
{
    /// <summary> 敵オブジェクト操作用のRigidbody </summary>
    Rigidbody _rb;

    /// <summary> プレイヤーオブジェクト </summary>
    GameObject _playerGameObject = null;

    /// <summary> 敵オブジェクト操作用のCapsuleCollider </summary>
    CapsuleCollider _capsuleCollider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();//Rigidbodyの取得
        _capsuleCollider = GetComponent<CapsuleCollider>();//CapsuleColliderの取得
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");//プレイヤーオブジェクトの検索

        { _rb.useGravity = false; }//Rigidbody初期化
        { _capsuleCollider.isTrigger = true; }//CapsuleCollider初期化
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.LookAt(_playerGameObject.transform.position, _playerGameObject.transform.up);//プレイヤーオブジェクトを向く
        _rb.velocity = this.transform.forward * .5f;//正面に移動
    }
}
