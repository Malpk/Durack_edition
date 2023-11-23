using System;
using System.Collections.Generic;
using UnityEngine;

//Для запуска анимаций и передвижений, обработчиков в отедльном потоке, синхронизация через lock
public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;
    private Queue<Action> actions = new Queue<Action>(); //Очередь задач

    public static bool IsRun => instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    //Выполняет конкретное действие в своём потоке. Поток один, остальные ждут (см. оператор lock)
    private void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
            {
                actions.Dequeue()?.Invoke();
            }
        }
    }

    //Запуск в главном потоке. Остальные ждут.
    public static void RunOnMainThread(Action action)
    {
        lock (instance.actions)
        {
            instance.actions.Enqueue(action);
        }
    }
}
