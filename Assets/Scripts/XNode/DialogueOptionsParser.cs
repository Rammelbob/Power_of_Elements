using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueOptionsParser : MonoBehaviour
{

    public GameObject button;

    private int selectedOptionIdex;
    Button lastSelected;
    NodeParser nodeParser;

    [Header("Stats")]
    public Transform scrollviewParent;
    public GameObject statPrefab;
    public ScrollRect statScrollRect;


    private List<Button> buttons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        selectedOptionIdex = 0;
    }


    public void ShowOptionButtons(string[] options, float spaceing, NodeParser nodeParser)
    {

        this.nodeParser = nodeParser;

        for (int i = 0; i < options.Length; i++)
        {
            var newButton = CreateOptionButton();
            int x = i;
            newButton.onClick.AddListener(() => OnButtonClick(x));
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
            buttons.Add(newButton);
        }

        buttons.ElementAt(0).Select();

    }

    public void RemoveOptions()
    {
        buttons.ForEach(b => b.GetComponent<OptionsButtonCustomisation>().OnUIStatSelect -= SetStatScrollViewToSelectedChild);
        buttons.ForEach(b => b.enabled = false);
        buttons.ForEach(b => b.onClick.RemoveAllListeners());
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons.ElementAt(i).gameObject);
        }

        buttons = new List<Button>();
    }

    private void OnButtonClick(int index)
    {
        RemoveOptions();
        nodeParser.NextNodeFromOption(index);
    }

    private Button CreateOptionButton()
    {
        var tempGameObject = Instantiate(button, scrollviewParent);
        var optionBtn = tempGameObject.GetComponent<OptionsButtonCustomisation>();
        if (optionBtn != null)
        {
            optionBtn.OnUIStatSelect += SetStatScrollViewToSelectedChild;
        }

        return tempGameObject.GetComponent<Button>();
    }

    public void SetStatScrollViewToSelectedChild(RectTransform selectedChild)
    {
        RectTransform contentPanel = scrollviewParent.GetComponent<RectTransform>();

        Vector2 viewportLocalPosition = statScrollRect.viewport.localPosition;
        Vector2 childLocalPosition = selectedChild.localPosition;
        Vector2 result = new Vector2(
            0,
            //0 - (viewportLocalPosition.x + childLocalPosition.x),
            0 - (viewportLocalPosition.y + childLocalPosition.y));

        contentPanel.localPosition = result;
    }

}
