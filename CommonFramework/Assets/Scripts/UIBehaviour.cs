using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ****************************************************************
// 功能：向UIManager 注册 GameObject. 挂载控件的事件放在继承UIBase这样的脚本上,不用去查找
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class UIBehaviour : MonoBehaviour {

    public void AddButtonListener(UnityAction action) {
        if (action != null) {
            GetComponent<Button>().onClick.AddListener(action);
        }
    }

    public void RemoveButtonListener(UnityAction action) {
        if (action != null) {
            GetComponent<Button>().onClick.RemoveListener(action);
        }
    }

    public void AddToggleListener(UnityAction<bool> action) {
        if (action != null) {
            GetComponent<Toggle>().onValueChanged.AddListener(action);
        }
    }

    public void RemoveToggleListener(UnityAction<bool> action) {
        if (action != null) {
            GetComponent<Toggle>().onValueChanged.RemoveListener(action);
        }
    }

    public void AddSliderListener(UnityAction<float> action) {
        if (action != null) {
            GetComponent<Slider>().onValueChanged.AddListener(action);
        }
    }

    public void RemoveSliderListener(UnityAction<float> action) {
        if (action != null) {
            GetComponent<Slider>().onValueChanged.RemoveListener(action);
        }
    }

    public void AddInputFieldListener(UnityAction<string> action) {
        if (action != null) {
            GetComponent<InputField>().onValueChanged.AddListener(action);
        }
    }

    public void RemoveInputFieldListener(UnityAction<string> action) {
        if (action != null) {
            GetComponent<InputField>().onValueChanged.RemoveListener(action);
        }
    }

    private void Awake() {
        UIManager.Instance.RegistGameObject(name, gameObject);
    } 
}

