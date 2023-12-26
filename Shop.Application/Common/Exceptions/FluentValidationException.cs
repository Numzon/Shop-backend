using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Common.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class FluentValidationException : Exception
{
	public FluentValidationException() : base("One or more validation failures have occured.")
	{
		Errors = new Dictionary<string, string[]>();	
	}

	public FluentValidationException(IEnumerable<ValidationFailure> failures) : this()
	{
        Errors = failures
           .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
           .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    } 

	public IDictionary<string,string[]> Errors { get; }
}
