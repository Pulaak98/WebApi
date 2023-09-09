using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApi.DatabaseContext;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly WebApiDbContext webApiDbcontext;

        public ProductsController(WebApiDbContext webApiDbcontext)
        {
            this.webApiDbcontext = webApiDbcontext;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {

            var productCreate = new Product()
            {
                ProductName = product.ProductName,
                Quatity=product.Quatity,
                Price=product.Price,
            };
            // await webApiDbcontext.UserInfo.AddAsync(//userCreate);
            //await webApiDbcontext.SaveChangesAsync();
            await webApiDbcontext.Database.ExecuteSqlInterpolatedAsync($@"EXEC InsertProduct @ProductName={productCreate.ProductName},@ProductQuantity={productCreate.Quatity},@ProductPrice={productCreate.Price}");
            return Ok(productCreate);
        }

        [HttpGet]
        public async Task<IActionResult>ViewProduct()
        {
            var product= await webApiDbcontext.Products.FromSqlRaw("EXEC GetAllProduct").ToListAsync();
            
            if(product == null)
            {
                return BadRequest();
            }
            else { return Ok(product); }
        }

        [HttpGet("EditViewProduct")]
        public async Task<IActionResult> ViewEditProduct(int id)
        {
            //var product = await webApiDbcontext.Products.FromSqlRaw($"EXEC GetProduct @ProductId={0}",id).FirstOrDefaultAsync();

            var product = await webApiDbcontext.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return BadRequest();
            }
            else {
                var updateProduct = new UpdateProduct()
                {
                    ProductId=product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quatity = product.Quatity
                };
                return Ok(updateProduct);   
            }
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult>UpdateProduct(UpdateProduct productUpdate)
        {
            var product = await webApiDbcontext.Products.FindAsync(productUpdate.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                product.ProductName = productUpdate.ProductName;
                product.Price = productUpdate.Price;
                product.Quatity = productUpdate.Quatity;

                await webApiDbcontext.Database.ExecuteSqlInterpolatedAsync(
        $@"EXEC UpdateProduct @pId={product.ProductId},@pName={product.ProductName},@pPrice={product.Price},@pQuantity={product.Quatity}");
                return (Ok(product));
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteProduct(UpdateProduct deleteProduct)
        {
            var product =await webApiDbcontext.Products.FindAsync(deleteProduct.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                await webApiDbcontext.Database.ExecuteSqlInterpolatedAsync($@"EXEC DeleteProduct @pId={product.ProductId}");
                return NoContent();
            }
        }
    }
}
