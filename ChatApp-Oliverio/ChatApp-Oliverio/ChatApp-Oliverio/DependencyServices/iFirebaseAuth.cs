using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp_Oliverio
{
    public interface iFirebaseAuth
    {
        Task<FirebaseAuthResponseModel> LoginWithEmailPassword(string email, string password);
        Task<FirebaseAuthResponseModel> SignUpWithEmailPassword(string name, string email, string password);
        FirebaseAuthResponseModel SignOut();
        FirebaseAuthResponseModel IsLoggedIn();
        Task<FirebaseAuthResponseModel> ResetPassword(string email);
        Task<FirebaseAuthResponseModel> SigninWithFacebook();
        Task<FirebaseAuthResponseModel> LoginAsyncWithFacebook();
        Task<FirebaseAuthResponseModel> SignUpWithFacebook();
        Task<FirebaseAuthResponseModel> SigninWithGoogle();
        Task<FirebaseAuthResponseModel> SignUpWithGoogle();
        ContactModel GenerateID(Account account);
    }
}
