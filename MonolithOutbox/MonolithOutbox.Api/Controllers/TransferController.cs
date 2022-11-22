using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransferModule.Application;
using TransferModule.Domain.Model;

namespace MonolithOutbox.Api.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class TransferController :
     ControllerBase
   {
      private readonly ITransferManager _transferManager;

      public TransferController(ITransferManager transferManager)
      {
         _transferManager = transferManager;
      }

      // GET api/transfer
      [HttpGet]
      public ActionResult<IEnumerable<TransferLog>> Get()
      {
         return Ok(_transferManager.GetTransferLogs());
      }
   }
}
