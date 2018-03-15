using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;

namespace CertMS.Helpers
{
    internal static class ProgramCaller
    {
        public static string CallProgramWith(string program, params string[] arguments)
        {
            var client = new HttpClient();
            var values = new Dictionary<string, string>();
            for (var i = 0; i < arguments.Length; i++)
                values.Add(i.ToString(), arguments[i]);
            values.Add("correlationId", Guid.NewGuid().ToString());
            var content = new FormUrlEncodedContent(values);
            try
            {
                var response = client.PostAsync($"http://localhost:8080/start/{program}", content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                return responseString;
            }
            catch (AggregateException)
            {
                MessageBox.Show($"Error communicating with {program}", "CertMS", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return string.Empty;
        }
    }
}