using System;
using System.Runtime.CompilerServices;
using ChatApp_Oliverio.Droid;
using Xamarin.Forms;
using Firebase.Auth;
using System.Threading.Tasks;
using Plugin.CloudFirestore;
using DependencyAttribute = Xamarin.Forms.DependencyAttribute;
using Plugin.FacebookClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Plugin.GoogleClient;

[assembly: Dependency(typeof(FirebaseAuthService))]
namespace ChatApp_Oliverio.Droid
{
    public class FirebaseAuthService : iFirebaseAuth
    {
        DataClass dataClass = DataClass.GetInstance;
        public FirebaseAuthResponseModel IsLoggedIn()
        {
            try
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Currently logged in." };
                if (FirebaseAuth.Instance.CurrentUser.Uid == null)
                {
                    response = new FirebaseAuthResponseModel() { Status = false, Response = "Currently logged out." };
                    dataClass.SignedIn = false;
                    dataClass.LoggedInUser = new Account();
                }
                else
                {
                    dataClass.LoggedInUser = new Account()
                    {
                        Uid = FirebaseAuth.Instance.CurrentUser.Uid,
                        Email = FirebaseAuth.Instance.CurrentUser.Email,
                        UserName = dataClass.LoggedInUser.UserName,
                        UserType = dataClass.LoggedInUser.UserType,
                        CreatedAt = dataClass.LoggedInUser.CreatedAt
                    };
                    dataClass.SignedIn = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                dataClass.SignedIn = false;
                dataClass.LoggedInUser = new Account();
                return response;
            }
        }

        public async Task<FirebaseAuthResponseModel> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Login successful." };
                IAuthResult result = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);

                if (result.User.IsEmailVerified && email == result.User.Email)
                {
                    var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("users")
                                        .GetDocument(result.User.Uid)
                                        .GetDocumentAsync();
                    var yourModel = document.ToObject<Account>();

                    dataClass.LoggedInUser = new Account()
                    {
                        Uid = result.User.Uid,
                        Email = result.User.Email,
                        UserName = yourModel.UserName,
                        UserType = yourModel.UserType,
                        CreatedAt = yourModel.CreatedAt,
                        contacts=yourModel.contacts
                    };
                    dataClass.SignedIn = true;
                }
                else
                {
                    FirebaseAuth.Instance.CurrentUser.SendEmailVerification();
                    response.Status = false;
                    response.Response = "Email not verified. Sent another verification email.";
                    dataClass.LoggedInUser = new Account();
                    dataClass.SignedIn = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                dataClass.SignedIn = false;
                return response;
            }
        }

        public async Task<FirebaseAuthResponseModel> ResetPassword(string email)
        {
            try
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Email has been sent to your email address." };
                await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);
                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                return response;
            }
        }

        public FirebaseAuthResponseModel SignOut()
        {
            try
            {
                if (dataClass.LoggedInUser.UserType == 2)
                {
                    CrossFacebookClient.Current.Logout();
                }
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Sign out successful." };
                FirebaseAuth.Instance.SignOut();
                dataClass.SignedIn = false;
                dataClass.LoggedInUser = new Account();
                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                dataClass.SignedIn = true;
                return response;
            }
        }

        public async Task<FirebaseAuthResponseModel> SignUpWithEmailPassword(string name, string email, string password)
        {
            try
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Sign up successful. Verification email sent." };
                await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                FirebaseAuth.Instance.CurrentUser.SendEmailVerification();

                int ndx = email.IndexOf("@");
                int cnt = email.Length - ndx;
                string defaultName = string.IsNullOrEmpty(name) ? email.Remove(ndx, cnt) : name;

                dataClass.LoggedInUser = new Account()
                {
                    Uid = FirebaseAuth.Instance.CurrentUser.Uid,
                    Email = email,
                    UserName = defaultName,
                    UserType = 0,
                    CreatedAt = DateTime.UtcNow
                };
                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                return response;
            }
        }

        public async Task<FirebaseAuthResponseModel> SigninWithFacebook()
        {
            try
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Successfully signed in to Facebook." };
                FacebookResponse<string> data = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "last_name" }, new string[] { "email", "user_birthday" });
                if (data.Status==FacebookActionStatus.Completed)
                {
                    var details = JsonConvert.DeserializeObject<FacebookProfile>(data.Data);
                    response = await LoginWithEmailPassword(details.email, details.first_name+details.last_name);
                    return response;
                }
                else
                {
                    response.Status = false;
                    response.Response = data.Message;
                }
                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                return response;
            }
            
        }

        public async Task<FirebaseAuthResponseModel> LoginAsyncWithFacebook()
        {
            try
            {
                CrossFacebookClient.Current.Logout();
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Successfully signed in to Facebook." };
                FacebookResponse<bool> res = await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "public_profile" });
                if (res.Status != FacebookActionStatus.Completed)
                {
                    response.Response = res.Message;
                    response.Status = false;
                }
                return response;
            }
            catch(Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                return response;
            }
        }

        public async Task<FirebaseAuthResponseModel> SignUpWithFacebook()
        {
            try
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Sign up successful. Verification email sent." };
                FacebookResponse<string> data = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "last_name" }, new string[] { "email", "user_birthday" });
                if (data.Status==FacebookActionStatus.Completed)
                {
                    var details = JsonConvert.DeserializeObject<FacebookProfile>(data.Data);
                    await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(details.email, details.first_name+ details.last_name);
                    FirebaseAuth.Instance.CurrentUser.SendEmailVerification();

                    int ndx = details.email.IndexOf("@");
                    int cnt = details.email.Length - ndx;
                    string defaultName = string.IsNullOrEmpty(details.first_name +" "+ details.last_name) ? details.email.Remove(ndx, cnt) : details.first_name + " " + details.last_name;

                    dataClass.LoggedInUser = new Account()
                    {
                        Uid = FirebaseAuth.Instance.CurrentUser.Uid,
                        Email = details.email,
                        UserName = defaultName,
                        UserType = 2,
                        CreatedAt = DateTime.UtcNow
                    };
                }
                else
                {
                    response.Status = false;
                    response.Response = data.Message;
                }
                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                return response;
            }
        }

        public async Task<FirebaseAuthResponseModel> SigninWithGoogle()
        {
            try
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Successfully signed in to Google." };
                CrossGoogleClient.Current.Logout();
                GoogleResponse<Plugin.GoogleClient.Shared.GoogleUser> data = await CrossGoogleClient.Current.LoginAsync();
                if (data.Status==GoogleActionStatus.Completed)
                {
                    response = await LoginWithEmailPassword(data.Data.Email, data.Data.GivenName + data.Data.FamilyName);
                    return response;
                }
                else
                {
                    response.Status = false;
                    response.Response = data.Message;
                }
                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                return response;
            }
        }

        public async Task<FirebaseAuthResponseModel> SignUpWithGoogle()
        {
            try
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = true, Response = "Sign up successful. Verification email sent." };
                CrossGoogleClient.Current.Logout();
                GoogleResponse<Plugin.GoogleClient.Shared.GoogleUser> data = await CrossGoogleClient.Current.LoginAsync();
                if (data.Status == GoogleActionStatus.Completed)
                {
                    await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(data.Data.Email, data.Data.GivenName+data.Data.FamilyName);
                    FirebaseAuth.Instance.CurrentUser.SendEmailVerification();

                    int ndx = data.Data.Email.IndexOf("@");
                    int cnt = data.Data.Email.Length - ndx;
                    string defaultName = string.IsNullOrEmpty(data.Data.GivenName + " " + data.Data.FamilyName) ? data.Data.Email.Remove(ndx, cnt) : data.Data.GivenName + " " + data.Data.FamilyName;

                    dataClass.LoggedInUser = new Account()
                    {
                        Uid = FirebaseAuth.Instance.CurrentUser.Uid,
                        Email = data.Data.Email,
                        UserName = defaultName,
                        UserType = 1,
                        CreatedAt = DateTime.UtcNow
                    };
                }
                else
                {
                    response.Status = false;
                    response.Response = data.Message;
                }
                return response;
            }
            catch (Exception ex)
            {
                FirebaseAuthResponseModel response = new FirebaseAuthResponseModel() { Status = false, Response = ex.Message };
                return response;
            }
        }

        public ContactModel GenerateID(Account account)
        {
            ContactModel contact = new ContactModel()
            {
                id = IDGenerator.generateID(),
                contactID = new string[] { DataClass.GetInstance.LoggedInUser.Uid, account.Uid },
                contactEmail = new string[] { DataClass.GetInstance.LoggedInUser.Email, account.Email },
                contactName = new string[] { DataClass.GetInstance.LoggedInUser.UserName, account.UserName+" "},
                created_at = DateTime.UtcNow
            };
            return contact;
        }
    }
}