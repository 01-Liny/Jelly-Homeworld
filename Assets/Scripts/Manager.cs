using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

    MonsterPathFinding monsterPathFinding=new MonsterPathFinding();
	// Use this for initialization
	void Awake () 
    {
        int[,] map = new int[7, 7]{{1, 1, 1, 1, 1, 1, 1},
                                    {1, 0, 1, 0, 0, 0, 1},
                                    {1, 0, 1, 0, 1, 0, 1},
                                    {1, 0, 0, 0, 0, 0, 1},
                                    {1, 0, 1, 1, 1, 0, 1},
                                    {1, 0, 0, 0, 1, 0, 1},
                                    {1, 1, 1, 1, 1, 1, 1}};
        monsterPathFinding.monsterPathFinding(map, 5, 3, 5, 5);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
