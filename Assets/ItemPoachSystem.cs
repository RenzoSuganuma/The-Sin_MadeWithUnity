using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// インベントリシステム用のクラス ver - alpha
/// </summary>
public class ItemPoachSystem : MonoBehaviour
{
    /// <summary> 子オブジェクトのマネージャクラス </summary>
    ChildObjectsManager _objectsManager;
    [SerializeField] GameObject[] _gameObject_DEBUGGING;//デバッグ用変数
    // Start is called before the first frame update
    void Start()
    {
        _objectsManager = new ChildObjectsManager();//クラスの使用
    }

    // Update is called once per frame
    void Update()
    {
        _gameObject_DEBUGGING = _objectsManager.GetChildObjects(this.gameObject);
        //_objectsManager.MakeChildToParent(_gameObject_DEBUGGING);
    }
}

/// <summary>
/// インベントリ用のオブジェクトの配下にある子オブジェクトのマネージャークラス
/// </summary>
public class ChildObjectsManager
{
    /// <summary>
    /// 子オブジェクトの取得をしてGameObject型で返す関数非アクティブでもOK
    /// </summary>
    /// <param name="parentObject"></param>
    /// <returns></returns>
    public GameObject[] GetChildObjects(GameObject parentObject)
    {
        // 子オブジェクトのトランスフォームを格納する配列作成
        var _children = new Transform[parentObject.transform.childCount];//添え字が変動する
        int childIndex = 0;//添え字

        // 子オブジェクトを格納する配列作成
        GameObject[] _returnObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する
        int convertingIndex = 0;//添え字

        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in parentObject.transform)
        {
            if (child != null)
                _children[childIndex++] = child;
            //Debug.Log("child index" + childIndex);
        }
        //子オブジェクトを順番にトランスフォーム型からゲームオブジェクト型に変換、返り値に格納
        foreach (Transform child in _children)
        {
            if (child != null)
                _returnObjects[convertingIndex++] = child.gameObject;
            //Debug.Log("conv index" + convertingIndex);
        }

        return _returnObjects;
    }

    /// <summary>
    /// 親子関係を切る関数 引数はGameObject型
    /// </summary>
    /// <param name="childObject"></param>
    public void MakeChildToParent(GameObject childObject)
    {
        if(childObject != null)//オブジェクトの中身のチェック
        {
            childObject.gameObject.transform.parent = null;
        }
    }

    /// <summary>
    /// 親子関係を切る関数 引数はGameObject[]型
    /// </summary>
    /// <param name="childObjects"></param>
    public void MakeChildToParent(GameObject[] childObjects)
    {
        if (childObjects != null)//オブジェクトの中身のチェック
        {
            for(int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].gameObject.transform.parent = null;
            }
        }
    }
}