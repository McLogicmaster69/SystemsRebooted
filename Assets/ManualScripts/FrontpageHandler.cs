using TMPro;
using UnityEngine;

public class FrontpageHandler : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int currentPage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void NextPage()
    {
        if (currentPage < text.textInfo.pageCount - 1)
        {
            currentPage++;
            text.pageToDisplay = currentPage + 1;
        }
        if (currentPage == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            text.pageToDisplay = currentPage + 1;
        }
        if (currentPage == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
