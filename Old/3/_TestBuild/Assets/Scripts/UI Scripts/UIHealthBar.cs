using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour 
{
    Transform healthBar;

    [SerializeField]
    GameObject damagePopUpPrefab;

    public Transform target;

    void Start()
    {
        healthBar = transform.GetChild(0).GetChild(0);
    }

    void Update()
    {
        UpdatePosition(target.position);
    }

    public void UpdatePosition(Vector3 targetPosition)
    {
        transform.position = Camera.main.WorldToScreenPoint(targetPosition + Vector3.up * 1.2f);
    }

    /// <summary>
    /// Updates the health bar.
    /// </summary>
    /// <param name="percent">Percent of health the mech is currently on, 0.0f - 1.0f range.</param>
    public void UpdateHealth(float percent)
    {
        healthBar.localScale = new Vector3(percent, 1f, 1f);
    }

    public void UpdateHealth(float percent, int damage)
    {
        healthBar.localScale = new Vector3(percent, 1f, 1f);
        GameObject popUp = GameObject.Instantiate(damagePopUpPrefab, transform);
        popUp.GetComponent<UIDamagePopUp>().SetText(damage);
        popUp.transform.position = transform.position;
    }

    public void SetColor(Color color)
    {
        if (healthBar == null)
            healthBar = transform.GetChild(0).GetChild(0);
        healthBar.GetChild(0).GetComponent<Image>().color = color;
    }
}
