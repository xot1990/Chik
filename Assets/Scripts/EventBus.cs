using System;
using UnityEngine;

public static class EventBus
{
    // ������� ��� ���������� ��������
    public static event Action<GameObject, Vector3> OnPlantPlaced;
    public static void RaiseOnPlantPlaced(GameObject plant, Vector3 position)
    {
        OnPlantPlaced?.Invoke(plant, position);
    }

    // ������� ��� ������� ��������
    public static event Action<GameObject> OnPlantSold;
    public static void RaiseOnPlantSold(GameObject plant)
    {
        OnPlantSold?.Invoke(plant);
    }

     // ������� ��� ��������� ����� ���������
    public static event Action<GameObject, float> OnPlantDamaged;
     public static void RaiseOnPlantDamaged(GameObject plant, float damage)
    {
          OnPlantDamaged?.Invoke(plant,damage);
    }

    // ������� ��� ��������� ����� �����
      public static event Action<GameObject, float> OnZombieDamaged;
     public static void RaiseOnZombieDamaged(GameObject zombie, float damage)
    {
          OnZombieDamaged?.Invoke(zombie,damage);
    }
    // ������� ��� ������ �����
      public static event Action<GameObject> OnZombieDied;
     public static void RaiseOnZombieDied(GameObject zombie)
    {
          OnZombieDied?.Invoke(zombie);
    }

    // ������� ��� ���������� ������
    public static event Action<bool> OnLevelEnd;
    public static void RaiseOnLevelEnd(bool win)
    {
        OnLevelEnd?.Invoke(win);
    }

    // ������� ��� ��������� ���������� ������
    public static event Action<float> OnSunChange;
    public static void RaiseOnSunChange(float sun)
    {
        OnSunChange?.Invoke(sun);
    }
    // ������� ��� ��������� ���������� ���������
    public static event Action<float> OnReactiveChange;
      public static void RaiseOnReactiveChange(float reactive)
    {
        OnReactiveChange?.Invoke(reactive);
    }

     // ������� ��� ������ ����� �����
    public static event Action OnWaveStart;
      public static void RaiseOnWaveStart()
    {
          OnWaveStart?.Invoke();
    }
     // ������� ��� ���������� �����
    public static event Action OnWaveEnd;
      public static void RaiseOnWaveEnd()
    {
          OnWaveEnd?.Invoke();
    }

     // ������� ��� ���������� ����
    public static event Action OnGameOver;
      public static void RaiseOnGameOver()
    {
          OnGameOver?.Invoke();
    }

     // ������� ��� ������ ����
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

     // ������� ��� ����������� ������
    public static event Action OnLevelRestart;
     public static void RaiseOnLevelRestart()
    {
           OnLevelRestart?.Invoke();
     }
}