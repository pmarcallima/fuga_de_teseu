using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]private GameObject painel_menu_inicial;
    [SerializeField]private GameObject painel_creditos;

    public void abrir_creditos()
    {
        painel_menu_inicial.SetActive(false);
        painel_creditos.SetActive(true);
    }

    public void fechar_creditos()
    {
        painel_menu_inicial.SetActive(true);
        painel_creditos.SetActive(false);
    }

}
