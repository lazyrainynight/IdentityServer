// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Configuration.RequestProcessing;
using Duende.IdentityServer.Configuration.Configuration;
using Duende.IdentityServer.Configuration.Validation.DynamicClientRegistration;
using Duende.IdentityServer.Configuration.Validation;

namespace IdentityServerHost;

internal class CustomClientRegistrationProcessor : DynamicClientRegistrationRequestProcessor
{
    private readonly ICollection<Client> _clients;

    public CustomClientRegistrationProcessor(
        IdentityServerConfigurationOptions options,
        IClientConfigurationStore store,
        ICollection<Client> clients) : base(options, store)
    {
        _clients = clients;
    }

    protected override async Task<ValidationStepResult> AddClientId(DynamicClientRegistrationValidationContext context)
    {
        if (context.Request.Extensions.TryGetValue("client_id", out var clientIdParameter))
        {
            var clientId = clientIdParameter.ToString();
            if(_clients.Any(c => c.ClientId == clientId))
            {
                return new ValidationStepFailure(
                    error: "Duplicate client id",
                    errorDescription: "Attempt to register a client with a client id that has already been registered"
                );
            } 
            else
            {
                context.Client.ClientId = clientId;
                return new ValidationStepSuccess();
            }
        }
        return await base.AddClientId(context);
    }

    protected override async Task<ValidationStepResult> GenerateSecret(DynamicClientRegistrationValidationContext context)
    {
         if(context.Request.Extensions.TryGetValue("client_secret", out var secretParam))
        {
            var plainText = secretParam.ToString();
            var secret = new Secret(plainText.Sha256());
            
            context.Items["secret"] = secret;
            context.Items["plainText"] = plainText;

            return new ValidationStepSuccess();
        }
        else
        {
            return await base.GenerateSecret(context);
        }

    }
}