using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ItemPoachSystem : MonoBehaviour
{
    ChildObjectsManager _objectsGetter;
    [SerializeField] GameObject[] _test = null;
    // Start is called before the first frame update
    void Start()
    {
        _objectsGetter = new ChildObjectsManager();
    }

    // Update is called once per frame
    void Update()
    {
        _test = _objectsGetter.GetChildObjects(gameObject);
        _objectsGetter.MakeChildToParent(_test);
    }
}

/// <summary>
/// インベントリ用のオブジェクトの配下にある子オブジェクトのマネージャークラス
/// </summary>
public class ChildObjectsManager
{
    /// <summary>
    /// 子オブジェクトの取得をしてGameObject型で返す関数
    /// </summary>
    /// <param name="parentObject"></param>
    /// <returns></returns>
    public GameObject[] GetChildObjects(GameObject parentObject)
    {
        // 子オブジェクトを格納する配列作成
        var _children = new Transform[parentObject.transform.childCount];//添え字が変動する
        int childIndex = 0;
        GameObject[] _returnObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する
        int convertingIndex = 0;
        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in parentObject.transform)
        {
            if(child != null)
                _children[childIndex++] = child;
            //Debug.Log("child index" + childIndex);
        }
        //子オブジェクトを順番にトランスフォーム型からゲームオブジェクト型に変換、返り値に格納
        foreach(Transform child in _children)
        {
            if(child != null)
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
        if(childObject != null)
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
        if (childObjects != null)
        {
            for(int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].gameObject.transform.parent = null;
            }
        }
    }
}