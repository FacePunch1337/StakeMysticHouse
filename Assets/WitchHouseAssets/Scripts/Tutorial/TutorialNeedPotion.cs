using UnityEngine;

public class TutorialNeedPotion: TutorialNextTrigger
{
    [SerializeField] private Inventory _inventory;
    
    // Doesnt support multiple same ingredients
    [SerializeField] private Potion[] _neededPotions; 
    

    public override void Init()
    {
        _inventory.OnPotionAdd += CheckIfCompleted;
    }

    private void CheckIfCompleted(Potion _)
    {
        CheckIfCompleted();
    }

    public override void CheckIfCompleted()
    {
        for (int i = 0; i < _neededPotions.Length; i++)
        {
            if(_inventory.HasPotion(_neededPotions[i]) == false)
            {
                return;
            }
        }

        SetCompleted();
    }

    public override void Disable()
    {
        _inventory.OnPotionAdd -= CheckIfCompleted;
    }
}