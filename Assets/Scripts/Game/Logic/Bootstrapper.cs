using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    private void Awake()
    {

        _container.Resolve<InputManager>().Init();

        Cursor.visible = false; //remove later
    }
}
