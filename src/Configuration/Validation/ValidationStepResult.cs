// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Configuration.Validation.DynamicClientRegistration;

namespace Duende.IdentityServer.Configuration.Validation;

/// <summary>
/// Represents the result of a step in the dynamic client registration validator.
/// </summary>
public abstract class ValidationStepResult 
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorDescription"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Task<ValidationStepResult> Failure(string errorDescription,
        string error = DynamicClientRegistrationErrors.InvalidClientMetadata) =>
            Task.FromResult<ValidationStepResult>(new ValidationStepFailure(
                    error,
                    errorDescription
                ));

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Task<ValidationStepResult> Success() =>
        Task.FromResult<ValidationStepResult>(new ValidationStepSuccess());
}

/// <summary>
/// Represents a successful validation step.
/// </summary>
public class ValidationStepSuccess : ValidationStepResult
{
}

/// <summary>
/// Represents a failed validation step.
/// </summary>
public class ValidationStepFailure : ValidationStepResult
{
    /// <summary>
    /// The validation error.
    /// </summary>
    public DynamicClientRegistrationValidationError Error { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationStepFailure" />
    /// class.
    /// </summary>
    /// <param name="error">The error code for the error that occurred during
    /// validation. Error codes defined by RFC 7591 are defined as constants in
    /// the <see cref="DynamicClientRegistrationErrors" /> class.</param>
    /// <param name="errorDescription">A human-readable description of the error
    /// that occurred during validation.</param>
    public ValidationStepFailure(string error, string errorDescription)
    {
        Error = new DynamicClientRegistrationValidationError(error, errorDescription);
    }
}
