using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class LeaderboardHandler : MonoBehaviour
{
    // Firebase Database reference
    private DatabaseReference playerDbRef;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bouncing-game-aca53.firebaseio.com/");

        // Get the root reference location of the database.
        playerDbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }
}
