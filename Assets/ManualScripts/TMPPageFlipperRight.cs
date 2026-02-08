using TMPro;
using UnityEngine;

public class TMPPageFlipperRight : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int currentPage = 0;

    public void NextPage()
    {
        if (currentPage < text.textInfo.pageCount - 1)
        {
            currentPage++;
            text.pageToDisplay = currentPage + 1;
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            text.pageToDisplay = currentPage + 1;
        }
    }
}
