using Microsoft.AspNetCore.Mvc;
using SalesService.Models;
using SalesService.Kafka;
using SalesService.Data;
using System.Threading.Tasks;

namespace SalesService.Controllers;

[ApiController]
[Route("sales")]
public class SalesController : ControllerBase
{
    private readonly KafkaProducerService _kafka;
    private readonly SalesDbContext _context;

    public SalesController(KafkaProducerService kafka, SalesDbContext context)
    {
        _kafka = kafka;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] Sale sale)
    {
        _context.Orders.Add(sale);
        await _context.SaveChangesAsync();

        await _kafka.PublishSaleAsync(sale);
        return Ok(new { message = "Sale saved and event published", sale });
    }
}
