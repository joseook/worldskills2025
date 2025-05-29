using APIWorkshop.Controllers.Dto;
using APIWorkshop.Data;
using APIWorkshop.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APIWorkshop.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {

        private readonly BelleCroissantContext _context;

        public OrdersController(BelleCroissantContext belleCroissantContext) {
            _context = belleCroissantContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> ListOrders() {
            try {
                var orders = await _context.Orders.Include(o => o.Customer)
                                                   .Include(o => o.Product)
                                                   .ToListAsync();
                if (orders == null || !orders.Any()) {
                    return NotFound(new { error = "Não encontrado", message = "Não existem pedidos na base de dados!" });
                }


                return Ok(orders);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderByID(int id) {
            try {

                var order = await _context.Orders.Include(o => o.Customer)
                                                  .Include(o => o.Product)
                                                  .FirstOrDefaultAsync(o => o.TransactionId == id);
                if (order == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum pedido com esse id" });
                }

                return Ok(order);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }

        }

        [HttpPost()]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto) {
            try {

                if (orderDto == null) {
                    return BadRequest(new { error = "Bad Request", message = "Dados do pedido inválidos ou incompletos" });
                }
                var order = new Order {
                    CustomerId = orderDto.CustomerId,
                    ProductId = orderDto.ProductId,
                    Quantity = orderDto.Quantity,
                    Price = orderDto.Price,
                    PaymentMethod = orderDto.PaymentMethod,
                    Channel = orderDto.Channel

                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetOrderByID), new { id = order.TransactionId }, order);

            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult<Order>> CompleteOrder(int id) {
            try {
                var order = await _context.Orders.FindAsync(id);
                if (order == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum pedido com esse id" });
                }
                order.Date = DateTime.Now;
                order.Time = DateTime.Now.ToString("HH:mm:ss");
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return Ok(order);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na atualização do pedido, {ex.Message} ");
            }
        }

        [HttpDelete("{id}/cancel")]
        public async Task<ActionResult> CancelOrder(int id) {
            try {
                var order = await _context.Orders.FindAsync(id);
                if (order == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum pedido com esse id" });
                }
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return Ok("Pedido deletado com sucesso!.");
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na remoção do pedido, {ex.Message} ");
            }
        }
    }
}