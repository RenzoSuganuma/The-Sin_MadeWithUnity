using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryTextContainer : MonoBehaviour
{
    [SerializeField] private TextAsset _textAsset;
    [SerializeField] public string _text;

    private void Awake()
    {
        this._text = this._textAsset.text;
    }
}
