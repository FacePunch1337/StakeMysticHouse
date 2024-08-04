using UnityEngine;

public class TutorialWaitForOrderComplete : TutorialNextTrigger
{
    [SerializeField] private OrderManager _orderManager; 
    

    public override void Init()
    {
        _orderManager.OnOrderComplete += CheckIfCompleted;
    }

    public override void CheckIfCompleted()
    {
        SetCompleted();
    }

    public override void Disable()
    {
        _orderManager.OnOrderComplete -= CheckIfCompleted;
    }
}