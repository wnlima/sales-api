using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.Commands;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.CreateSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.GetSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.ListSales;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CancelSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CreateSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.DTOs;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.GetSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.ListSales;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.Validators;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer
{
    /// <summary>
    /// Controller for managing sale operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The sale creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        [HttpPost]
        [Authorize(Policy = "AnyRole")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var command = _mapper.Map<CreateSaleCommand>(request);
            command.CustomerId = GetCurrentUserId();

            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }

        /// <summary>
        /// Retrieves a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "AnyRole")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetSaleByIdRequest(id, this.GetCurrentUserId());
            var validator = new GetSaleByIdRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<GetSaleByIdCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<GetSaleResponse>(response), "Sale retrieved successfully");
        }

        /// <summary>
        /// Cancels a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale to cancel</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response if the sale was successfully canceled</returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AnyRole")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new CancelSaleRequest(id, this.GetCurrentUserId());
            var validator = new CancelSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<CancelSaleCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Sucess("Sale deleted successfully");
        }

        [HttpGet]
        [Authorize(Policy = "AnyRole")]
        [SwaggerOperation(
        Summary = "List sales with advanced filtering, ordering and pagination",
        Description = """
    Supports dynamic filtering through query string parameters.

    | Filter Type  | Example (Query String)
    | ------------ | ----------------------
    | Equals       | `?name=Beer`          
    | Contains     | `?name=*bee*`         
    | Starts With  | `?name=bee*`          
    | Ends With    | `?name=*beer`         
    | Greater Than | `?_minprice=10`       
    | Less Than    | `?_maxprice=100`      

    #### Ordering
    Use `_order` query param with comma-separated fields.
    Example: `?_order=price desc,name asc`

    #### Pagination
    Supported via:
    - `_size` (default: 1)
    - `_page` (default: 10)
    """
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful request", typeof(PaginatedList<ListSalesResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error", typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No results found", typeof(ApiResponse))]
        [ProducesResponseType(typeof(PaginatedList<ListSalesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List(CancellationToken cancellationToken,
        [FromQuery] Dictionary<string, string>? filters = null)
        {
            var request = new ListSalesRequest(GetCurrentUserId(), filters);
            var validator = new ListSalesRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<ListSalesCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);
            var ret = _mapper.Map<ListSalesResponse>(result);

            return OkPaginated(ret);
        }
    }
}