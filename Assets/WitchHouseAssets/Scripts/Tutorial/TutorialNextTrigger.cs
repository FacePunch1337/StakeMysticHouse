using UnityEngine;

public abstract class TutorialNextTrigger : MonoBehaviour
{
    [SerializeField] private TutorialManager _tutorialManager;
    [SerializeField] private int _tutorialStepIndex = 0;

    protected bool _completed;

    private void OnValidate()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
        if(_tutorialManager == null)
            Debug.LogError("No TutorianManager on scene");
    }

    protected void TriggerMoveToNext()
    {
        _tutorialManager.TryMoveNext(_tutorialStepIndex);
    }

    public abstract void Init();

    public abstract void CheckIfCompleted();

    public abstract void Disable();

    protected void SetCompleted()
    {
        if (_completed)
            return;
        
        _tutorialManager.TryMoveNext(_tutorialStepIndex);
        
        Disable();
        _completed = true;
    }
}
