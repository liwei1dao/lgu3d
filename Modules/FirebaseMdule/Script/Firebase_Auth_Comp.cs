using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// Firebase Auth 身份认证组件
  /// </summary>
  public class Firebase_Auth_Comp : ModelCompBase<FirebaseMdule>
  {
    private string displayName;
    private bool fetchingToken = false;
    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseAuth otherAuth;
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    private Firebase.AppOptions otherAuthOptions = new Firebase.AppOptions
    {
      ApiKey = "",
      AppId = "",
      ProjectId = ""
    };
    public override void Load(ModelBase _ModelContorl, params object[] agr)
    {
       base.Load(_ModelContorl, agr);
      Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
      {
        dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available)
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

    // Handle initialization of the necessary firebase modules:
    protected void InitializeFirebase()
    {
      Debug.Log("Setting up Firebase Auth");
      auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
      auth.StateChanged += AuthStateChanged;
      auth.IdTokenChanged += IdTokenChanged;
      // Specify valid options to construct a secondary authentication object.
      if (otherAuthOptions != null &&
          !(String.IsNullOrEmpty(otherAuthOptions.ApiKey) ||
            String.IsNullOrEmpty(otherAuthOptions.AppId) ||
            String.IsNullOrEmpty(otherAuthOptions.ProjectId)))
      {
        try
        {
          otherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(
            otherAuthOptions, "Secondary"));
          otherAuth.StateChanged += AuthStateChanged;
          otherAuth.IdTokenChanged += IdTokenChanged;
        }
        catch (Exception)
        {
          Debug.Log("ERROR: Failed to initialize secondary authentication object.");
        }
      }
      AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
      Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
      Firebase.Auth.FirebaseUser user = null;
      if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
      if (senderAuth == auth && senderAuth.CurrentUser != user)
      {
        bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
        if (!signedIn && user != null)
        {
          Debug.Log("Signed out " + user.UserId);
        }
        user = senderAuth.CurrentUser;
        userByAuth[senderAuth.App.Name] = user;
        if (signedIn)
        {
          Debug.Log("AuthStateChanged Signed in " + user.UserId);
          displayName = user.DisplayName ?? "";
          DisplayDetailedUserInfo(user, 1);
        }
      }
    }
    // Track ID token changes.
    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
      Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
      if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
      {
        senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
          task => Debug.Log(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
      }
    }
    // Display a more detailed view of a FirebaseUser.
    protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
    {
      string indent = new String(' ', indentLevel * 2);
      DisplayUserInfo(user, indentLevel);
      Debug.Log(String.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
      Debug.Log(String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
      Debug.Log(String.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
      var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
      var numberOfProviders = providerDataList.Count;
      if (numberOfProviders > 0)
      {
        for (int i = 0; i < numberOfProviders; ++i)
        {
          Debug.Log(String.Format("{0}Provider Data: {1}", indent, i));
          DisplayUserInfo(providerDataList[i], indentLevel + 2);
        }
      }
    }
    // Display user information.
    protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
    {
      string indent = new String(' ', indentLevel * 2);
      var userProperties = new Dictionary<string, string> {
        {"Display Name", userInfo.DisplayName},
        {"Email", userInfo.Email},
        {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
        {"Provider ID", userInfo.ProviderId},
        {"User ID", userInfo.UserId}
      };
      foreach (var property in userProperties)
      {
        if (!String.IsNullOrEmpty(property.Value))
        {
          Debug.Log(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
        }
      }
    }
    protected void SignOut()
    {
      Debug.Log("Signing out.");
      auth.SignOut();
    }

    // Determines whether another authentication object is available to focus.
    protected bool HasOtherAuth { get { return auth != otherAuth && otherAuth != null; } }

    // Swap the authentication object currently being controlled by the application.
    protected void SwapAuthFocus()
    {
      if (!HasOtherAuth) return;
      var swapAuth = otherAuth;
      otherAuth = auth;
      auth = swapAuth;
      Debug.Log(String.Format("Changed auth from {0} to {1}",
                              otherAuth.App.Name, auth.App.Name));
    }

    void GUIDisplayGameCenterControls()
    {
      bool isOnIosDevice = Application.platform == RuntimePlatform.IPhonePlayer;
      bool isOnOSXDesktop = (Application.platform == RuntimePlatform.OSXEditor ||
                              Application.platform == RuntimePlatform.OSXPlayer);

      if (isOnIosDevice || isOnOSXDesktop)
      {
        if (GUILayout.Button(new GUIContent("Authenticate To Game Center")))
        {
          AuthenticateToGameCenter();
        }

        bool gameCenterEnabled = (isOnIosDevice ?
                                    Firebase.Auth.GameCenterAuthProvider.IsPlayerAuthenticated() :
                                    false);
        using (new ScopedGuiEnabledModifier(gameCenterEnabled))
        {
          string tooltip = "";
          if (!gameCenterEnabled)
          {
            tooltip = "No Game Center player authenticated.";
          }
          if (GUILayout.Button(new GUIContent("Sign In With Game Center", tooltip)))
          {
            SignInWithGameCenterAsync();
          }
        }
      }
    }
    public void AuthenticateToGameCenter()
    {
#if UNITY_IOS
        Social.localUser.Authenticate(success => {
          Debug.Log("Game Center Initialization Complete - Result: " + success);
        });
#else
      Debug.Log("Game Center is not supported on this platform.");
#endif
    }
    public Task SignInWithGameCenterAsync()
    {
      var credentialTask = Firebase.Auth.GameCenterAuthProvider.GetCredentialAsync();
      var continueTask = credentialTask.ContinueWithOnMainThread(task =>
      {
        if (!task.IsCompleted)
          return null;

        if (task.Exception != null)
          Debug.Log("GC Credential Task - Exception: " + task.Exception.Message);

        var credential = task.Result;

        var loginTask = auth.SignInWithCredentialAsync(credential);
        return loginTask.ContinueWithOnMainThread(HandleSignInWithUser);
      });

      return continueTask;
    }
    // Called when a sign-in without fetching profile data completes.
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
    {
      // EnableUI();
      if (LogTaskCompletion(task, "Sign-in"))
      {
        Debug.Log(String.Format("{0} signed in", task.Result.DisplayName));
      }
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation)
    {
      bool complete = false;
      if (task.IsCanceled)
      {
        Debug.Log(operation + " canceled.");
      }
      else if (task.IsFaulted)
      {
        Debug.Log(operation + " encounted an error.");
        foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
        {
          string authErrorCode = "";
          Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
          if (firebaseEx != null)
          {
            authErrorCode = String.Format("AuthError.{0}: ",
              ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
          }
          Debug.Log(authErrorCode + exception.ToString());
        }
      }
      else if (task.IsCompleted)
      {
        Debug.Log(operation + " completed");
        complete = true;
      }
      return complete;
    }

    private class ScopedGuiEnabledModifier : IDisposable
    {
      private bool wasEnabled;
      public ScopedGuiEnabledModifier(bool newValue)
      {
        wasEnabled = GUI.enabled;
        GUI.enabled = newValue;
      }

      public void Dispose()
      {
        GUI.enabled = wasEnabled;
      }
    }
  }

}