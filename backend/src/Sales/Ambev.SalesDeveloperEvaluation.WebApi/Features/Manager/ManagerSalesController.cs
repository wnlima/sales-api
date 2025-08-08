using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.Commands;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.GetSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.ListSales;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.CancelSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.DTOs;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.GetSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.ListSales;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.Validators;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager
{
    /// <summary>
    /// Controller for managing sale operations
    /// </summary>
    [ApiController]
    [Route("api/manager/sales")]
    public class ManagerSalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ManagerSalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrManager")]
        [ProducesResponseType(typeof(ApiResponseWithData<ManagerGetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new ManagerGetSaleByIdRequest(id);
            var validator = new ManagerGetSaleByIdRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<ManagerGetSaleByIdCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<ManagerGetSaleResponse>(response), "Sale retrieved successfully");
        }

        /// <summary>
        /// Cancels a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale to cancel</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response if the sale was successfully canceled</returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrManager")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromRoute] Guid customerId, CancellationToken cancellationToken)
        {
            var request = new ManagerCancelSaleRequest(id);
            var validator = new ManagerCancelSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<ManagerCancelSaleCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Sucess("Sale deleted successfully");
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManager")]
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
        [SwaggerResponse(StatusCodes.Status200OK, "Successful request", typeof(PaginatedList<ManagerListSalesResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error", typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No results found", typeof(ApiResponse))]
        [ProducesResponseType(typeof(PaginatedList<ManagerListSalesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List(CancellationToken cancellationToken,
        [FromQuery] Dictionary<string, string>? filters = null)
        {
            var request = new ManagerListSalesRequest(filters);
            var validator = new ManagerListSalesRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<ManagerListSalesCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);
            var ret = _mapper.Map<ManagerListSalesResponse>(result);


            return OkPaginated(ret);
        }
    }
}