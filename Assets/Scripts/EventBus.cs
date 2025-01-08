using System;
using UnityEngine;

public static class EventBus
{
    // Событие для размещения растения
    public static event Action<GameObject, Vector3> OnPlantPlaced;
    public static void RaiseOnPlantPlaced(GameObject plant, Vector3 position)
    {
        OnPlantPlaced?.Invoke(plant, position);
    }

    // Событие для продажи растения
    public static event Action<GameObject> OnPlantSold;
    public static void RaiseOnPlantSold(GameObject plant)
    {
        OnPlantSold?.Invoke(plant);
    }

     // Событие для получения урона растением
    public static event Action<GameObject, float> OnPlantDamaged;
     public static void RaiseOnPlantDamaged(GameObject plant, float damage)
    {
          OnPlantDamaged?.Invoke(plant,damage);
    }

    // Событие для получения урона зомби
      public static event Action<GameObject, float> OnZombieDamaged;
     public static void RaiseOnZombieDamaged(GameObject zombie, float damage)
    {
          OnZombieDamaged?.Invoke(zombie,damage);
    }
    // Событие для смерти зомби
      public static event Action<GameObject> OnZombieDied;
     public static void RaiseOnZombieDied(GameObject zombie)
    {
          OnZombieDied?.Invoke(zombie);
    }

    // Событие для завершения уровня
    public static event Action<bool> OnLevelEnd;
    public static void RaiseOnLevelEnd(bool win)
    {
        OnLevelEnd?.Invoke(win);
    }

    // Событие для изменения количества солнца
    public static event Action<float> OnSunChange;
    public static void RaiseOnSunChange(float sun)
    {
        OnSunChange?.Invoke(sun);
    }
    // Событие для изменения количества реактивов
    public static event Action<float> OnReactiveChange;
      public static void RaiseOnReactiveChange(float reactive)
    {
        OnReactiveChange?.Invoke(reactive);
    }

     // Событие для старта новой волны
    public static event Action OnWaveStart;
      public static void RaiseOnWaveStart()
    {
          OnWaveStart?.Invoke();
    }
     // Событие для завершения волны
    public static event Action OnWaveEnd;
      public static void RaiseOnWaveEnd()
    {
          OnWaveEnd?.Invoke();
    }

     // Событие для завершения игры
    public static event Action OnGameOver;
      public static void RaiseOnGameOver()
    {
          OnGameOver?.Invoke();
    }

     // Событие для старта игры
    public static event Action OnGameStart;
      public static void RaiseOnGameStart()
    {
          OnGameStart?.Invoke();
    }
      
    public static event Action OnGameExit;
    public static void RaiseOnGameExit()
    {
        OnGameExit?.Invoke();
    }

     // Событие для перезапуска уровня
    public static event Action OnLevelRestart;
     public static void RaiseOnLevelRestart()
    {
           OnLevelRestart?.Invoke();
     }
}