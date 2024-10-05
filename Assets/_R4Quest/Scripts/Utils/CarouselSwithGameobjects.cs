using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarouselSwithGameobjects : MonoBehaviour
{
   [SerializeField] List<GameObject> _goList;
   [SerializeField] private Button _button;
   private int current = 0;
   
   private void Start()
   {
      _button.onClick.AddListener(Click);
      _goList[0].SetActive(true);
   }

   private void Click()
   {
      _goList[current].SetActive(false);
      if (current != _goList.Count -1)
      {
         _goList[current + 1].SetActive(true);
         current++;
      }
      else
      {
         _goList[0].SetActive(true);
         current = 0;
      }
   }
}
