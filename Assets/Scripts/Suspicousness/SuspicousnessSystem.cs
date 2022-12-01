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

    public int threshold = 60;
    public int maxSuspiciousRounds = 3;


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
        if (suspicousness < 0)
        {
            suspicousness = 0;
        }
        UIHandler.Instance.UpdateSuspicion();
    }
}
