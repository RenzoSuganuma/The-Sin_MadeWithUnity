using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 敵キャラ操作用クラス ver - alpha
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpectorController : MonoBehaviour
{
    /// <summary> 敵オブジェクト操作用のRigidbody </summary>
    Rigidbody _rb;

    /// <summary> プレイヤーオブジェクト </summary>
    GameObject _playerGameObject = null;

    [SerializeField] private bool _isMadNow = true;

    private void Start()
    {
        this._rb = GetComponent<Rigidbody>();//Rigidbodyの取得
        this._playerGameObject = GameObject.FindGameObjectWithTag("PlayerCamera");//プレイヤーオブジェクトの検索

        { this._rb.useGravity = false; }//Rigidbody初期化
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.LookAt(this._playerGameObject.transform.position, this._playerGameObject.transform.up);//プレイヤーオブジェクトを向く
        if(this._isMadNow)
            this._rb.AddForce(this.transform.forward * .3f, ForceMode.VelocityChange);//正面に移動
        else
            this._rb.velocity = this.transform.forward * .6f;//正面に移動
        //Debug.Log(_isMadNow);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided WITH Player");
            Destroy(this.gameObject);
        }
    }
}
