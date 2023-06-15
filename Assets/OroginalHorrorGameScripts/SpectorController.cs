using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 敵キャラ操作クラス ver - alpha 2023/06/06
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpectorController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))//Playerとぶつかったら
        {
            Destroy(this.gameObject);//このオブジェクトの破棄
        }
    }
}
