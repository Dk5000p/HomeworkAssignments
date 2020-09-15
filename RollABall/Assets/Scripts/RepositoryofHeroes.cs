using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RepositoryofHeroes : MonoBehaviour
{
    public List<Hero> listOfHeroes;
    // Start is called before the first frame update
    void Start()
    {
        RepositoryofHeroes repHeroes = new RepositoryofHeroes();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            listOfHeroes.RemoveAt(listOfHeroes.Count - 1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Brick>() != null)
        {
            listOfHeroes.Add(other.GetComponent<Brick>());
            Debug.Log(listOfHeroes);
        }
        if (other.GetComponent<Speedster>() != null)
        {
            listOfHeroes.Add(other.GetComponent<Speedster>());
            Debug.Log(listOfHeroes);
        }
    }
    public RepositoryofHeroes()
    {
        listOfHeroes = new List<Hero>();
    }
    
    }


