using UnityEngine;
using UnityEngine.UI;

public class GameobjectActiveSwitch : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _go;

    private void OnEnable()
    {
        if (_button == null)
            _button = GetComponent<Button>();
        
        _button.onClick.AddListener(Switch);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Switch);
    }

    void Switch()
    {
        _go.SetActive(!_go.activeInHierarchy);
    }
}
