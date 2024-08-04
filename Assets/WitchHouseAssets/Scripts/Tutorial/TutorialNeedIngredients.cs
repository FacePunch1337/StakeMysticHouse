using UnityEngine;

public class TutorialNeedIngredients : TutorialNextTrigger
{
    [SerializeField] private Inventory _inventory;
    
    // Doesnt support multiple same ingredients
    [SerializeField] private Ingredient[] _neededIngredients; 
    

    public override void Init()
    {
        _inventory.OnIngredientAdd += CheckIfCompleted;
    }

    private void CheckIfCompleted(Ingredient _)
    {
        CheckIfCompleted();
    }

    public override void CheckIfCompleted()
    {
        for (int i = 0; i < _neededIngredients.Length; i++)
        {
            if(_inventory.HasIngredient(_neededIngredients[i]) == false)
            {
                return;
            }
        }

        SetCompleted();
    }

    public override void Disable()
    {
        _inventory.OnIngredientAdd -= CheckIfCompleted;
    }
}