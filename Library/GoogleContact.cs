using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;

namespace PCS_JIM_Web.Library
{
    public class GoogleContact
    {
        private static string[] Scopes = { PeopleServiceService.Scope.Contacts };
        private const string ApplicationName = "internal-front-desk";

        public static async Task AddNewContact(Person contactperson)
        {
            UserCredential credential;

            // Load client secrets from credentials.json

            string getpath = Directory.GetCurrentDirectory();

            using (var stream = new FileStream("D:\\Aplikasi Project\\PCS JIM Web\\credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None);
            }

            // Create the PeopleServiceService
            var service = new PeopleServiceService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Create a new contact
            var newContact = contactperson;

            try
            {
                var request = service.People.CreateContact(newContact);
                var response = await request.ExecuteAsync();

                Console.WriteLine($"Contact created with resource name: {response.ResourceName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating contact: {ex.Message}");
            }
        }

    }
}