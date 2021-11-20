//using System;
//using System.IO;
//using System.Security.Cryptography.X509Certificates;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Serilog;


//namespace IdentityProvider
//{
//    public class CertificationManager
//    {
//        public static async Task<(X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate)> GetCertificates(IWebHostEnvironment environment, IConfiguration configuration)
//        {
             
//            Log.Information($"USING {configuration["CertificateNameKeyVault"]}");
//            Log.Information($"USING {configuration["AzureKeyVaultEndpoint"]}");

//            var certificateConfiguration = new CertificateConfiguration
//            {
//                // Use an Azure key vault
//                CertificateNameKeyVault = configuration["CertificateNameKeyVault"],
//                KeyVaultEndpoint = configuration["AzureKeyVaultEndpoint"],

//                // development certificate
//                DevelopmentCertificatePfx = Path.Combine(environment.ContentRootPath, "sts_dev_cert.pfx"),
//                DevelopmentCertificatePassword = "1234" //configuration["DevelopmentCertificatePassword"] 
//            };

//            (X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate)
//                certs = await GetCertificates(certificateConfiguration).ConfigureAwait(false);
//            return certs;
//        }

//        public static async Task<(X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate)> GetCertificates(CertificateConfiguration certificateConfiguration)
//        {
//            (X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate) certs = (null, null);

//            if (certificateConfiguration.UseLocalCertStore)
//            {
//                Log.Warning("certificateConfiguration.UseLocalCertStore");

//                using X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
//                store.Open(OpenFlags.ReadOnly);
//                var storeCerts = store.Certificates.Find(
//                    X509FindType.FindByThumbprint,
//                    certificateConfiguration.CertificateThumbprint,
//                    false);

//                certs.ActiveCertificate = storeCerts[0];
//                store.Close();
//            }
//            else
//            {
//                Log.Warning("certificateConfiguration.UseLocalCertStore = false");

//                if (!string.IsNullOrEmpty(certificateConfiguration.KeyVaultEndpoint))
//                {
//                    var keyVaultCertificateService = new KeyVaultCertificateService(
//                        certificateConfiguration.KeyVaultEndpoint,
//                        certificateConfiguration.CertificateNameKeyVault);

//                    certs = await keyVaultCertificateService
//                        .GetCertificatesFromKeyVault().ConfigureAwait(false);
//                }
//            }

//            // search for local PFX with password, usually local dev
//            if (certs.ActiveCertificate == null)
//            {
//                certs.ActiveCertificate = new X509Certificate2(
//                    certificateConfiguration.DevelopmentCertificatePfx,
//                    certificateConfiguration.DevelopmentCertificatePassword);
//            }

//            return certs;
//        }
//    }
//}
