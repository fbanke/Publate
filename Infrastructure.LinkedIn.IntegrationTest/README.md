To run the integration tests you need to create a user secret store with the values referenced in LinkedInSettings

> :warning: **LinkedIn doesn't have a sandbox environment**: The integration tests will run on production with the user you auth with


You create the user secret store using 

```dotnet user-secrets init```

You set the secrets using:

```dotnet user-secrets set "LinkedIn:ClientId" "xxx"```
