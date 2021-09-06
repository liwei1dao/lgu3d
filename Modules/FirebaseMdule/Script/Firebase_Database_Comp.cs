using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace lgu3d
{
  /// <summary>
  /// Firebase Database 数据库组件
  /// </summary>
  public class Firebase_Database_Comp : ModelCompBase<FirebaseMdule>
  {
    ArrayList leaderBoard = new ArrayList();
    private string email = "";
    private int score;
    private const int MaxScores = 5;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;
    public override void Load(ModelBase _ModelContorl, params object[] agr)
    {
      base.Load(_ModelContorl, agr);
      leaderBoard.Clear();
      leaderBoard.Add("Firebase Top " + MaxScores.ToString() + " Scores");

      FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
      {
        dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
          InitializeFirebase();
        }
        else
        {
          Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
      });
    }
    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
      FirebaseApp app = FirebaseApp.DefaultInstance;
      StartListener();
      isFirebaseInitialized = true;
    }


    protected void StartListener()
    {
      FirebaseDatabase.DefaultInstance
        .GetReference("Leaders").OrderByChild("score")
        .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
        {
          if (e2.DatabaseError != null)
          {
            Debug.LogError(e2.DatabaseError.Message);
            return;
          }
          Debug.Log("Received values for Leaders.");
          string title = leaderBoard[0].ToString();
          leaderBoard.Clear();
          leaderBoard.Add(title);
          if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
          {
            foreach (var childSnapshot in e2.Snapshot.Children)
            {
              if (childSnapshot.Child("score") == null
                  || childSnapshot.Child("score").Value == null)
              {
                Debug.LogError("Bad data in sample.");
                break;
              }
              else
              {
                Debug.Log("Leaders entry : " +
                childSnapshot.Child("email").Value.ToString() + " - " +
                childSnapshot.Child("score").Value.ToString());
                leaderBoard.Insert(1, childSnapshot.Child("score").Value.ToString()
                + "  " + childSnapshot.Child("email").Value.ToString());
              }
            }
          }
        };
    }

    // A realtime database transaction receives MutableData which can be modified
    // and returns a TransactionResult which is either TransactionResult.Success(data) with
    // modified data or TransactionResult.Abort() which stops the transaction with no changes.
    TransactionResult AddScoreTransaction(MutableData mutableData)
    {
      List<object> leaders = mutableData.Value as List<object>;

      if (leaders == null)
      {
        leaders = new List<object>();
      }
      else if (mutableData.ChildrenCount >= MaxScores)
      {
        // If the current list of scores is greater or equal to our maximum allowed number,
        // we see if the new score should be added and remove the lowest existing score.
        long minScore = long.MaxValue;
        object minVal = null;
        foreach (var child in leaders)
        {
          if (!(child is Dictionary<string, object>))
            continue;
          long childScore = (long)((Dictionary<string, object>)child)["score"];
          if (childScore < minScore)
          {
            minScore = childScore;
            minVal = child;
          }
        }
        // If the new score is lower than the current minimum, we abort.
        if (minScore > score)
        {
          return TransactionResult.Abort();
        }
        // Otherwise, we remove the current lowest to be replaced with the new score.
        leaders.Remove(minVal);
      }

      // Now we add the new score as a new entry that contains the email address and score.
      Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
      newScoreMap["score"] = score;
      newScoreMap["email"] = email;
      leaders.Add(newScoreMap);

      // You must set the Value to indicate data at that location has changed.
      mutableData.Value = leaders;
      return TransactionResult.Success(mutableData);
    }
    
    public void AddScore()
    {
      if (score == 0 || string.IsNullOrEmpty(email))
      {
        Debug.Log("invalid score or email.");
        return;
      }
      Debug.Log(String.Format("Attempting to add score {0} {1}",
        email, score.ToString()));

      DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Leaders");

      Debug.Log("Running Transaction...");
      // Use a transaction to ensure that we do not encounter issues with
      // simultaneous updates that otherwise might create more than MaxScores top scores.
      reference.RunTransaction(AddScoreTransaction)
        .ContinueWithOnMainThread(task =>
        {
          if (task.Exception != null)
          {
            Debug.Log(task.Exception.ToString());
          }
          else if (task.IsCompleted)
          {
            Debug.Log("Transaction complete.");
          }
        });
    }

  }
}