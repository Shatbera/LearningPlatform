using UnityEngine;

public class GlobalInstaller : ServiceInstaller
{
    [SerializeField] private CommandSystemRefSO _commandSystemRef;
    [SerializeField] private ItemPickupAnimatorRefSO _itemPickupAnimatorRef;

    [SerializeField] private ItemPickupAnimator _itemPickupAnimator;
    public override void Install()
    {
        _commandSystemRef.InstallService(new CommandSystem());
        _itemPickupAnimatorRef.InstallService(_itemPickupAnimator);
    }
}
