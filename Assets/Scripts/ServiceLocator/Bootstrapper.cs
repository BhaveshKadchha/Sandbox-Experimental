using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ServiceLocator))]
public abstract class Bootstrapper : MonoBehaviour
{
    ServiceLocator container;

    internal ServiceLocator Container => container.OrNull() ?? (container = GetComponent<ServiceLocator>());

    bool hasBeenBootstrapped;

    void Awake() => BootstrapOnDemand();

    public void BootstrapOnDemand()
    {
        if (hasBeenBootstrapped) return;
        hasBeenBootstrapped = true;
        Bootstrap();
    }

    protected abstract void Bootstrap();
}

public class ServiceLocatorGlobalBootstrapper : Bootstrapper
{
    [SerializeField] bool dontDestroyOnLoad = true;

    protected override void Bootstrap() => Container.ConfigureAsGlobal(dontDestroyOnLoad);
}

public class ServiceLocatorSceneBootstrapper : Bootstrapper
{
    protected override void Bootstrap() => Container.ConfigureForScene();
}