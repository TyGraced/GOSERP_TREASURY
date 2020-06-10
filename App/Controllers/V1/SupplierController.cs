using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.Commands.Supplier;
using GODPAPIs.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puchase_and_payables.Contracts.Commands.Supplier.setup;
using Puchase_and_payables.Contracts.Queries.Supplier;
using Puchase_and_payables.Contracts.V1;
using System.Threading.Tasks;

namespace Puchase_and_payables.Controllers.V1
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupplierController : Controller
    {
        private readonly IMediator _mediator;
        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.ADD_UPDATE_TERMS)]
        public async Task<ActionResult> ADD_UPDATE_TERMS([FromBody] AddUpdateServiceTermCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.ADD_UPDATE_SUPPLIER_TYPE)]
        public async Task<ActionResult> ADD_UPDATE_SUPPLIER_TYPE([FromBody] AddUpdateSuppliertypeCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost(ApiRoutes.SupplierEndpoints.ADD_UPDATE_TASK_SETUP)]
        public async Task<ActionResult> ADD_UPDATE_TASK_SETUP([FromBody] AddUpdateTasksetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_TERMS)]
        public async Task<ActionResult> GET_ALL_TERMS()
        {
            var query = new GetAllServiceTermsQuery();
            return Ok(await _mediator.Send(query)); 
        }
        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_SUPPLIER_TYPE)]
        public async Task<ActionResult> GET_ALL_SUPPLIER_TYPE()
        {
            var query = new GetAllSupplierTypeQuery();
            return Ok(await _mediator.Send(query));
        }
        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_TASK_SETUP)]
        public async Task<ActionResult> GET_ALL_TASK_SETUP()
        {
            var query = new GetAllTaskSetupQuery();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_TERMS)]
        public async Task<ActionResult> GET_TERMS([FromQuery] GetServiceTermsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpGet(ApiRoutes.SupplierEndpoints.GET_SUPPLIER_TYPE)]
        public async Task<ActionResult> GET_SUPPLIER_TYPE([FromQuery] GetSupplierTypeQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpGet(ApiRoutes.SupplierEndpoints.GET_TASK_SETUP)]
        public async Task<ActionResult> GET_TASK_SETUP([FromQuery] GetTaskSetupQuery query)
        {
            return Ok(await _mediator.Send(query));
        }


        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_SUPPLIERS)]
        public async Task<ActionResult> GetAllSuppliers()
        {
            var query = new GetAllSupplierQuery();
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_SUPPLIER)]
        public async Task<ActionResult> GetSupplier([FromQuery]GetSupplierQuery query)
        {
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_SUPPLIER_AUTHORIZATIONS)]
        public async Task<ActionResult> GetAllSupplierAuths()
        {
            var query = new GetAllSupplierAuthorizationQuery();
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }


        [HttpGet(ApiRoutes.SupplierEndpoints.GET_SUPPLIER_AUTHORIZATION)]
        public async Task<ActionResult> GetSupplier([FromQuery] GetSupplierAuthorization query)
        {
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_SUPPLIER_BUSINESS_OWNER)]
        public async Task<ActionResult> GetAllSupplierBus()
        {
            var query = new GetAllSupplierBusinessOwnerQuery();
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }


        [HttpGet(ApiRoutes.SupplierEndpoints.GET_SUPPLIER_BUSINESS_OWNER)]
        public async Task<ActionResult> GetSupplierBus([FromQuery] GetSupplierBusinessOwnerQuery query)
        {
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_SUPPLIER_DOCUMENTS)]
        public async Task<ActionResult> GetAllSupplierDocs()
        {
            var query = new GetAllSupplierDocumentQuery();
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_SUPPLIER_DOCUMENT)]
        public async Task<ActionResult> GetSupplierBus([FromQuery] GetSupplierDocumentQuery query)
        {
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_TOP_CLIENTS)]
        public async Task<ActionResult> GetAllSupplierTopClients()
        {
            var query = new GetAllSupplierTopClientQuery();
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_SUPPLIER_TOP_CLIENT)]
        public async Task<ActionResult> GetSupplierClient([FromQuery] GetSupplierTopClientQuery query)
        {
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }


        [HttpGet(ApiRoutes.SupplierEndpoints.GET_ALL_TOP_SUPPLIERS)]
        public async Task<ActionResult> GetAllSupplierTopSuppliers()
        {
            var query = new GetAllSupplierTopSupplierQuery();
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_SUPPLIER_TOP_SUPPLIER)]
        public async Task<ActionResult> GetSupplierTopSupplier([FromQuery] SupplierTopSupplierQuery query)
        {
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.UPDATE_SUPPLIER)]
        public async Task<ActionResult> UpdateSuppliers([FromBody] UpdateSupplierCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.UPDATE_SUPPLIER_AUTHORIZATION)]
        public async Task<ActionResult> UpdateSupAuth([FromBody] UpdateSupplierAuthorizationCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.UPDATE_SUPPLIER_BUSINESS_OWNER)]
        public async Task<ActionResult> UpdateSupAuth([FromBody] UpdateSupplierBuisnessOwnerCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.UPDATE_SUPPLIER_DOCUMENT)]
        public async Task<ActionResult> UpdateSupDoc([FromBody] UpdateSupplierDocumentCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.UPDATE_SUPPLIER_TOP_CLIENT)]
        public async Task<ActionResult> UpdateTopClient([FromBody] UpdateSupplierTopClientCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.UPDATE_SUPPLIER_TOP_SUPPLIER)]
        public async Task<ActionResult> UpdateTopSup([FromBody] UpdateSupplierTopSupplierCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.DELETE_SUPPLIER)]
        public async Task<ActionResult> DeleteSupplier([FromBody] DeleteSupplierCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.SupplierEndpoints.DELETE_SUPPLIER_AUTHORIZATION)]
        public async Task<ActionResult> DeleteSupplierAuth([FromBody] DeleteSupplierAuthorizationCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost(ApiRoutes.SupplierEndpoints.DELETE_SUPPLIER_BUSINESS_OWNER)]
        public async Task<ActionResult> DeleteBusOwn([FromBody] DeleteSupplierBuisnessOwnerCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost(ApiRoutes.SupplierEndpoints.DELETE_SUPPLIER_DOCUMENT)]
        public async Task<ActionResult> DeleteSupDoc([FromBody] DeleteSupplierDocumentCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost(ApiRoutes.SupplierEndpoints.DELETE_SUPPLIER_TOP_CLIENT)]
        public async Task<ActionResult> DeleteSupDoc([FromBody] DeleteSupplierTopClientCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost(ApiRoutes.SupplierEndpoints.DELETE_SUPPLIER_TOP_SUPPLIER)]
        public async Task<ActionResult> DeleteSupDoc([FromBody] DeleteSupplierTopSupplierCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.SupplierEndpoints.GET_AWAITING_APPROVALS)]
        public async Task<ActionResult> GET_AWAITING_APPROVALS()
        {
            var query = new GetAllSupplierDataAwaitingApprovalQuery();
            return Ok(await _mediator.Send(query));
        } 
    }
}
