To run the integration tests you need to create a user secret store with the values referenced in Settings

You create the user secret store using 

```dotnet user-secrets init```

You set the secrets using:

```dotnet user-secrets set "Auth0:ClientId" "xxx"```

The Auth0 user must have the permissions ```read:user_idp_tokens``` and ```read:users```