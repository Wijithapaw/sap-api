using Microsoft.AspNetCore.Mvc;
using SAP.Api.Authorization;
using SAP.Domain.Constants;
using SAP.Domain.Dtos;
using SAP.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SAP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<List<TransactionDto>> Get(TransactionSearchDto filter)
        {
            return await _transactionService.SearchAsync(filter);
        }

        [HttpGet("{id}")]
        public async Task<TransactionDto> Get(string id)
        {
            return await _transactionService.GetAsync(id);
        }

        [HttpPost]
        public async Task<string> Post(TransactionDto dto)
        {
            return await _transactionService.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, TransactionDto dto)
        {
            await _transactionService.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _transactionService.DeleteAsync(id);
        }

        [HttpPatch("reconcile/{id}")]
        [ClaimAuthorize(CustomClaims.TransactionReconcile)]
        public async Task Reconcile(string id)
        {
            await _transactionService.ReconcileAsync(id);
        }

        [HttpPatch("unreconcile/{id}")]
        [ClaimAuthorize(CustomClaims.TransactionReconcile)]
        public async Task UnReconcile(string id)
        {
            await _transactionService.UnReconcileAsync(id);
        }
    }
}
