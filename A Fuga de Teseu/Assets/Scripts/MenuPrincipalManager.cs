using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]private GameObject painel_menu_inicial;
    [SerializeField]private GameObject painel_creditos;
    [SerializeField]private GameObject painel_opcoes;

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

    public void abrir_opcoes()
    {
        painel_menu_inicial.SetActive(false);
        painel_opcoes.SetActive(true);
    }

    public void fechar_opcoes()
    {
        painel_menu_inicial.SetActive(true);
        painel_opcoes.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Labirinto-01");
    }


}
