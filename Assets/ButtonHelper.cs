using Catavaneer;
using UnityEngine;
using UnityEngine.UI;
public class ButtonHelper : MonoBehaviour
{

    public Button button;
    public string FunctionToCallFromGMAN;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Button>())
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnUseButton);
        }
        else
        {
            Debug.LogWarning("No button attached to " + name);
        }
    }

    public  void OnUseButton()
    {
        FindObjectOfType<GameManager>().Invoke(FunctionToCallFromGMAN,0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
