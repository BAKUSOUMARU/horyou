using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("基本設定")]
    [SerializeField] 
    [Tooltip("基本の速さ")]
    float _defaultSpeed = 5;

    Rigidbody2D rb;
    enum VerticalHorizontalChange
    {
        Vertical,
        Horizontal
    }

    [Header("反転設定")]
    [SerializeField]
    [Tooltip("移動方向 : 縦 / 横")]
    VerticalHorizontalChange vhChange;

    enum TurnAround
    {
        Time,
        WallTouch,
        FloorContact
    }

    [SerializeField]
    [Tooltip("反転方法 : 時間 / 壁に当たった時 / 床に触れてないとき")]
    TurnAround turnAround;

    [SerializeField]
    [Tooltip("反転方法が「Time」の場合の 反転時間(s)")]
    float _turnTime = 10;

    float _countTime = 0;

    bool _wallTouch = false;
    bool _floorContact = false;

    [SerializeField]
    [Tooltip("壁を検知する当たり判定")]
    List<GameObject> _wallDetection = new List<GameObject>();

    [SerializeField]
    [Tooltip("床を検知する当たり判定")]
    List<GameObject> _floorDetection = new List<GameObject>();
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Movement();
    }
    void Movement()
    {
        switch (vhChange)
        {
            case VerticalHorizontalChange.Horizontal:
                rb.velocity = new Vector2(_defaultSpeed,rb.velocity.y);
                break;
            case VerticalHorizontalChange.Vertical:
                rb.velocity = new Vector2(rb.velocity.x, _defaultSpeed);
                break;
        }

        switch (turnAround)
        {
            case TurnAround.Time:
                _countTime++;
                if(_turnTime * 60 <= _countTime)
                {
                    _defaultSpeed = _defaultSpeed * -1;
                    _countTime = 0;
                }
            break;

            case TurnAround.WallTouch:
                _wallTouch = true;
                for(int i =0; i < _wallDetection.Count ; i++)
                {
                    _wallDetection[i].gameObject.SetActive(true);
                    print("c");
                }
            break;

            case TurnAround.FloorContact:
                _floorContact = true;
                for (int i = 0; i < _floorDetection.Count; i++)
                {
                    _floorDetection[i].gameObject.SetActive(true);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_wallTouch)
        {
            _defaultSpeed = _defaultSpeed * -1;   
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (_floorContact)
        {
            _defaultSpeed = _defaultSpeed * -1;
        }
    }
}
