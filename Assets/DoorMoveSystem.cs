using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ニゲラレナイ のドアを空けるために使うクラス 使い捨て。つかいまわしNG
/// </summary>
public class DoorMoveSystem : MonoBehaviour
{
    [SerializeField] GameObject _doorObjectOpened, _doorObjectClosed;

    public bool _DoorIsOpened = false;

    private void Awake()
    {
        this._doorObjectOpened.SetActive(false);
    }

    private void Update()
    {
        Debug.Log($"Door Transform Is {this._doorObjectOpened.transform.localPosition}");
        InteractDoor(this._DoorIsOpened);
    }
    
    /// <summary>
    /// 空けたら閉めることができなくなる仕様です
    /// </summary>
    /// <param name="isOpen"></param>
    public void InteractDoor(bool isOpen)
    {
        if(isOpen)
        {
            this._doorObjectOpened.SetActive(true);
            this._doorObjectClosed.SetActive(false);
            if (TryGetComponent<Collider>(out Collider component))//アタッチされてるCollliderコンポーネントをすべて破棄。
            {
                Collider[] components = GetComponents<Collider>();
                foreach(Collider c in components)
                {
                    Destroy(c);
                }
            }
        }
    }
    
}
