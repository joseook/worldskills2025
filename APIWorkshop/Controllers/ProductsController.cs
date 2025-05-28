using APIWorkshop.Controllers.Dto;
using APIWorkshop.Data;
using APIWorkshop.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWorkshop.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {

        private readonly BelleCroissantContext _context;

        public ProductsController(BelleCroissantContext belleCroissantContext) {
            _context = belleCroissantContext;
        }
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {
            try {
                var products = await _context.Products.ToListAsync();
                if (products == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum produto na base de dados!" });
                }
                
                return Ok(products);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductForId (int id) {
            try { 
                var product = await _context.Products.FindAsync(id);
                if (product == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum produto com esse id" });
                }
                
                return Ok(product);

            } catch (Exception ex) {
                  return StatusCode(StatusCodes.Status500InternalServerError, $"Error na captura do banco de dados, {ex.Message} ");
            }
        }

       
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto createProductDto) {

            try {
                if (createProductDto == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum produto na base de dados!" });
                }

                var products = new Product {
                    ProductName = createProductDto.ProductName,
                    Category = createProductDto.Category,
                    Ingredients = createProductDto.Ingredients,
                    Price = createProductDto.Price,
                    Cost = createProductDto.Cost,
                    Seasonal = createProductDto.Seasonal,
                    Active = createProductDto.Active,
                    IntroducedDate = createProductDto.IntroducedDate
                };

                _context.Products.Add(products);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProducts), new { id = products.ProductId }, products);

            } catch(Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na criação do produto, {ex.Message} ");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto) {
            try {

                if (updateProductDto == null) {
                    return BadRequest(new { error = "Bad Request", message = "Dados do produto inválidos ou incompleto" });
                }

                var product = await _context.Products.FindAsync(id);
                if (product == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum produto com esse id" });
                }

                product.ProductName = updateProductDto.ProductName ?? product.ProductName;    
                product.Category = updateProductDto.Category ?? product.Category;
                product.Ingredients = updateProductDto.Ingredients ?? product.Ingredients;
                product.Price = updateProductDto.Price ?? product.Price; 
                product.Active = updateProductDto.Active ?? product.Active; 
                product.Cost = updateProductDto.Cost ?? product.Cost; 
                product.Seasonal = updateProductDto.Seasonal ?? product.Seasonal;


                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return Ok(product);

            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na atualização do produto, {ex.Message} ");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id) {
            try {

                var product = await _context.Products.FindAsync(id);
                if (product == null) {
                    return NotFound(new { error = "Não encontrado", message = "Não existe nenhum produto com esse id" });
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Produto removido com sucesso!" });
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error na remoção do produto, {ex.Message} ");
            }
        }
    }

    
}
