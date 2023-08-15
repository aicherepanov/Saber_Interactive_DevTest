**Software Development Engineer in Test task**

Let's suppose that we have some online service with few functions:

```
UserService:
string Register(string email); //returns the guest user id.
void RequestActivationCode(string guestUserId); //will sent the confirmation code to email address that has been used on registration.
void ConfirmEmail(string guestUserId, string password, string activationCode); //promotes user from guest to registered.
string SignIn(string userId, string password); //starts user session and returns an authorization token which is lasts 15 minutes.
void UpdateUser(string authToken, string email, string password); //update user data.
string UpdateToken(string authToken); //returns prolonged authorization token.
void SignOut(); //terminates user session.

```
**Note**: 
1. All data is sent or received via http requests;
2. As a simplified example, let's exclude things like request headers and other information not visibly included in this sample;
3. Failed operations will result an Exception.
----
Please provide a text solution (no need to write code) with all possible test cases in the following format:

- **Call Register**:
1. with valid (specify the "valid" definition) email address, expect: user id has been received;
2. etc.
3. ...

- **Call RequestActivationCode**:
4. ...