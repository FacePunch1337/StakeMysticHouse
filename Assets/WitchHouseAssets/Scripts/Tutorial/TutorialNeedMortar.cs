using UnityEngine;

public class TutorialNeedMortar: TutorialNextTrigger
{
    [SerializeField] private Inventory _inventory;
    
    // Doesnt support multiple same ingredients
    [SerializeField] private Mortar[] _neededMortar; 
    

    public override void Init()
    {
        _inventory.OnMortarAdd += CheckIfCompleted;
    }

    private void CheckIfCompleted(Mortar _)
    {
        CheckIfCompleted();
    }

    public override void CheckIfCompleted()
    {
        for (int i = 0; i < _neededMortar.Length; i++)
        {
            if(_inventory.HasMortar(_neededMortar[i]) == false)
            {
                return;
            }
        }

        SetCompleted();
    }

    public override void Disable()
    {
        _inventory.OnMortarAdd -= CheckIfCompleted;
    }
}