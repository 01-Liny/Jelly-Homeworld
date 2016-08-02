using UnityEngine;
using System.Collections;

public interface  IState 
{
    void OnEnter(string prevState="");//进入状态
    void OnExit(string nextState="");//退出状态
    void OnUpdate();//不断刷新,外部Update函数不断调用
    void OnTrigger();//该状态下的行动，外部调用
}
