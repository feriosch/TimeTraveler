using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{

    public IUsable MyUsable { get; set; }

    public Button MyButton { get; private set; }
    public Image MyIcon { get => icon; set => icon = value; }

    // Start is called before the first frame update

    [SerializeField]
    private Image icon;

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (MyUsable != null)
        {
            MyUsable.Use();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
