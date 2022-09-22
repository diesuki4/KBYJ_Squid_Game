using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Text Component 상속 및 재정의 */

public class XRText : Text
{
    // 1 크기가 변경되었을 때 호출되는 함수를 가지고 있는 변수
    public Action onChangeSize;
    
    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputVertical();
        if (onChangeSize != null)
        {
            onChangeSize();
        }
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        if (onChangeSize != null)
        {
            onChangeSize();
        }
    }
}
