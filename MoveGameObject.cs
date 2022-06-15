using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JoyCastle笔试题目：
/// ① move(GameObjct gameObject, Vector3 begin, Vector3 end, float time, bool pingpong){ }
/// 使 gameObject 在 time 秒内，从 begin 移动到 end，若 pingpong 为 true，则在结束时 使 gameObject 在 time 秒内从 end 移动到 begin，如此往复。
/// ② 在以上基础上实现 easeIn easeOut easeInOut 动画效果
///
/// 思路：
/// 设置个timer计时，pingpong决定timer的规则
/// 设置个根据不同缓动曲线求插值的方法，begin作为起始值，end作为结束值，timer/time作为插值的值
/// </summary>
public class MoveGameObject : MonoBehaviour
{
    public GameObject obj;

    private void Start()
    {
        // 初始化
        var beginPos = obj.transform.position;
        var endPos = beginPos + new Vector3(10, 0, 0);
        var time = 2.0f;

        // Move所有的情况如下：
        Move(obj, beginPos, endPos, time, false);
        // Move(obj, beginPos, endPos, time, true);
        // Move(obj, beginPos, endPos, time, false, EaseType.Linear);
        // Move(obj, beginPos, endPos, time, false, EaseType.EaseIn);
        // Move(obj, beginPos, endPos, time, false, EaseType.EaseOut);
        // Move(obj, beginPos, endPos, time, false, EaseType.EaseInOut);
        // Move(obj, beginPos, endPos, time, true, EaseType.Linear);
        // Move(obj, beginPos, endPos, time, true, EaseType.EaseIn);
        // Move(obj, beginPos, endPos, time, true, EaseType.EaseOut);
        // Move(obj, beginPos, endPos, time, true, EaseType.EaseInOut);
    }

    /// <summary>
    /// 根据easeType返回对应value值时的插值
    /// </summary>
    /// <param name="begin">初始Vector</param>
    /// <param name="end">末尾Vector</param>
    /// <param name="value">value范围[0,1]</param>
    /// <param name="easeType">缓动类型，默认为Linear</param>
    /// <returns></returns>
    private Vector3 VectorLerp(Vector3 begin, Vector3 end, float value, EaseType easeType = EaseType.Linear)
    {
        switch (easeType)
        {
            case EaseType.Linear:
                return Vector3.Lerp(begin, end, value);
            case EaseType.EaseIn:
                return (end - begin) * value * value + begin;
            case EaseType.EaseOut:
                return -(end - begin) * value * (value - 2) + begin;
            case EaseType.EaseInOut:
                value *= 2;
                if (value < 1)
                {
                    return (end - begin) / 2 * value * value + begin;
                }

                value--;
                return -(end - begin) / 2 * (value * (value - 2) - 1) + begin;
            default:
                return Vector3.Lerp(begin, end, value);
        }
    }

    private void Move(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong,
        EaseType easeType = EaseType.Linear)
    {
        StartCoroutine(MoveCoroutine(gameObject, begin, end, time, pingpong, easeType));
    }

    IEnumerator MoveCoroutine(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong,
        EaseType easeType)
    {
        var timer = 0f; //计时器
        var flipflop = true;

        while (0 <= timer && timer <= time)
        {
            timer += flipflop ? Time.deltaTime : -Time.deltaTime;

            if (timer >= time)
            {
                timer = time;
                if (pingpong)
                {
                    flipflop = !flipflop;
                }
            }

            if (timer <= 0)
            {
                timer = 0;
                if (pingpong)
                {
                    flipflop = !flipflop;
                }
            }

            var value = Mathf.Clamp(timer / time, 0, 1);
            gameObject.transform.position = VectorLerp(begin, end, value, easeType);

            yield return null;
        }

        // 第二版，只处理pingpong
        // while (pingpong)
        // {
        //     while (timer < time)
        //     {
        //         timer += Time.deltaTime;
        //
        //         var value = Mathf.Clamp(timer / time, 0, 1);
        //         gameObject.transform.position = VectorLerp(begin, end, value, easeType);
        //         yield return null;
        //     }
        //
        //     while (timer > time)
        //     {
        //         timer += Time.deltaTime;
        //
        //         var value = Mathf.Clamp((2 * time - timer) / time, 0, 1);
        //         gameObject.transform.position = VectorLerp(begin, end, value, easeType);
        //         if (timer >= 2 * time)
        //         {
        //             timer = 0f;
        //         }
        //
        //         yield return null;
        //     }
        // }


        // 第一版，只处理单向移动
        // while (timer < time)
        // {
        //     timer += Time.deltaTime;
        //
        //     var value = Mathf.Clamp(timer / time, 0, 1);
        //     gameObject.transform.position = VectorLerp(begin, end, value, easeType);
        //     yield return null;
        // }
    }
}

// 定义缓动曲线类型
public enum EaseType
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut
}