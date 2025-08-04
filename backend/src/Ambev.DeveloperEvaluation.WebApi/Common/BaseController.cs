using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected Guid GetCurrentUserId() =>
            Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    protected IActionResult Ok<T>(T data, string message = "Operation successful") =>
            base.Ok(new ApiResponseWithData<T> { Data = data, Success = true, Message = message });

    protected IActionResult FailedResponse(IEnumerable<ValidationFailure> errors, string message = "Validation error") =>
        base.BadRequest(new ApiResponse { Success = false, Message = message, Errors = errors.Select(o => (ValidationErrorDetail)o) });

    protected IActionResult Sucess(string message = "Operation successful") =>
             base.Ok(new ApiResponse { Success = true, Message = message });

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse { Message = message, Success = false });

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
            base.Ok(new PaginatedResponse<T>
            {
                Data = pagedList.Data,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                AvailableItems = pagedList.AvailableItems,
                Success = true
            });
}
