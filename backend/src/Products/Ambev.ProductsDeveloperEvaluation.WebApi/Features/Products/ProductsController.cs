using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Validators;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products
{
    /// <summary>
    /// Controller for managing product operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of ProductsController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="request">The product creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created product details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateProductCommandValidator();
            var command = _mapper.Map<CreateProductCommand>(request);

            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateProductResponse>
            {
                Success = true,
                Message = "Product created successfully",
                Data = _mapper.Map<CreateProductResponse>(response)
            });
        }

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="request">The product update request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated product details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateProductCommandValidator();
            var command = _mapper.Map<UpdateProductCommand>(request);
            command.Id = id;

            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<UpdateProductResponse>(response), "Product updated successfully");
        }

        /// <summary>
        /// Retrieves a product by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The product details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetProductByIdRequest(id);
            var validator = new GetProductByIdRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<GetProductByIdCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(
                 _mapper.Map<GetProductResponse>(response),
                 "Product retrieved successfully"
            );
        }

        /// <summary>
        /// Deletes a product by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response if the product was deleted</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new DeleteProductRequest(id);
            var validator = new DeleteProductRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<DeleteProductCommand>(request.Id);
            await _mediator.Send(command, cancellationToken);

            return Sucess("Product deleted successfully");
        }


        [HttpGet]
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
        [SwaggerResponse(StatusCodes.Status200OK, "Successful request", typeof(PaginatedList<ListProductsResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error", typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No results found", typeof(ApiResponse))]
        [ProducesResponseType(typeof(PaginatedList<ListProductsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListProducts(CancellationToken cancellationToken, [FromQuery] Dictionary<string, string>? filters = null)
        {
            var request = new ListProductsRequest(filters);
            var validator = new ListProductsRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return this.FailedResponse(validationResult.Errors);

            var command = _mapper.Map<ListProductsCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);
            var ret = _mapper.Map<ListProductsResponse>(result);

            return OkPaginated(ret);
        }
    }
}
