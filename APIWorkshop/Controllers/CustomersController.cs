using APIWorkshop.Controllers.Dto;
using APIWorkshop.Data;
using APIWorkshop.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkshop.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase {
        private readonly BelleCroissantContext _context;

        public CustomersController(BelleCroissantContext belleCroissantContext) {
            _context = belleCroissantContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers() {
            try {
                var customers = await _context.Customers.ToListAsync();

                if (customers == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum cliente na base de dados!" });
                }

                return Ok(customers);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerForId(int id) {
            try {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum cliente com esse id" });
                }

                return Ok(customer);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CreateCustomerDto customerDto) {
            try {
                if (customerDto == null) {
                    return BadRequest(new { error = "Bad Request", message = "Dados do cliente inválidos ou incompleto" });
                }

                var customer = new Customer {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    Age = customerDto.Age,
                    Gender = customerDto.Gender,
                    PostalCode = customerDto.PostalCode,
                    PhoneNumber = customerDto.PhoneNumber,
                    MembershipStatus = customerDto.MembershipStatus,

                    /* Valores padrões que não precisam obrigatoriamente serem passados pelo usuario
                       Pois, o objetivo é criar um cliente novo, e esses valores serão preenchidos automaticamente
                    */

                    JoinDate = DateTime.UtcNow, 
                    TotalSpending = 0,          
                    AverageOrderValue = 0,      
                    Frequency = 0,              
                    PreferredCategory = "",     
                    Churned = false,            
                    Orders = null
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCustomers), new { id = customer.CustomerId }, customer);

            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> PutCostumer(int id, [FromBody] UpdateCustomerDto updateCustomerDto) {
            try {
            
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum cliente com esse id" });
                }


                // Atualiza os campos do cliente com os dados do DTO
                customer.FirstName = updateCustomerDto.FirstName ?? customer.FirstName;
                customer.LastName = updateCustomerDto.LastName ?? customer.LastName;
                customer.Email = updateCustomerDto.Email ?? customer.Email;
                customer.Age = updateCustomerDto.Age ?? customer.Age;
                customer.Gender = updateCustomerDto.Gender ?? customer.Gender;
                customer.PostalCode = updateCustomerDto.PostalCode ?? customer.PostalCode;
                customer.PhoneNumber = updateCustomerDto.PhoneNumber ?? customer.PhoneNumber;       
                customer.MembershipStatus = updateCustomerDto.MembershipStatus ?? customer.MembershipStatus;


                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(customer);

            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }
        }
     } 
}
