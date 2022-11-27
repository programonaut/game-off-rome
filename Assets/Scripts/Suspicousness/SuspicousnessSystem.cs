using UnityEngine;

public enum SuspicousnessAmountType {
    Low,
    Medium,
    High,
    Super
}

public class SuspicousnessSystem : MonoBehaviour
{
    public static SuspicousnessSystem Instance { get; private set; }

    [SerializeField, Range(0,100)] private int suspicousness = 0;
    public int Suspicousness { get => suspicousness; }

    private int suspiciosnessFrame = 30;
    public int[] suspiciosnessValues = {5,10,15,25};


    private void Awake()
    {
        Instance = this;
    }

    public void IncreaseSuspicousness(int amount)
    {
        suspicousness += amount;
        if (suspicousness > 100)
        {
            suspicousness = 100;
        }
    }

    public void DecreaseSuspicousness(int amount)
    {
        suspicousness -= amount;
        if (suspicousness < 0)
        {
            suspicousness = 0;
        }
    }

    public bool Caught() 
    {
        int catchPercentage = GetCatchPercentage();
        int random = Random.Range(0, 100 + 1);

        return catchPercentage == 0 ? false : random <= catchPercentage;
    }

    public int GetCatchPercentage() 
    {
        int min = suspicousness - suspiciosnessFrame > 0 ? suspicousness - suspiciosnessFrame : 0;
        int max = suspicousness;

        if (suspicousness == 100) min = 100;

        return Random.Range(min, max + 1);
    }
}
