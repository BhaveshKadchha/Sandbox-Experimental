using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

public class ServiceLocator : MonoBehaviour
{
    static ServiceLocator global;
    static Dictionary<Scene, ServiceLocator> sceneContainers;
    static List<GameObject> tmpSceneGameObjects;

    readonly ServiceManager services = new ServiceManager();

    const string globalServiceLocatorName = "ServiceLocator [Global]";
    const string sceneServiceLocatorName = "ServiceLocator [Scene]";

    #region CONFIGS
    internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
    {
        if (global == this) Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Already configured as global", this);
        else if (global != null) Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Another ServiceLocator is already configured as global", this);
        else
        {
            global = this;
            if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }

    internal void ConfigureForScene()
    {
        Scene scene = gameObject.scene;

        if (sceneContainers.ContainsKey(scene))
        {
            Debug.LogError("ServiceLocator.ConfigureForScene: Another ServiceLocator is already configured for this scene", this);
            return;
        }

        sceneContainers.Add(scene, this);
    }
    #endregion

    #region ACCESS
    public static ServiceLocator Global
    {
        get 
        {
            if (global != null) return global;

            if (FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found)
            {
                found.BootstrapOnDemand();
                return global;
            }

            var container = new GameObject(globalServiceLocatorName,typeof(ServiceLocator));
            container.AddComponent<ServiceLocator>();

            return global;
        }
        private set { global = value; }
    }

    public static ServiceLocator For(MonoBehaviour mb)
    {
        return mb.GetComponent<ServiceLocator>().OrNull() ?? ForSceneOf(mb) ?? Global;
    }

    public static ServiceLocator ForSceneOf(MonoBehaviour mb)
    {
        Scene scene = mb.gameObject.scene;

        if (sceneContainers.TryGetValue(scene,out ServiceLocator container) && container != mb)
            return container;

        tmpSceneGameObjects.Clear();
        scene.GetRootGameObjects(tmpSceneGameObjects);

        foreach (GameObject go in tmpSceneGameObjects.Where(go => go.GetComponent<ServiceLocatorSceneBootstrapper>() != null))
        {
            if (go.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper) && bootstrapper.Container != mb)
            {
                bootstrapper.BootstrapOnDemand();
                return bootstrapper.Container;
            }
        }

        return Global;
    }
    #endregion

    #region REGISTER
    public ServiceLocator Register<T>(T service)
    {
        services.Register(service);
        return this;
    }

    public ServiceLocator Register(Type type, object service)
    {
        services.Register(type, service);
        return this;
    }
    #endregion

    #region GET
    public ServiceLocator Get<T>(out T service) where T : class
    {
        if (TryGetService(out service)) return this;
        if (TryGetNextInHierarcy(out ServiceLocator container))
        {
            container.Get(out service);
            return this;
        }

        throw new ArgumentException($"ServiceLocator.Get: Service of type {typeof(T).FullName} not registered");
    }

    bool TryGetService<T>(out T service) where T : class
    {
        return services.TryGet(out service);
    }

    bool TryGetNextInHierarcy(out ServiceLocator container)
    {
        if (this == global)
        {
            container = null;
            return false;
        }

        container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(this);
        return container != null;
    }
    #endregion

    #region CLEAN_UP
    void OnDestroy()
    {
        if (this == global)
            global = null;
        else if (sceneContainers.ContainsValue(this))
            sceneContainers.Remove(gameObject.scene);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        global = null;
        sceneContainers = new Dictionary<Scene, ServiceLocator>();
        tmpSceneGameObjects = new List<GameObject>();
    }
    #endregion

#if UNITY_EDITOR
    [MenuItem("GameObject/ServiceLocator/Add Global")]
    static void AddGlobal()
    {
        var go = new GameObject(globalServiceLocatorName, typeof(ServiceLocatorGlobalBootstrapper));
    }

    [MenuItem("GameObject/ServiceLocator/Add Scene")]
    static void AddScene()
    {
        var go = new GameObject(sceneServiceLocatorName, typeof(ServiceLocatorSceneBootstrapper));
    }
#endif
}