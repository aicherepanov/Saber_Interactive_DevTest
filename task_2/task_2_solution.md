**Online service**:

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
----
**Possible test cases**:

- **Call Register**:
1. with a valid email address (not used before and matches regular expression to check for the structure of an email), expect: user id has been received.
2. with an already used email address, expect: Exception (duplicate email).
3. with an invalid email address format (does not matches regular expression to check for the structure of an email), expect: Exception (invalid email format).
4. with an empty email address, expect: Exception (empty email).

- **Call RequestActivationCode**:
5. with valid guestUserId, expect: Activation code sent successfully.
6. with non-existing guestUserId, expect: Exception (user id not found).
7. with guestUserId already registered user, expect: Exception (user already registered).

- **Call ConfirmEmail**:
8. with correct activationCode and password, expect: User promoted from guest to registered. 
9. with incorrect activationCode, expect: Exception (invalid activation code).
10. with correct activationCode but wrong password, expect: Exception (incorrect password).
11. with guestUserId already registered user, expect: Exception (user already registered). 
12. with non-existing guestUserId, expect: Exception (user id not found).

- **Call SignIn**:
13. with correct userId and password, expect: Authorization token received. 
14. with incorrect userId, expect: Exception (user id not found).
15. with correct userId but wrong password, expect: Exception (incorrect password).
16. with a guest userId, expect: Exception (guest users cannot sign in).
17. with a registered userId that is already signed in, expect: Exception (user already signed in).
18. with a registered userId after signing out, expect: Authorization token received.

- **Call UpdateUser**:
19. with correct authToken, new email, and new password, expect: User data updated successfully.
20. with incorrect authToken, expect: Exception (unauthorized).
21. with an expired authToken, expect: Exception (token expired).
22. with correct authToken, empty email, and new password, expect: Exception (empty email).
23. with correct authToken, new email, and empty password, expect: Exception (empty password).
24. with correct authToken and new password, expect: User data updated successfully.

- **Call UpdateToken**:
25. with a valid authToken, expect: New prolonged authorization token received.
26. with incorrect authToken, expect: Exception (unauthorized).
27. with an expired authToken, expect: Exception (token expired).

- **Call SignOut**:
Must accept an authToken as an input argument, without an argument, do not match the userId to perform SignOut